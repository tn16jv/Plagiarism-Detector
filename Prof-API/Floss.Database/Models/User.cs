using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floss.Database.Models
{
    public partial class User
    {
        public User()
        {
            Enrollment = new HashSet<Enrollment>();
            Supervision = new HashSet<Supervision>();
            AssignmentSubmissions = new HashSet<AssignmentSubmission>();
        }

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
        public string Domain { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        [MaxLength(7), MinLength(7)]
        public string StudentNumber { get; set; }
        //public bool IsActive { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Enrollment> Enrollment { get; set; }
        public ICollection<Supervision> Supervision { get; set; }
        public ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; }
    }
}
