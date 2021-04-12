using System;
using System.Collections.Generic;

namespace Floss.Database.Models
{
    public partial class AssignmentSubmission
    {
        public AssignmentSubmission()
        {

        }

        public long AssId { get; set; }
        public long UserId { get; set; }
        public string FilePath { get; set; }
        public string StrippedFilePath { get; set; }
        public string EvalFilePath { get; set; }
        public string FileType { get; set; }

        public DateTime SubmittedDate { get; set; }

        public User User { get; set; }
        public Assignment Assignment { get; set; }
    }
}