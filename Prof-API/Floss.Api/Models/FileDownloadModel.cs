using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Models
{
    /// <summary>
    /// model for assignment download
    /// </summary>
	public class FileDownloadModel
	{

		public FileStream FileStream { get; set; }

		public string FileName { get; set; }

	}
}
