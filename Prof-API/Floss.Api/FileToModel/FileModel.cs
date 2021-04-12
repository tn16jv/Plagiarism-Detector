using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.FileToModel
{
    //public class Plagiarism_Engine_Models.File
    //{
    //    public int Id { get; set; } // Unique ID of user from database
    //    public string CodeFiles { get; set; } // stripped code files
    //    public string FileType { get; set; } //if java, c, or c++
    //    public bool isExempt { get; set; }
    //}


	public class ComparisonResults
	{
		public string CourseName { get; set; } // dept code year dur
		public string AssignmentName { get; set; }
		public List<FileByLineModel> UserReports { get; set; }
	}

	public class FileByLineModel
	{
		public FileByLineModel()
		{
			CopyFrom = new List<CopyFromModel>();
		}

		public int UserId { get; set; } // submitter of assignment
		public string AssignmentName { get; set; } // name of submitted file (i.e. A1-5772199-jp14fg)
		public List<CopyFromModel> CopyFrom { get; set; } // who the user copied from & the lines they copied
	}

	public class CopyFromModel
	{
		public CopyFromModel()
		{
			Lines = new Dictionary<int, CopyModel>();
		}

		public Dictionary<int, CopyModel> Lines { get; set; } // 1-based index corresponding to a line number in the file
		public double CopiedPercentage { get; set; }
		public int? CopiedFromId { get; set; } // ID of user being copied from
		public string AssignName { get; set; } // Name of user/assignment info being copied from (fetched using CopiedFromId)
	}

	public class CopyModel
	{
		public bool Copied { get; set; } // indicates whether or not the line is copied
		public bool IsMethod { get; set; }
		public string Line { get; set; } // the line of code
		public int? CopiedLineId { get; set; } // the line number in the CopiedFrom file that the user copied

		public override string ToString()
		{
			return string.Format("Copied: {0}, Line: {1}", this.Copied, this.Line);
		}
	}
}