using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Floss.Api.Auth {

    /// <summary>
    /// result for auth
    /// </summary>
    public class JwtAuthResult {

        public ClaimsPrincipal Principal { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpirationUtc { get; set; }
        public string RefreshToken { get; set; }

        public string UserErrorMsg { get; set; }

        public JwtSecurityToken Jwt { get; set; }
		public int UserId { get; set; }

		public bool IsFaulted {
            get { return !string.IsNullOrEmpty(this.UserErrorMsg); }
        }
    }
}
