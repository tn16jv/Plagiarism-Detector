using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Auth {
    /// <summary>
    /// This class contains configuration for both STS and application-internal OAuth mechanisms
    /// </summary>
    public class OAuthProviderConfig {
        /// <summary>OAuth ClientID as specified on STS side. 
        /// This value is required for Code Flow only and should come from STS Administrator</summary>
        public string ClientId { get; set; }
        /// <summary>OAuth Client Secret as specified on STS side. 
        /// This value is required for Code Flow only and should come from STS Administrator</summary>
        public string ClientSecret { get; set; }
        /// <summary>Address of the TokenController. 
        /// This value is required for Code Flow only.
        /// </summary>
        public string TokenControllerAddress { get; set; }
        /// <summary>
        /// This is published root URL of this application.
        /// In terms of STS this is REALM.
        /// STS will produce token for this resource (token.Audience will be equal this.Resource).
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Address of STS Federation metadata.
        /// </summary>
        public string FederationMetadataAddress { get; set; }

        /// <summary>
        /// Lifetime in minutes for the cached Federation data.
        /// </summary>
        public int FederationMetadataTtlMinutes { get; set; }


        /// <summary>
        /// This is signing key for application-specific JWT.
        /// Generate it like this:
        /// <code>Convert.ToBase64String((new HMACSHA256()).Key)</code>
        /// </summary>
        public string AppJwtSecret { get; set; }
        /// <summary>
        /// Application specific JWT lifetime in minutes
        /// </summary>
        public int AppJwtTtlMinutes { get; set; }
        /// <summary>
        /// Application specific JWT issuer authority
        /// </summary>
        public string AppJwtAuthority { get; set; }
        /// <summary>
        /// Application specific JWT audience
        /// </summary>
        public string AppJwtAudience { get; set; }

        /// <summary>
        /// Returns <see cref="AppJwtAuthority"/> or <see cref="Resource"/> if former is not specified.
        /// </summary>
        public string ActualAppJwtAuthority {
            get {
                return string.IsNullOrEmpty(this.AppJwtAuthority) ? this.Resource : this.AppJwtAuthority;
            }
        }

        /// <summary>
        /// Returns <see cref="AppJwtAudience"/> or <see cref="ActualAppJwtAuthority"/> if former is not specified.
        /// </summary>
        public string ActualAppJwtAudiense {
            get {
                return string.IsNullOrEmpty(this.AppJwtAudience) ? this.ActualAppJwtAuthority : this.AppJwtAudience;
            }
        }

        public OAuthProviderConfig() {
            this.FederationMetadataTtlMinutes = 30;
            this.AppJwtTtlMinutes = 20;
        }

    }
}
