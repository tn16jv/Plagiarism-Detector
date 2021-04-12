using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Floss.Api.Auth {
    /// <summary>
    /// Helper that allows to retrieve token valiation parameters via FederationMetadata.xml.
    /// STS metadata do not change frequently so this data just should be cached and refreshed on some lazy schedule.
    /// </summary>
    public class StsOptions {
        private readonly IOptions<OAuthProviderConfig> _oauthCfg;

        private StsConfig _stsCfg;
        private object cfgLock = new object();

        public string MetadataAddress {
            get {
                return _oauthCfg?.Value?.FederationMetadataAddress;
            }
        }

        /// <summary>
        /// When next metadata check should occure
        /// </summary>
        public DateTime NextCheck { get; protected set; }

        public StsOptions(IOptions<OAuthProviderConfig> oauthCfg) {
            _oauthCfg = oauthCfg;
            NextCheck = DateTime.MinValue;
        }

        public TokenValidationParameters GetStsTokenValidationParameters() {
            if (DateTime.Now>this.NextCheck) {
                lock(this.cfgLock) {
                    if (DateTime.Now>this.NextCheck) {
                        this._stsCfg = LoadVerifiedFrom(this.MetadataAddress);
                        this.NextCheck = DateTime.Now.AddMinutes(this._oauthCfg.Value.FederationMetadataTtlMinutes);
                    }
                }
            }
            var stsCfg = this._stsCfg;

            return new TokenValidationParameters() {
                ClockSkew = new TimeSpan(0, 10, 0), // Allow 10 minutes time skew between STS and this app. IRL this parameter better be in configuration
                IssuerSigningKeys = new List<SecurityKey>(from r in stsCfg.SigningCerts select new X509SecurityKey(r)),
                ValidIssuer = stsCfg.Issuer,
                ValidAudience = this._oauthCfg.Value.Resource
            };
        }

        protected StsConfig LoadVerifiedFrom(string metadataAddress) {
            try {
                var xdm = new XmlDocument();
                xdm.Load(metadataAddress);
                // Verify XML signature in the federation metadata along with the signing certificate itself
                // You can provide alternative verification by specifying signing certificate validation delegate in VerifyXmlSignature
                //if (!Crypto.VerifyXmlSignature(xdm, false)) {
                //    throw new ApplicationException("Federation metadata signature is not valid, will not be able to validate STS JWT token.");
                //}
                var nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                nsm.AddNamespace("meta", "urn:oasis:names:tc:SAML:2.0:metadata");
                nsm.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
                var xcerts = xdm.SelectNodes("/meta:EntityDescriptor/meta:RoleDescriptor[@xsi:type='fed:SecurityTokenServiceType']/meta:KeyDescriptor[@use='signing']/sig:KeyInfo/sig:X509Data/sig:X509Certificate", nsm);
                List<X509Certificate2> stsSigningTokens = new List<X509Certificate2>();
                X509Certificate2 cert;
                foreach (XmlNode xc in xcerts) {
                    cert = new X509Certificate2(Convert.FromBase64String(xc.InnerText));
                    stsSigningTokens.Add(cert);
                }
                return new StsConfig() {
                    Issuer = xdm.DocumentElement.GetAttribute("entityID"),
                    SigningCerts = stsSigningTokens,
                    CheckedAt = DateTime.Now
                };
            }
            catch (Exception x) {
                x.Data["metadataAddress"] = metadataAddress ?? "-?!-";
                throw;
            }
        }

        protected class StsConfig {
            public string Issuer { get; set; }
            public List<X509Certificate2> SigningCerts { get; set; }
            public DateTime CheckedAt { get; set; }
        }

    }
}
