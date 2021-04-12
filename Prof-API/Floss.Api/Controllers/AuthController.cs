using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floss.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Floss.Api.Helpers;
using System.Security.Claims;
//Only to test to commit 
namespace Floss.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {

        private readonly OAuthUtil _oauthUtil;

        public AuthController(OAuthUtil oauthUtil)
        {
            this._oauthUtil = oauthUtil;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public AppAuthRsp GetByToken(string accessToken)
        {
            OAuthUtil util = (OAuthUtil)ServiceProviderFactory.ServiceProvider.GetService(typeof(OAuthUtil));
            return util.GetByToken(accessToken);
        }

        /// <summary>
        /// gets authorization
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuthorizationId")]
        public int? GetAuthorizationId()
        {
            
            if (this.User is null || this.User.Claims.Count() <= 3) return null;

            List<string> authorizationArr = new List<string>();
            var suhsetList = this.User.Claims.ToList().GetRange(3, this.User.Claims.Count() - 3);
			var authIds = this.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => int.Parse(x.Value)).ToList();
			
			if (authIds.Count > 0) {
				if (authIds.Contains(2))
					return 2;
				else if (authIds.Contains(1))
					return 1;
				else if (authIds.Contains(3))
					return 3;
				else if (authIds.Contains(4))
					return 4;
			}

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetUserErrorMsg(Exception x, string fmt, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            var rx = x.GetBaseException() ?? x;
            sb.AppendFormat(fmt, args).AppendFormat("; {0} {1}", rx.GetType().Name, rx.Message);
            return sb.ToString();
        }

    }
}