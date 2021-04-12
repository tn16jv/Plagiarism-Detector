using System;
using System.Collections.Generic;
using System.Text;

namespace Plagiarism_Engine_Models
{
	public class MessageModel
	{ 
	
		public int ProfId { get; set; } // ID of requesting prof

		public string ReportString { get; set; } // json string of Report model

		public List<File> Files { get; set; } // list of student files

		public List<string> Paths { get; set; } // path names (for PTC)

		public int AssignmentId { get; set; } // assignment ID (for PC)

		public string ReportName { get; set; } // name of test file

		public string CallBackUrl { get; set; } // url/controller/action to call to send data back
	}
}
