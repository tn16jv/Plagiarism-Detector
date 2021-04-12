using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Models
{
    /// <summary>
    /// model for assignments that have been submitted by students
    /// </summary>
	public class AssignmentSubmissionModel
	{

		public long AssId { get; set; }
		public long UserId { get; set; }

		public string UserName { get; set; }
		public string StudentNumber { get; set; }

		public string FileType { get; set; }
		public DateTime SubmittedDate { get; set; }
	}
}
