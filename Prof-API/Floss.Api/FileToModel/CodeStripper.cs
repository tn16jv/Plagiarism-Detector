using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Floss.Api.FileToModel
{
    public class CodeStripper
    {
        /// <summary>
        /// function to call the strip function based on the language
        /// </summary>
        /// <param name="path"></param>
        /// <param name="progType"></param>
        /// <returns>string</returns>
        public static string StripAssignment(string path, string progType)
        {
            switch (progType)
            {
                case "java":
                    return (javaStrip(path));
                case "cpp":
                    return (cppStrip(path));
                case "c":
                    return (cStrip(path));
            }
            return "";
        }

        /// <summary>
        /// the c++ strip
        /// </summary>
        /// <param name="path"></param>
        /// <returns>result of the strip</returns>
        public static string cppStrip(string path)
        {
            string assignment = "";
            foreach (string filepath in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)) // iterates through the files
            { 
                if (Unzip.extensionMatch(filepath, "cpp"))
                {
                    int i = 0;
                    string[] lines = new string[System.IO.File.ReadLines(filepath).Count() + 1];
                    foreach (string line in System.IO.File.ReadLines(filepath))
                    {
                        if (line.ToLower().StartsWith("#include") || line.ToLower().StartsWith("using"))
                        {
                            lines[i] = line.Remove(0);
                        }// for packages and libraries used
						else
						{
							lines[i] = line + System.Environment.NewLine;
						}
						i++;
                    }
                    string code = String.Join("", lines); // combines all the lines together
                    var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";

                    assignment += Regex.Replace(code, re, "$1"); // regex for removing all comments 
                }
            }
            return assignment;
        }

        /// <summary>
        /// the java strip
        /// </summary>
        /// <param name="path"></param>
        /// <returns>result of the strip</returns>
        public static string javaStrip(string path)
        {
            string assignment = "";
            foreach (string filepath in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)) // iterates through the files
            {
                if (Unzip.extensionMatch(filepath, "java"))
                {
                    int i = 0;
                    string[] lines = new string[System.IO.File.ReadLines(filepath).Count() + 1];
                    foreach (string line in System.IO.File.ReadLines(filepath))
                    {
                        if (line.ToLower().StartsWith("import") || line.ToLower().StartsWith("package"))
                        {
							lines[i] = line.Remove(0);
                        } // for packages and libraries used
                        else
                        {
                            lines[i] = line + System.Environment.NewLine;
						}
                        i++;
                    }
                    string code = String.Join("", lines); // combines all the lines together
                    var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";

                    assignment += Regex.Replace(code, re, "$1"); // regex for removing all comments
                    
                }
            }
            return assignment;
        }

        /// <summary>
        /// the c strip
        /// </summary>
        /// <param name="path"></param>
        /// <returns>result of the strip</returns>
        public static string cStrip(string path)
        {
            string assignment = ""; 
            foreach (string filepath in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)) // iterates through the files
            {
                if (Unzip.extensionMatch(filepath, "c"))
                {
                    int i = 0;
                    string[] lines = new string[System.IO.File.ReadLines(filepath).Count() + 1];
                    foreach (string line in System.IO.File.ReadLines(filepath))
                    {
                        if (line.ToLower().StartsWith("#include") || line.ToLower().StartsWith("using"))
                        {
                            lines[i] = line.Remove(0);
                        } // for packages and libraries used
                        else
                        {
                            lines[i] = line + System.Environment.NewLine;
                        }
                        i++;
                    }
                    string code = String.Join("", lines); // combines all the lines together
                    var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/"; 

                    assignment += Regex.Replace(code, re, "$1"); // regex for removing all comments

                }
            }
            return assignment;
        }
    }
}
