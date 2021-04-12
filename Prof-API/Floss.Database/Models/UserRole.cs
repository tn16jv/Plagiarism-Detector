using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floss.Database.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
        public long UserId { get; set; }
        public long AuthorizationRoleTypeId { get; set; }

        public User User { get; set; }
		public AuthorizationRoleType AuthorizationRoleType { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
