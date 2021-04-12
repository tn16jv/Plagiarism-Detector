using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Floss.Database.Models;
using Floss.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Floss.Api.Auth {

    /// <summary>
    /// This utility class provides functionality to handle STS token verification (both implicit token and code flow).
    /// 
    /// It also provides simple example mechanism for the application-specific token generation.
    /// </summary>
    public class OAuthUtil {
        static string scopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";
        private readonly ILogger Logger;
        private IOptions<OAuthProviderConfig> OauthCfg { get; }
        private StsOptions StsCfg { get; }

        public OAuthUtil(ILogger<OAuthUtil> logger, IOptions<OAuthProviderConfig> oauthCfg, StsOptions stsCfg) {
            this.Logger = logger;
            this.OauthCfg = oauthCfg;
            this.StsCfg = stsCfg;
        }

        /// <summary>
        /// This method is part of OAuth2 code flow.
        /// It calls STS TokenControler with authorization code, clientId and secret in order to retrieve actual JWT access token.
        /// </summary>
        /// <param name="code">Authorization code provided by STS</param>
        /// <returns></returns>
        public JwtResponse GetAuthTokenByCode(string code) {
            var encAscii = ASCIIEncoding.ASCII;
            JwtResponse rz = null;
            try {
                var oauthCfg = OauthCfg.Value;
                var rqt = WebRequest.CreateHttp(OauthCfg.Value.TokenControllerAddress);
                rqt.Method = "POST";
                rqt.ContentType = "application/x-www-form-urlencoded";
                rqt.UserAgent = oauthCfg.ClientId;

                // Add ClientID and Secret as an authorization header
                rqt.Headers.Add(HttpRequestHeader.Authorization,
                    new AuthenticationHeaderValue(
                        scheme: "Basic",
                        parameter: Convert.ToBase64String(encAscii.GetBytes(string.Format("{0}:{1}", oauthCfg.ClientId, oauthCfg.ClientSecret)))
                    ).ToString());

                // Here we just construct form data that will be specified inside request
                // NameValueCollection from HttpUtility will automatically do encoding end proper formatting.
                NameValueCollection formData = HttpUtility.ParseQueryString(String.Empty);
                formData.Add("grant_type", "authorization_code");
                formData.Add("code", code);
                formData.Add("resource", oauthCfg.Resource);
                formData.Add("client_id", oauthCfg.ClientId);
                byte[] formBytes = encAscii.GetBytes(formData.ToString());

                rqt.ContentLength = formBytes.Length;
                using (Stream hs = rqt.GetRequestStream()) {
                    hs.Write(formBytes, 0, formBytes.Length);
                    hs.Flush();
                    hs.Close();
                }

                var response = rqt.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream(), encAscii)) {
                    var rsp = reader.ReadToEnd();
                    // Parse the response from STS Token service
                    rz = JsonExtensions.DeserializeFromJson<JwtResponse>(rsp);
                    System.Diagnostics.Debug.WriteLine(rz);
                }
            }
            catch (Exception x) {
                Logger.LogError(x, "Failed to get STS JWT by code '{0}'", code);
                throw;
            }
            return rz;
        }

        /// <summary>
        /// This method validates JWT response data from STS.
        /// Depending on flow there can be <c>refresh token</c> and expiration time as a separate fileds.
        /// Access token must always be present.
        /// <seealso cref="https://blogs.msdn.microsoft.com/vbertocci/2012/11/20/introducing-the-developer-preview-of-the-json-web-token-handler-for-the-microsoft-net-framework-4-5/"/>
        /// </summary>
        /// <param name="rsp">Response data from STS. The only mandatory field is <see cref="JwtResponse.access_token"/></param>
        /// <returns></returns>
        public JwtAuthResult ValidateStsJwtResponse(JwtResponse rsp) {
            JwtAuthResult rz = new JwtAuthResult();
            SecurityToken secToken;
            DateTime expTime;
            rz.AccessToken = rsp.access_token;
            rz.RefreshToken = rsp.refresh_token;
            if (rsp.TryGetUtcExpiresAt(out expTime)) {
                rz.TokenExpirationUtc = expTime;
            }

            // Here we have text of the token and additional parameters per JwtResponse
            // Here is an example of token validation: https://blogs.msdn.microsoft.com/vbertocci/2012/11/20/introducing-the-developer-preview-of-the-json-web-token-handler-for-the-microsoft-net-framework-4-5/

            JwtSecurityTokenHandler jwtHnd = new JwtSecurityTokenHandler(); // Prepare JWT token handler using data from STS Federation metdata

            // Get token validation parameters for STS token
            TokenValidationParameters stsTokenValidationParams = this.StsCfg.GetStsTokenValidationParameters();
            // Here you can adjust some parameters if necessary
            stsTokenValidationParams.NameClaimType = ClaimTypes.NameIdentifier; // Defines what claim will be used as result ClaimsIdentity.NameClaimType

            try {
                // Validate token
                rz.Principal = jwtHnd.ValidateToken(rsp.access_token, stsTokenValidationParams, out secToken);
                rz.Jwt = secToken as JwtSecurityToken;

                // This is how you can verify that this is not user impersonation (i.e. one user works on behalf of another).
                // User impersonation is normal (e.g. some service works on behalf of user), this is just an example on how you can check it if you need.
                // if the token is scoped, verify that required permission is set in the scope claim
                var tokScope = rz.Principal.FindFirst(scopeClaimType)?.Value;
                if (!string.IsNullOrEmpty(tokScope) && (tokScope != "user_impersonation")) {
                    rz.UserErrorMsg = string.Format("Granted scope \"{0}\" is forbidden. Access denied.", tokScope);
                }

                return rz;
            }



            catch (SecurityTokenValidationException vx) {
                rz.UserErrorMsg = string.Format("Invalid authentication result: {0}. Access denied.", vx.Message);
            }
            catch (Exception x) {
                Logger.LogError(x, "Failed to validate STS JWT");
                rz.UserErrorMsg = string.Format("Technical error Ref# {0}.\r\nContact system administrator for assistance.", "n/a");
            }
            return rz;
        }
        public AppAuthRsp GetByToken(string accessToken)
        {
            accessToken = accessToken == null ? accessToken : accessToken.Replace("Bearer ", "");
            try
            {
                var xrz = this.ValidateStsJwtResponse(new JwtResponse()
                {
                    access_token = accessToken
                });

                if (xrz.IsFaulted)
                {
                    return new AppAuthRsp()
                    {
                        Error = xrz.UserErrorMsg
                    };
                }

                var arz = this.GetAppSpecificJwt(xrz.Principal);


                int expireTimeMinutes = 15;

                return new AppAuthRsp()
                {
					UserId = arz.UserId,
                    AppJwt = arz.AccessToken,
                    Principal = arz.Principal,
                    UserName = arz.Principal.Identity.Name,
                    Email = arz.Principal.Claims.ToList()[1].Value,
                //Email = arz.Principal.FindFirst(System.IdentityModel.Claims.ClaimTypes.Email).Value,
                    ExpireDateTime = DateTime.Now.AddMinutes(expireTimeMinutes),
                };
            }
            catch (Exception x)
            {
                return new AppAuthRsp() { Error = x.Message };
            }
        }


        /// <summary>
        /// Process authorization code returned from STS Token controller.
        /// This method obtains JWT from STS and converts it to the application-specific JWT via <see cref="ValidateStsJwtResponse"/>
        /// </summary>
        /// <param name="authCode">Authorization code received from STS</param>
        /// <returns></returns>
        public JwtAuthResult ValidateAuthCode(string authCode) {
            var jwtRsp = GetAuthTokenByCode(authCode);
            JwtAuthResult rz = new JwtAuthResult();
            if (jwtRsp == null) {
                rz.UserErrorMsg = "Invalid response from authentication provider. Access denied.";
                return rz;
            }
            return ValidateStsJwtResponse(jwtRsp);
        }


        /// <summary>
        /// Create application-specific token by the specified principal.
        /// This is just an example implementation.
        /// 
        /// It is up to specific application project team to define what claims are required by your application in both STS and 
        /// app-specific tokens.
        /// </summary>
        /// <param name="stsPrinc">Principal produced from STS JWT token</param>
        /// <returns></returns>
        public JwtAuthResult GetAppSpecificJwt(ClaimsPrincipal stsPrinc) {
            var cfg = this.OauthCfg.Value;
            var symmetricKey = Convert.FromBase64String(cfg.AppJwtSecret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.AuthenticationInstant, now.ToString("o")));
            // Here you build claims for your application-specific user identity
            var cl = stsPrinc.FindFirst(ClaimTypes.Upn);
            if (cl == null) {
                claims.Add(new Claim(ClaimTypes.Email, "not@available"));
                claims.Add(new Claim(ClaimTypes.Name, "No Available"));
            }
            else {
                claims.Add(new Claim(ClaimTypes.Email, cl.Value));
                int ix = cl.Value.IndexOf('@');
                claims.Add(new Claim(ClaimTypes.Name, (ix>=0) ? cl.Value.Substring(0, ix) : cl.Value));
            }

            var user = this.GetUser(cl.Value);
            //string roleList = employee.EmployeeRoles.Select(er => (string.Format({ 0} er.AuthorizationRoleTypeId.ToString() + ",").FirstOrDefault();
            if (user != null)
            {
                foreach (var role in user.UserRoles)
                {
					//claims.Add(new Claim(ClaimTypes.Role, (string.Format("{0}|{1}|{2}", role.AuthorizationRoleType.RoleName, role.AuthorizationRoleTypeId, role.BusinessAccountId == null ? "" : role.BusinessAccountId.ToString()))));
					//claims.Add(new Claim(ClaimTypes.Role, (string.Format("{0}|{1}|{2}", role.AuthorizationRoleType.RoleName, role.AuthorizationRoleTypeId, ""))));
					claims.Add(new Claim(ClaimTypes.Role, role.AuthorizationRoleTypeId.ToString()));
				}
				claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			}
           
            var clIdt = new ClaimsIdentity(claims, "Federation", ClaimTypes.Name, ClaimTypes.Role);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Audience= cfg.AppJwtAudience??cfg.Resource,
                Issuer= cfg.AppJwtAuthority??cfg.Resource,
                IssuedAt= now,
                Subject = clIdt,
                Expires = now.AddMinutes(cfg.AppJwtTtlMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return new JwtAuthResult() {
				UserId = (int)user.Id,
                AccessToken = token,
                Jwt = stoken as JwtSecurityToken,
                Principal = new ClaimsPrincipal(clIdt),
                TokenExpirationUtc = tokenDescriptor.Expires.Value
            };
        }

        private User GetUser(string emailString)
        {
            if (String.IsNullOrEmpty(emailString)) return null;
      
            FlossContext ctx = FlossContext.GetContext();

            User user;

            try
            {
                user = ctx.User.Where(e => StringComparer.CurrentCultureIgnoreCase.Equals(e.Email, emailString)).Include(emp=>emp.UserRoles).ThenInclude(er=>er.AuthorizationRoleType)
                .FirstOrDefault();

            }
            catch (Exception ex)
            { throw ex; }

            return user;
        }
    }
}
