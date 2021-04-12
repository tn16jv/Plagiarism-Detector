using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Floss.Api.FileToModel
{
    class Unzip
    {
        public static void unpack (string path)
        {
            string extractPath = @".\";
            ZipFile.ExtractToDirectory(path, extractPath);
        }

        public static bool extensionMatch(ZipArchiveEntry entry, string type)
        {
            ICollection<string> extensions = ValidExtensions.getExtensions(type);
            foreach (string ext in extensions)
            {
                if (entry.FullName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool extensionMatch(string filepath, string type)
        {
            ICollection<string> extensions = ValidExtensions.getExtensions(type);
            string fileExtension = Path.GetExtension(filepath);
            if (extensions.Any(f => f.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                return true;
            return false;
        }

        /// <summary>
        /// Given a path that references a .zip file, it will unzip files with desired extensions into a new directory.
        /// </summary>
        public static void unpackFiltered(string zipPath, string targetPath, string type)
        {
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            
            if (!targetPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))    // ensures escape char at end of string
                targetPath += Path.DirectorySeparatorChar;

            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (extensionMatch(entry, type))
                        {
							//string fileName = entry.Name;

							//if (fileName.StartsWith('.')) // skip hidden files
							//	continue;

							string destinationPath = Path.Combine(targetPath, entry.Name);  // this is for relative path

                            if (destinationPath.StartsWith(targetPath, StringComparison.Ordinal))
                            {
                                if (File.Exists(destinationPath))   // Replaces pre-existing files if this is a resubmission.
                                    File.Delete(destinationPath);
                                entry.ExtractToFile(destinationPath);
                            }
                        }
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Given a path that references a directory file, it will save files with desired extensions into a new directory.
        /// </summary>
        public static void unpackFilteredWithoutUnzipping(string filespath, string tempfolderpath, string type)
        {
            try
            {
                string[] archive = Directory.GetFiles(filespath, "*", SearchOption.AllDirectories);

                foreach (string entry in archive)
                {
                    if (extensionMatch(entry, type))
                    {
						string fileName = Path.GetFileName(entry);

						if (fileName.StartsWith('.')) // skip hidden files
							continue;

						string destinationPath = Path.Combine(tempfolderpath, fileName);  // this is for relative path
                        if (destinationPath.StartsWith(filespath, StringComparison.Ordinal))
                        {
                            if (File.Exists(destinationPath))   // Replaces pre-existing files if this is a resubmission.
                                File.Delete(destinationPath);
                            File.Copy(entry, destinationPath); // moves the file to the top layer
                            File.Delete(entry);
                        }
                    }
                File.Delete(entry); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


    }
}
