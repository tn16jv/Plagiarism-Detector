using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Models
{
    /// <summary>
    /// model of student for who will be enrolled into a class
    /// </summary>
	public class EnrollStudentModel
	{
		public int CourseId { get; set; }
		public string StudentEmails { get; set; }
	}
}
