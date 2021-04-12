using Floss.Database.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Helpers
{
    /// <summary>
    /// class for multi used functions
    /// </summary>
    public class PlagiarismHelper
    {

        FlossContext _db;
        IConfiguration _config;

        public PlagiarismHelper(FlossContext _db, IConfiguration config)
        {
            this._db = _db;
            this._config = config;
        }

        /// <summary>
        /// to save the message
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns>string</returns>
        public async Task<string> WriteToBlob(string messageBody)
        {
            Guid uniqueId = Guid.NewGuid();
            // write to local file first
            string path = string.Format("{0}\\{1}\\", Path.Combine(Directory.GetCurrentDirectory(), ".."), "TemporaryMessages");
            var localFileName = $"{uniqueId}.txt";

            var sourceFile = Path.Combine(path, localFileName); // file with path

            FileInfo file = new FileInfo(sourceFile);
            file.Directory.Create(); // creates the path if it doesn't exist

            System.IO.File.WriteAllText(sourceFile, messageBody); // write to local file

            string storageConnection = _config.GetSection("AppSettings").GetValue<string>("BlobStorageConnectionString");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("appcontainer");

            //create a container if it is not already exists

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            //get Blob reference

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
            await cloudBlockBlob.UploadFromFileAsync(sourceFile);

            System.IO.File.Delete(sourceFile); // delete local file :D

            return localFileName;

        }

        /// <summary>
        /// deletes a directory and all its files
        /// </summary>
        /// <param name="targetDir">target directory</param>
        public void DeleteDirectory(string targetDir)
        {
            if (!Directory.Exists(targetDir))   // method otherwise throws an exception if the directory doesn't exist
                return;
            System.IO.File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories); // gets all files in every directory
            string[] dirs = Directory.GetDirectories(targetDir); // gets all directories 
             
            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file); // deletes all files 
            }

            foreach (string dir in dirs) // deletes all directories
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false); // deletes target directory
        }
    }
}
