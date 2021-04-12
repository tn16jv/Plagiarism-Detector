using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Floss.Database.Models
{
    public partial class AssignmentExempt
    {
        public AssignmentExempt()
        {

        }

        public long AssId { get; set; }
        public string FilePath { get; set; }
        public string StrippedFilePath { get; set; }
        public string FileType { get; set; }

        public Assignment Assignment { get; set; }
    }
}