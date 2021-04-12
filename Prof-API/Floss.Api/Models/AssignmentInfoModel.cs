using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Models
{
    /// <summary>
    /// holds information about an assignment
    /// </summary>
	public class AssignmentInfoModel
	{

		public string CourseName { get; set; }
		public string YearDuration { get; set; }
		public string AssignmentName { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime LateDueDate { get; set; }


	}
}
