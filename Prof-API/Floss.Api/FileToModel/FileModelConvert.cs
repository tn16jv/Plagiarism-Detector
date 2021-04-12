using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floss.Api.FileToModel
{
    class FileModelConvert
    {
        /// <summary>
        /// Given a path to a directory, it will read all the files in it to strings.
        /// </summary>
        /// <returns>List of strings, each string holding all the contents of a file.</returns>
        public static string filesToStrings(string directoryPath)
        {
            List<string> files = new List<string>();
            try
            {
                string[] fileNames = Directory.GetFiles(directoryPath);
                foreach (string filePath in fileNames)
                {
                    files.Add(System.IO.File.ReadAllText(filePath));
                }
            } catch (Exception e)
            {
            }
            //return files;
			return "";
        }

        /// <summary>
        /// Takes a directory path to a student's unzipped assignment folder, their student id, and the filetype
        /// they wrote their assignment in.
        /// </summary>
        /// <returns>File class with all relevant information about an assignment submission.</returns>
        public static Plagiarism_Engine_Models.File filesToClass(string directoryPath, int id, string type, bool exemption)
        {
            Plagiarism_Engine_Models.File newFile = new Plagiarism_Engine_Models.File();
            newFile.Id = id;
            newFile.CodeFiles = System.IO.File.ReadAllText(directoryPath);
            newFile.FileType = type;
            newFile.isExempt = exemption;
            return newFile;
        }
    }
}
