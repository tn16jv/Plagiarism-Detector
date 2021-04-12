using System;

namespace Plagiarism_Engine_Models
{
	public class File
	{
		public int Id { get; set; } // Unique ID of user from database
		public string CodeFiles { get; set; } // stripped code files
		public string FileType { get; set; } //if java, c, or c++
		public bool isExempt { get; set; } //if the file is exempt code from plagiarism
	}
}

