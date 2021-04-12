using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Models
{
    /// <summary>
    /// model for an assignment an prof creates
    /// </summary>
	public class StudentAssignmentModel
	{

		public long AssId { get; set; }
		public long CourseId { get; set; }
		public string AssignmentName { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime LateDueDate { get; set; }
		public DateTime? SubmittedDate { get; set; }
	}
}
