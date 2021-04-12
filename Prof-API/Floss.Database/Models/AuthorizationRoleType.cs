using System;
using System.Collections.Generic;

namespace Floss.Database.Models
{
    public partial class AuthorizationRoleType
    {
        public long Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<UserRole> EmployeeRoles { get; set; }
    }
}
