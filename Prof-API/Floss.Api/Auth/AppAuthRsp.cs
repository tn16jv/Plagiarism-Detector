using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Floss.Api.Auth {

    /// <summary>
    /// Response for application-specific authorization request.
    /// Contains application-specific JWT or error.
    /// </summary>
    public class AppAuthRsp {
		public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AppJwt { get; set; }
        internal ClaimsPrincipal Principal{ get;set;}
        public string Error { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}
