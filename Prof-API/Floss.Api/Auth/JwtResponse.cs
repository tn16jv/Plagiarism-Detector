using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Auth {
    /// <summary>
    /// 
    /// </summary>
    public class JwtResponse {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }

        public override string ToString() {
            return string.Format("access_token: \"{0}\"\r\ntoken_type: \"{1}\"\r\nexpres_in: {2}\r\nrefresh_token: \"{3}\"\r\n",
                access_token, token_type, expires_in, refresh_token);
        }

        public bool TryGetUtcExpiresAt(out DateTime expirationTime) {
            if (expires_in > 0) {
                expirationTime = DateTime.UtcNow.AddSeconds(this.expires_in);
                return true;
            }
            expirationTime = DateTime.MinValue;
            return false;
        }
    }
}
