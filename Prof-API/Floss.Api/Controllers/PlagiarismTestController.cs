using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Floss.Api.FileToModel;
using Floss.Api.PlagEngineModels;
using Floss.Database.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Azure.ServiceBus;
using Plagiarism_Engine_Models;
using Floss.Api.Helpers;
using System.Text.RegularExpressions;

namespace Floss.Api.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(Policy = "Operator")]
    [ApiController]
    public class PlagiarismTestController : Controller
    {

        FlossContext _db;
        IConfiguration _config;
		PlagiarismHelper plagiarismHelper;


		public PlagiarismTestController(FlossContext _db, IConfiguration config)
        {
            this._db = _db;
            this._config = config;
			plagiarismHelper = new PlagiarismHelper(_db, config);
		}

        /// <summary>
        /// uploades of the test and making of the comparison call
        /// </summary>
        /// <param name="fileType">language used</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadTest")]
        public ActionResult<string> UploadTest(string fileType)
        {
            IFormFile postfile;
            string returnMessage;
            var profId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            try
            {
                postfile = Request.Form.Files.First();
                if (string.Equals(Path.GetExtension(postfile.FileName), ".zip", StringComparison.OrdinalIgnoreCase))
                { // makes sure it's a zip file

                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
                    pathToSave = Path.Combine(pathToSave, "FlossRepo"); 
                    string path = string.Format("{0}\\{1}\\{2}\\",
                                             pathToSave, profId, "Test"); // path to save upload

					plagiarismHelper.DeleteDirectory(path);      // Clears directory of previous assignment submissions

                    var filePath = Path.Combine(path, postfile.FileName);
                    // path needs to be changed later
                    FileInfo file = new FileInfo(filePath);
                    file.Directory.Create(); // creates the path if it doesn't exist

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        postfile.CopyToAsync(fileStream).Wait();
                    } // copies from local upload to filepath

                    string fn = filePath.Remove(filePath.Length - 4); // removes .zip from name
					string uploadedFileName = System.IO.Path.GetFileName(fn); // gets name of submitted zip file
                    ZipFile.ExtractToDirectory(filePath, fn); //extract folder
                    System.IO.File.Delete(filePath); // delete zip
                    List<string> filetypes = new List<string>();
                    List<string> paths = new List<string>();
                    string[] fr = Directory.GetDirectories(fn, "*", SearchOption.TopDirectoryOnly); // get list of top directories 

					string[] directoryList;
					if (fr.Count() == 1) // proper submission format
					{
						directoryList = Directory.GetDirectories(fr[0]);
					}
					else // naive submission format
					{
						directoryList = fr;
					}

					int i = 0;
                    foreach (string f in directoryList) // iterates through the assignments
                    {
						string langtype = getType(f); // checks the language type

						if (!langtype.Equals(fileType)) // only work with fileType files
							continue;

						string strippedpath = Path.Combine(f, "filtered.txt"); // for the stripped code
                        string temppath = Path.Combine(f, "tmp");
                        if (!Directory.Exists(temppath)) // create a temp path 
                            Directory.CreateDirectory(temppath);

                        
                        filetypes.Add(langtype); // add type to list
                        paths.Add(strippedpath); // add filter path to list
                        Unzip.unpackFilteredWithoutUnzipping(f, temppath, filetypes.ElementAt(i));
                        string assignment = CodeStripper.StripAssignment(temppath, filetypes.ElementAt(i)); // gets rid of useless stuff in the code
                        System.IO.File.WriteAllText(strippedpath, assignment); // saves all the useful text
						plagiarismHelper.DeleteDirectory(temppath); // delete temp
                        i++;
                    }
                    string result = GetCompare(paths, filetypes, profId, uploadedFileName); // make comparison call
					Guid guid; //unique id
					bool valid = Guid.TryParse(result, out guid);

					if (valid)
						return Ok(new { guid = guid }); // For some reason this has to be an object since angular post calls must have object return types??? Not sure... Screw it

					return BadRequest(result);
                }
                else
                    return BadRequest("Failed test: not a zip file.");
            }
            catch (Exception ex) // in case something is wrong
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// gets the type of assignment it is
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>result</returns>
        private string getType(string path)
        {
            foreach (string p in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(p).Equals(".java"))
                {
                    return "java";
                } else if (Path.GetExtension(p).Equals(".c"))
                {
                    return "c";
                } else if (Path.GetExtension(p).Equals(".cpp"))
                {
                    return "cpp";
                }
            } // finds the first file of that type and returns the result
            return "";
        }

        /// <summary>
        /// calls the comparison api
        /// </summary>
        /// <param name="paths">paths</param>
        /// <param name="filetype">language</param>
        /// <param name="profId">prof id</param>
        /// <param name="uploadedFileName">filename</param>
        /// <returns>string</returns>
        private string GetCompare(List<string> paths, List<string> filetype, int profId, string uploadedFileName)
        {
            if (paths.Count == 0)
                return ("No files found that matches that filetype in the zip. Resubmit and choose the proper file type or include proper files.");
            string email = _db.User.Where(x => x.Id == profId).First().Email; // gets email of the user
            List<Plagiarism_Engine_Models.File> studentFiles = new List<Plagiarism_Engine_Models.File>();
            try
            {
                int i = 0;
                foreach (string asub in paths)
                {
                    studentFiles.Add(FileModelConvert.filesToClass(asub, i, filetype[i], false));
                    i++;
                } // adds to the Files used to compare list
            }
            catch (Exception ex)
            {
                return "Failure in adding files to comparison.";
            }
            HttpClient client = new HttpClient();
			client.Timeout = new TimeSpan(0, 10, 0); // 10 minutes

			string url = _config.GetSection("AppSettings").GetValue<string>("Plagiarism_API"); // gets all the urls and keys
			string callBackUrl = _config.GetSection("AppSettings").GetValue<string>("Floss_Prof_API");
			string apikey = _config.GetSection("AppSettings").GetValue<string>("API_KEY");
			string environment = _config.GetSection("AppSettings").GetValue<string>("Environment"); 

			try
            {
				if (environment == "DEV") // IF RUNNING ON SERVER, call Function app
				{

					const string ServiceBusConnectionString = "Endpoint=sb://floss-engine-dev.servicebus.windows.net/;SharedAccessKeyName=engine;SharedAccessKey=zNs1q1NQ6+7nDGK85Ifq5KFkIpVitQYCgiBk35N/Pf4=;";
					const string QueueName = "incomingcomparisons";
					IQueueClient queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

					MessageModel model = new MessageModel(); // whats going to be send to the call
					model.Files = studentFiles;
					model.Paths = paths;
					model.ProfId = profId;
					model.ReportName = uploadedFileName;
					model.CallBackUrl = $"{callBackUrl}/PlagiarismTest/PushCompareResults";


					string messageBody = Newtonsoft.Json.JsonConvert.SerializeObject(model); 
                    
					// write messageBody to blob storage with unique ID
					string messageFileName = plagiarismHelper.WriteToBlob(messageBody).Result;
					// pass unique ID to queue 

					Message message = new Message(Encoding.UTF8.GetBytes(messageFileName));
					queueClient.SendAsync(message).Wait(); // makes call

					return "Comparison request queued! You will recieve an email with the results shortly.";

				} else // any other env
				{
					string requestUrl = String.Format("{0}/{1}?{2}", url, "Compare/Compare", "apiKey=" + apikey);

					var response = (client.PostAsync(requestUrl,
						new StringContent(JsonConvert.SerializeObject(studentFiles), Encoding.UTF8, "application/json"))).Result; //makes call

					if (!response.IsSuccessStatusCode) // if its not successful
					{
						return response.Content.ReadAsStringAsync().Result;
					}
					var responseString = response.Content.ReadAsStringAsync().Result; // gets the response result


					List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString); // converts it to reports
                    
					ComparisonResults fByLine = ReceiveDataFromTestCompareCall(reports, studentFiles, paths, uploadedFileName); // seperates it into lines

					string result = JsonConvert.SerializeObject(fByLine).ToString();

					var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
					string path = string.Format("{0}\\{1}\\",
						pathToSave, "ComparisonResults");

					FileInfo file = new FileInfo(path);
					file.Directory.Create(); // creates the path if it doesn't exist

					Guid g = Guid.NewGuid();
					string profGuid = string.Format("{0}_{1}", profId, g.ToString()); // makes unique id
					using (StreamWriter fileStream = new StreamWriter(path + profGuid))
					{
						fileStream.Write(result);
					} // writes result to file
					stmpsendemail(profId, g.ToString(), uploadedFileName); // makes email saying results are finished
					return g.ToString();
				}

			}
			catch (Exception ex) // in case an error occurs
			{
				return ex.Message;
			}
		}

        /// <summary>
        /// saves the results from the comparison results
        /// </summary>
        /// <param name="resultModel">result model</param>
        /// <returns>string</returns>
		[AllowAnonymous] // turn off authentication :D
		[HttpPost]
		[Route("PushCompareResults")]
		public ActionResult<string> PushCompareResults(MessageModel resultModel)
		{
			List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(resultModel.ReportString); // gets reports 

			ComparisonResults fByLine = ReceiveDataFromTestCompareCall(reports, resultModel.Files, resultModel.Paths, resultModel.ReportName);
            // seperates reports by lines
			string result = JsonConvert.SerializeObject(fByLine).ToString();

			var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
			string path = string.Format("{0}\\{1}\\",
				pathToSave, "ComparisonResults"); // where the results will be saved

			FileInfo file = new FileInfo(path);
			file.Directory.Create(); // creates the path if it doesn't exist

			Guid g = Guid.NewGuid();
			string profGuid = string.Format("{0}_{1}", resultModel.ProfId, g.ToString()); // unique id
			using (StreamWriter fileStream = new StreamWriter(path + profGuid))
			{
				fileStream.Write(result);
			} // saves results to file
			stmpsendemail(resultModel.ProfId, g.ToString(), resultModel.ReportName); // sends emails that results are in
			return g.ToString(); //returns id
		}
		
        /// <summary>
        /// sends the prof an email that the comparison test results are complete
        /// </summary>
        /// <param name="profId">prof id</param>
        /// <param name="pguid">id</param>
        /// <param name="reportName">report name</param>
        /// <returns>string</returns>
        private string stmpsendemail(int profId, string pguid, string reportName)
        {
			string email = _db.User.Where(x => x.Id == profId).First().Email; // gets email of the user

			string uiUrl = _config.GetSection("AppSettings").GetValue<string>("Floss_UI"); // ui url

            string link = string.Format("{0}/compare/{1}", uiUrl, pguid); // the results of comparison will be shown

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("flossresults@gmail.com");
            message.To.Add(new MailAddress(email));
            message.Subject = $"FLOSS: Comparison Results for {reportName}.";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = string.Format(
                @"Your comparison results are ready at the link below: <br>
						<a href=""{0}"">results</a>", link);

            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("flossresults@gmail.com", "Bockus1234");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                smtp.Send(message); // sends email
            }
            catch (Exception ex) // if something goes wrong
            {
                return ex.Message;
            }
            
            return "ok";
        }
        
        /// <summary>
        /// This is a helper method to combine results of engine API with the files
        /// </summary>
        /// <param name="reports"></param>
        /// <param name="studentFiles"></param>
        /// <returns></returns>
        private ComparisonResults ReceiveDataFromTestCompareCall(List<Report> reports, List<Plagiarism_Engine_Models.File> studentFiles, List<string> assignNames, string reportName)
        {
            ComparisonResults results = new ComparisonResults(); // results
			results.CourseName = "Stage3";
			results.AssignmentName = reportName;
            List<FileByLineModel> userCopy = new List<FileByLineModel>();
            string testcase = "Plagiarism Test";
            // for each copying user in reports
            foreach (var report in reports) // for every report 
            {
                var copyInstance = new FileByLineModel();
                copyInstance.UserId = report.userReportId; // for the user the report is about
                
				string folderName = getFolderName(assignNames[copyInstance.UserId]); // path of of assign

				copyInstance.AssignmentName = folderName;
                
                string[] assignmentLines;
                var studentFile = studentFiles.FirstOrDefault(x => x.Id == copyInstance.UserId); // for every student thats copied from

                if (studentFile != null)
                    assignmentLines = studentFile.CodeFiles.Split(new string[] { System.Environment.NewLine, "\n" }, StringSplitOptions.None); // get lines in code file
                else
                    continue;

                // for each user that the current user has copied from
                foreach (var copyFrom in report.froms) 
                {
                    var copyFromUser = new CopyFromModel();
                    copyFromUser.CopiedFromId = copyFrom.userFromID; // store ID of user they copied from
					copyFromUser.AssignName = getFolderName(assignNames[copyFrom.userFromID]);

                    // store all lines in CopyFromModel dictionary
                    for (int i = 0; i < assignmentLines.Count(); i++)
                    {
                        var copiedLineModel = new CopyModel();
                        copiedLineModel.Line = assignmentLines[i]; // saves line

                        copyFromUser.Lines.Add(i + 1, copiedLineModel); // adds it to list
                    }

                    // for each copied section
                    foreach (var plag in copyFrom.copiedFrom)
                    {

                        for (int i = plag.reportStartLine; i <= plag.reportEndLine; i++) // for each line
                        {
                            if (copyFromUser.Lines.ContainsKey(i)) 
                            {
                                copyFromUser.Lines[i].Copied = plag.isMethod ? false : true; // If its copied
								copyFromUser.Lines[i].IsMethod = plag.isMethod;  // if its a method
								if (i == plag.reportStartLine) // if on first line
                                {
                                    copyFromUser.Lines[i].CopiedLineId = plag.fromStart; // if its copied and from where
                                }
                            }
                        }
                    }
					var copiedLineCount = (double)copyFromUser.Lines.Count(x => x.Value.Copied); // count of copied lines
					var totalLineCount = (double)copyFromUser.Lines.Count(x => !ignoredLines.IsMatch(x.Value.Line)); // total count

					copyFromUser.CopiedPercentage = (int)(Math.Round((copiedLineCount / totalLineCount), 2)*100); // percent
                    copyInstance.CopyFrom.Add(copyFromUser); // user copied from
                }
				copyInstance.CopyFrom = copyInstance.CopyFrom.OrderByDescending(x => x.CopiedPercentage).ToList(); // creates list of each copied

				userCopy.Add(copyInstance); // add to list of results
            }
			// sort list by order of total copied-ness
			userCopy = userCopy?.OrderByDescending(x => x.CopyFrom?.Max(c => c?.CopiedPercentage))?.ToList();

            results.UserReports = userCopy;

            return results; // return results
        }

		private Regex ignoredLines = new Regex(@"^\s*$|}\n|};\n|}\s*$"); //regex for ignoring these lines
        /// <summary>
        /// gets the assignments folder name
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>assignments folder name</returns>
        private string getFolderName(string path)
        {
            return new DirectoryInfo(path).Parent.Name;
        } //get the parents folder name, for the assignment folder

    }
}