using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Floss.Api.Models;
using Floss.Api.FileToModel;
using Floss.Database.Context;
using Floss.Database.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Floss.Api.PlagEngineModels;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Plagiarism_Engine_Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Floss.Api.Helpers;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Floss.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class PlagiarismController : Controller
    {
        FlossContext _db;
		IConfiguration _config;
		PlagiarismHelper plagiarismHelper;
		private Regex ignoredLines = new Regex(@"^\s*$|}\n|};\n|}\s*$");

		public PlagiarismController(FlossContext _db, IConfiguration config)
		{
			this._db = _db;
			this._config = config;
			plagiarismHelper = new PlagiarismHelper(_db, config);
		}

        /// <summary>
        /// This is a helper method to combine results of engine API with the files
        /// </summary>
        /// <param name="reports">list of reports</param>
        /// <param name="studentFiles">list of comparison files</param>
        /// <param name="assignmentId">assignment id</param>
        /// <returns>ComparisonResults</returns>
        private ComparisonResults ReceiveDataFromCompareCall(List<Report> reports, List<Plagiarism_Engine_Models.File> studentFiles, int assignmentId)
        {
			ComparisonResults results = new ComparisonResults();
			List<FileByLineModel> userCopy = new List<FileByLineModel>();

			var assignmentInfo = _db.Assignment.Include(x => x.Course).FirstOrDefault(x => x.AssId == assignmentId);// assignment info from db
            var courseInfo = assignmentInfo.Course; 

            results.CourseName = string.Format("{0}{1}/{2}/{3}", courseInfo.DepartmentName, courseInfo.CourseCode, courseInfo.Year, courseInfo.Duration);//sets up the name for the course
            results.AssignmentName = assignmentInfo.AssignmentName; // assign name

            // for each copying user in reports
            foreach (var report in reports)
            {
                var copyInstance = new FileByLineModel();
                copyInstance.UserId = report.userReportId; // copied user is

                var user = _db.User.FirstOrDefault(x => x.Id == copyInstance.UserId); // gets the user from db

                copyInstance.AssignmentName = $"{user.AccountName}";
				copyInstance.AssignmentName += user.StudentNumber != null && user.StudentNumber != "" ? $"-{user.StudentNumber}" : ""; // include student number in assignment name if not null

                string[] assignmentLines;
                var studentFile = studentFiles.FirstOrDefault(x => x.Id == copyInstance.UserId);

                if (studentFile != null) // copy over lines
                    assignmentLines = studentFile.CodeFiles.Split(new string[] { System.Environment.NewLine, "\n" }, StringSplitOptions.None); // get lines in code file
                else
                    continue;

                // for each user that the current user has copied from
                foreach (var copyFrom in report.froms)
                {
                    var copyFromUser = new CopyFromModel();
                    copyFromUser.CopiedFromId = copyFrom.userFromID; // store ID of user they copied from

					var copyFromUserInfo = _db.User.FirstOrDefault(x => x.Id == copyFrom.userFromID); // get from db
                    copyFromUser.AssignName = $"{copyFromUserInfo.AccountName}";
					copyFromUser.AssignName += copyFromUserInfo.StudentNumber != null && copyFromUserInfo.StudentNumber != "" ? $"-{copyFromUserInfo.StudentNumber}" : ""; // include student number in assignment name if not null
					

                    // store all lines in CopyFromModel dictionary
                    for (int i = 0; i < assignmentLines.Count(); i++)
                    {
                        var copiedLineModel = new CopyModel();
                        copiedLineModel.Line = assignmentLines[i];

                        copyFromUser.Lines.Add(i + 1, copiedLineModel); // zero based,adds all the lines to a list
                    }

					// for each copied section
					foreach (var plag in copyFrom.copiedFrom)
					{

						for (int i = plag.reportStartLine; i <= plag.reportEndLine; i++)
						{
							if (copyFromUser.Lines.ContainsKey(i))
							{
								copyFromUser.Lines[i].Copied = plag.isMethod ? false : true; // if its copied
								copyFromUser.Lines[i].IsMethod = plag.isMethod; // if its a method copied
                                if (i == plag.reportStartLine) // if on first line
								{
									copyFromUser.Lines[i].CopiedLineId = plag.fromStart;
								}
							}
						}
					}
					var copiedLineCount = (double)copyFromUser.Lines.Count(x => x.Value.Copied); // copied lines num
                    var totalLineCount = (double)copyFromUser.Lines.Count(x => !ignoredLines.IsMatch(x.Value.Line)); // total lines

                    copyFromUser.CopiedPercentage = (int)(Math.Round((copiedLineCount / totalLineCount), 2) * 100); // percentage 

                    copyInstance.CopyFrom.Add(copyFromUser);
				}
				copyInstance.CopyFrom = copyInstance.CopyFrom.OrderByDescending(x => x.CopiedPercentage).ToList();


				userCopy.Add(copyInstance); // add the instance to object
            }

			results.UserReports = userCopy;

            return results; // returns all the results from who they were copied from
        }


        /// <summary>
        /// makes a comparison call for an assignment
        /// </summary>
        /// <param name="AssignId">assignment id</param>
        /// <returns>guid</returns>
        [HttpGet]
        [Route("RepoCompare")]
        [Authorize(Policy = "RoleProfessor")]
		public ActionResult<string> CompareRepo(int AssignId)
		{

            var userId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); //gets the user id from claim
            List<AssignmentSubmission> assigns = _db.AssignmentSubmission.Where(x => x.AssId.Equals(AssignId)).ToList(); //gets all the submitted assignments for the assignid
            string result = GetCompare(assigns, AssignId, userId); // calls the compare function 

            Guid guid; // creates a gui so its unqiue 
            bool valid = Guid.TryParse(result, out guid); 

			if (valid)
				return Ok(new { guid = guid }); // For some reason this has to be an object since angular post calls must have object return types??? Not sure... Screw it

			return BadRequest(result);

		}

        /// <summary>
        /// comparison for multiple assignments from selected students
        /// </summary>
        /// <param name="AssignId">assignment id</param>
        /// <param name="userIds"></param>
        /// <returns>string</returns>
        [HttpGet]
        [Route("MultiAssignCompare")]
        [Authorize(Policy = "RoleProfessor")]
        public ActionResult<string> CompareMulti(int AssignId, List<int> userIds)
        {
            var userId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); //gets the user id from claim

            List<AssignmentSubmission> assigns = new List<AssignmentSubmission>(); 
            List<AssignmentSubmission> Allassigns = _db.AssignmentSubmission.Where(x => x.AssId.Equals(AssignId)).ToList(); // makes a list of all the assigneds 
            foreach (int id in userIds) //gets all the submitted assignments for selected users
            {
                if (Allassigns.Any(x => ((int)x.UserId).Equals(id)))
                {
                    assigns.Add(Allassigns.Where(x => ((int)x.UserId).Equals(id)).First());
                } // makes sure it exists before adding
            } // gets a list of selected assignments
            return GetCompare(assigns, AssignId, userId);
        }

        /// <summary>
        /// makes the comparison call for the plagiarism api 
        /// </summary>
        /// <param name="assSub">list of assignment submissions</param>
        /// <param name="assId">assignment id</param>
        /// <param name="profId">prof who submitted the request</param>
        /// <returns></returns>
        private string GetCompare(List<AssignmentSubmission> assSub, int assId, int profId)
        {

            List<Plagiarism_Engine_Models.File> studentFiles = new List<Plagiarism_Engine_Models.File>(); // gets all the student files that are going to be compared
            try
            {
                foreach (AssignmentSubmission asub in assSub) // for every assign compared 
                {    
                    studentFiles.Add(FileModelConvert.filesToClass(asub.StrippedFilePath, (int)asub.UserId, asub.FileType, false)); // add it to list
                } // creates a list of files 

				var exemptCode = _db.AssignmentExempt.FirstOrDefault(x => x.AssId == assId); // check if its exempt
                if (exemptCode != null) // if any exempt code for assignment, add it to list
					studentFiles.Add(FileModelConvert.filesToClass(exemptCode.StrippedFilePath, -1, exemptCode.FileType, true));

			}
			catch (Exception ex) // in case something fails
            {
                return "Failure in adding files to comparison.";
            }
            HttpClient client = new HttpClient();
            // gets all the keys and urls
            string url = _config.GetSection("AppSettings").GetValue<string>("Plagiarism_API");
			string callBackUrl = _config.GetSection("AppSettings").GetValue<string>("Floss_Prof_API");
			string apikey = _config.GetSection("AppSettings").GetValue<string>("API_KEY");
			string environment = _config.GetSection("AppSettings").GetValue<string>("Environment"); 
			try
            {
				if (environment == "DEV")  // for dev environment 
                {
					const string ServiceBusConnectionString = "Endpoint=sb://floss-engine-dev.servicebus.windows.net/;SharedAccessKeyName=engine;SharedAccessKey=zNs1q1NQ6+7nDGK85Ifq5KFkIpVitQYCgiBk35N/Pf4=;";
					const string QueueName = "incomingcomparisons";
					IQueueClient queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

					MessageModel model = new MessageModel();  //sets up whats going to be passed to the api call
                    model.Files = studentFiles;
					model.AssignmentId = assId;
					model.ProfId = profId;
					model.CallBackUrl = $"{callBackUrl}/Plagiarism/PushCompareResults";

					string messageBody = Newtonsoft.Json.JsonConvert.SerializeObject(model);
					// write messageBody to blob storage with unique ID
					string messageFileName = plagiarismHelper.WriteToBlob(messageBody).Result;
					// pass unique ID to queue 

					Message message = new Message(Encoding.UTF8.GetBytes(messageFileName));
					queueClient.SendAsync(message).Wait();

					return "Comparison request queued! You will recieve an email with the results shortly.";
				}
                else // for other environments
                {
					string requestUrl = String.Format("{0}/{1}?{2}", url, "Compare/Compare", "apiKey=" + apikey); // url

                    var response = (client.PostAsync(requestUrl,
						new StringContent(JsonConvert.SerializeObject(studentFiles), Encoding.UTF8, "application/json"))).Result; //makes the call
					if (!response.IsSuccessStatusCode) // error occured 
                    {
						return "An error occurred when calling the comparison engine.";
					}
					var responseString = response.Content.ReadAsStringAsync().Result; // gets result


                    List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString); // converts the results to reports 

                    ComparisonResults fByLine = ReceiveDataFromCompareCall(reports, studentFiles, assId);

					string result = JsonConvert.SerializeObject(fByLine).ToString();  // converts all the reports to specific results

                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
					string path = string.Format("{0}\\{1}\\",
						pathToSave, "ComparisonResults");

					FileInfo file = new FileInfo(path);
					file.Directory.Create(); // creates the path if it doesn't exist

					Guid g = Guid.NewGuid();
					string profGuid = string.Format("{0}_{1}", profId, g); // ID to use for file storage

					using (StreamWriter fileStream = new StreamWriter(path + profGuid))
					{
						fileStream.Write(result);
					} // saves the results in a file
					stmpsendemail(profId, g.ToString(), fByLine.CourseName, assId); // makes email call 

                    return g.ToString(); // returns the file id
                }
            }
            catch (Exception ex)  // in case error occurs
            {
                return ex.Message;
            } 
        }

        /// <summary>
        /// get the comparison results
        /// </summary>
        /// <param name="guid">uid</param>
        /// <returns>comparisonresults</returns>
        [HttpGet]
        [Route("GetCompareResults")]
        [Authorize(Policy = "RoleProfessor")]
        public ActionResult<ComparisonResults> GetComparedResults(string guid)
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
            string path = string.Format("{0}\\{1}",
                pathToSave, "ComparisonResults");
            try
            {
                 
				if (Directory.EnumerateFiles(path).Any(f => f.Contains(guid))) // check if any of the files match the id
                {
					DirectoryInfo dir = new DirectoryInfo(path);
					var files = dir.GetFiles("*" + guid); // guids are unique, should only ever be one file

					var file = files.FirstOrDefault();

					if (file == null) // if file doesnt exist
                        return BadRequest("Results not found");

					string results = System.IO.File.ReadAllText(file.FullName);
					// convert json to list filebyline models and return that
					if (System.IO.File.GetCreationTime(file.FullName).AddHours(24.0) >= DateTime.Now)  // makes sure it doesnt expire 
                    {
						ComparisonResults fByModel = JsonConvert.DeserializeObject<ComparisonResults>(results);
						return fByModel;
					}
                    else // its expired so delete it
                    {
						System.IO.File.Delete(file.FullName);
						return BadRequest("Request has been expired.");
					}
				}
			} catch (Exception ex) // in case an error occured 
            {
                return BadRequest("An error has occured. Results cannot be displayed.");
            }
            return BadRequest("Request does not exist anymore.");
        }
        
        /// <summary>
        /// sends an email for the comparison results being finished
        /// </summary>
        /// <param name="profId">prof id</param>
        /// <param name="pguid">guid</param>
        /// <param name="courseName">course name</param>
        /// <param name="assignmentId">assignment id</param>
        /// <returns>string</returns>
        private string stmpsendemail(int profId, string pguid, string courseName, int assignmentId)
        {
            string email = _db.User.Where(x => x.Id == profId).First().Email; // gets email of the user

			string assignmentName = _db.Assignment.FirstOrDefault(x => x.AssId == assignmentId).AssignmentName; 

            string uiUrl = _config.GetSection("AppSettings").GetValue<string>("Floss_UI");

			string link = string.Format("{0}/compare/{1}", uiUrl, pguid);

			MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient(); // sets up email
            message.From = new MailAddress("flossresults@gmail.com");
            message.To.Add(new MailAddress(email));
            message.Subject = string.Format("FLOSS: Comparison Results for {0}/{1}", courseName, assignmentName);
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
            } catch (Exception ex) // in case error occured
            {
                return ex.Message;
            }

            // check config settings to get site and use guid
            return "ok";
        }
        
        /// <summary>
        /// get a list of results for a prof
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListofResults")]
        [Authorize(Policy = "RoleProfessor")]
        public ActionResult<List<string>> GetListOfResults()
        {
            var profId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);  // gets the claim id 
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "..");
            string path = string.Format("{0}\\{1}\\",
                pathToSave, "ComparisonResults");
            List<string> LinkIds = new List<string>();
            if (Directory.Exists(path)) //if folder exists
            {
                foreach (string f in Directory.GetFiles(path)) // check through each file
                {
                    string fileid = Path.GetFileName(f);
                    if (fileid.StartsWith(profId.ToString())) // file the file starts with the prof id
                    {
                        LinkIds.Add(fileid); // add it to list
                    }
                }
                return Ok(LinkIds);
            } 
            return BadRequest("There are no results.");
        } // returns a list of ids for the existing files


        /// <summary>
        /// saves the comparison results
        /// </summary>
        /// <param name="resultModel">result model</param>
        /// <returns>string</returns>
        [AllowAnonymous] // turn off authentication :D
        [HttpPost]
        [Route("PushCompareResults")]
        public ActionResult<string> PushCompareResults(MessageModel resultModel)
        {
            List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(resultModel.ReportString); // gets a list of reports

            ComparisonResults fByLine = ReceiveDataFromCompareCall(reports, resultModel.Files, resultModel.AssignmentId);  // converts them to results

            string result = JsonConvert.SerializeObject(fByLine).ToString();

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
            string path = string.Format("{0}\\{1}\\",
                pathToSave, "ComparisonResults");

            FileInfo file = new FileInfo(path);
            file.Directory.Create(); // creates the path if it doesn't exist

            Guid g = Guid.NewGuid(); // unique id for file
            string profGuid = string.Format("{0}_{1}", resultModel.ProfId, g.ToString());
            using (StreamWriter fileStream = new StreamWriter(path + profGuid))
            {
                fileStream.Write(result);
            } //write results to file
            var course = _db.Assignment.Include(x => x.Course).Where(x => x.AssId == resultModel.AssignmentId).Select(x => x.Course).FirstOrDefault();
            // get the course name
            string courseName = $"{course.DepartmentName}{course.CourseCode}";


			stmpsendemail(resultModel.ProfId, g.ToString(), courseName, resultModel.AssignmentId); // makes email call
            return g.ToString();  //return id
        }



    }
}

