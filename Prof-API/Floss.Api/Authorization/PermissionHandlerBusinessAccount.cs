using Floss.Database.Context;
using Floss.Database.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Floss.Api.Authorization
{
    public class PermissionHandlerBusinessAccount
    {
        public PermissionHandlerBusinessAccount() { }

        // 
        public bool CanAccessBusinessAccount(ClaimsPrincipal user, long businessAccountId)
        {
            // Check whether email property can be obtained
            if (user is null || user.Claims.Count() <= 1) { return false; }

            // Get email string, and get user from DB with matching email
            string emailString = user.Claims.ToList()[1].Value;
            User cuser = this.GetUser(emailString);

            // Loop through roles and check if correct role exists
            foreach (var role in cuser.UserRoles)
            {
				return true;
                //if (role.BusinessAccountId == businessAccountId || role.BusinessAccountId == null) return true;
            }
            return false;
        }

        private User GetUser(string emailString)
        {
            FlossContext ctx = FlossContext.GetContext();

            User user;

            try
            {
                user = ctx.User.Where(e => StringComparer.CurrentCultureIgnoreCase.Equals(e.Email, emailString)).Select(emp =>
                new User()
                {
                    Id = emp.Id,
                    AccountName = emp.AccountName,
                    DisplayName = emp.DisplayName,
                    Domain = emp.Domain,
                    Email = emp.Email,
                    FullName = emp.FullName,
                    UserRoles = emp.UserRoles
                }).FirstOrDefault();

            }
            catch (Exception ex)
            { throw ex; }

            return user;
        }
    }
}
