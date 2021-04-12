using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Models
{
    /// <summary>
    /// model for a user of the website
    /// </summary>
	public class UserModel
	{
		public long Id { get; set; }
		public string Domain { get; set; }
		public string AccountName { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }
		public string StudentNumber { get; set; }
		public string RoleName { get; set; }
		public long RoleId { get; set; }
	}
}
