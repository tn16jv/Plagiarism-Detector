using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floss.Database.Models
{
    public partial class Course
    {
        public Course()
        {
            Assignment = new HashSet<Assignment>();
            Enrollment = new HashSet<Enrollment>();
            Supervision = new HashSet<Supervision>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ProfId { get; set; }
        public string DepartmentName { get; set; }
        public string CourseCode { get; set; }
        public int Year { get; set; }
        public string Duration { get; set; }

        public ICollection<Assignment> Assignment { get; set; }
        public ICollection<Enrollment> Enrollment { get; set; }
        public ICollection<Supervision> Supervision { get; set; }
    }
}
