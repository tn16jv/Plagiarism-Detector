using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floss.Database.Models
{
    public partial class Assignment
    {
        public Assignment()
        {
            AssignmentSubmissions = new HashSet<AssignmentSubmission>();
            AssignmentExempts = new HashSet<AssignmentExempt>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AssId { get; set; }
        public long CourseId { get; set; }
        public string AssignmentName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime LateDueDate { get; set; }

        public Course Course { get; set; }
        public ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public ICollection<AssignmentExempt> AssignmentExempts { get; set; }
    }
}
