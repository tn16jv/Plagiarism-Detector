using System;
using System.Collections.Generic;

namespace Floss.Database.Models
{
    public partial class Supervision
    {
        public long UserId { get; set; }
        public long CourseId { get; set; }

        public Course Course { get; set; }
        public User User { get; set; }
    }
}
