using Floss.Api.Auth;
using Floss.Api.Controllers;
using Floss.Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

//using System.Web.Http.Filters;

namespace Floss.Api.Filters
{
    public class AuthenticationFilter : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string usertoken = context.HttpContext.Request.Headers["Authorization"];
            OAuthUtil util = (OAuthUtil) ServiceProviderFactory.ServiceProvider.GetService(typeof(OAuthUtil));
            AppAuthRsp resp = util.GetByToken(usertoken);
            context.HttpContext.User = resp.Principal;
        }


   
    }
}
