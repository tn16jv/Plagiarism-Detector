using System;
using System.Collections.Generic;

namespace Floss.Database.Models
{
    public partial class Enrollment
    {
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public DateTime? Until { get; set; }

        public Course Course { get; set; }
        public User User { get; set; }
    }
}
