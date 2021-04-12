using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Floss.Database.Models;
using Floss.Database.Context;

namespace Floss.Api.Authorization
{

    public class PermissionHandler : IAuthorizationHandler
    {
        private IAuthorizationRequirement requirement;

        public Task HandleAsync(AuthorizationHandlerContext context)
        {   
            // Check whether email property can be obtained
            if (context.User is null || context.User.Claims.Count() <= 1) { context.Fail(); return Task.CompletedTask; }
 
            // Get email string, and get user from DB with matching email
            string emailString = context.User.Claims.ToList()[1].Value;
            User user = this.GetEmployee(emailString);

            if (user != null)
            {
                // Get all pending requirements
                var pendingRequirements = context.PendingRequirements.ToList();

                // Loop through the requirements and resolve them based on requirement type
                foreach (var requirement in pendingRequirements)
                {  
                    if ((requirement is RoleProfessorRequirement &&
                        (IsRoleProfessor(user) || IsRoleSystemAdmin(user))) // Operations requirement can be fulfilled by Operations and System Admin roles

                        || (requirement is RoleSystemAdminRequirement && IsRoleSystemAdmin(user)) // System Admin roles can be fulfilled only by the System Admin

                        || (requirement is RoleStudentRequirement && 
                        (IsRoleStudent(user) ||  IsRoleGroupCoordinator(user) || IsRoleProfessor(user) || IsRoleSystemAdmin(user))) // Developer requirement can be fulfilled by all other roles

                        //|| (requirement is RoleGroupCoordinatorRequirement &&
                        //(IsRoleGroupCoordinator(user) || IsRoleProfessor(user) || IsRoleSystemAdmin(user))) // Group Coordinator requirement can be fulfilled by GC, Operations and System Admin roles
                        )
                        context.Succeed(requirement);
                }
            
            } else
            {  // If no matching email found in DB user will not be authorized to continue
                context.Fail();
            }
           

            return Task.CompletedTask;
        }

        private bool IsRoleProfessor(User user)
        {
            foreach (var role in user.UserRoles)
            {
                if (role.AuthorizationRoleTypeId == 1) return true;
            }
            return false;
        }

        private bool IsRoleSystemAdmin(User user)
        {
            foreach (var role in user.UserRoles)
            {
                if (role.AuthorizationRoleTypeId == 2) return true;
            }
            return false;
        }
        private bool IsRoleStudent(User user)
        {
            foreach (var role in user.UserRoles)
            {
                if (role.AuthorizationRoleTypeId == 3) return true;
            }
            return false;
        }
        private bool IsRoleGroupCoordinator(User user)
        {
            foreach (var role in user.UserRoles)
            {
                if (role.AuthorizationRoleTypeId == 4) return true;
            }
            return false;
        }

        private User GetEmployee(string emailString)
        {
            FlossContext ctx = FlossContext.GetContext();

			User user;

            try
            {
                user = ctx.User.Where(e => StringComparer.CurrentCultureIgnoreCase.Equals(e.Email, emailString)).Select(x => 
                new User()
                {
                    Id = x.Id,
                    AccountName = x.AccountName,
                    DisplayName = x.DisplayName,
                    Domain = x.Domain,
                    Email = x.Email,
                    FullName = x.FullName,
                    UserRoles = x.UserRoles
                }).FirstOrDefault();

            }
            catch (Exception ex)
            { throw ex; }

            return user;
        }
    }
}