using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Floss.Api.Models;
using Floss.Api.FileToModel;
using Floss.Database.Context;
using Floss.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Floss.Api.Controllers
{
    [Route("api/[controller]")]
	[Authorize(Policy = "RoleStudent")]
	[ApiController]
    public class AssignmentSubmissionController : ControllerBase
    {

		FlossContext _db;

		public AssignmentSubmissionController(FlossContext _db)
		{
			this._db = _db;
		}

        /// <summary>
        /// Retrieves the student's assignemnt submission.
        /// </summary>
        /// <param name="assignmentId">Assignment's ID</param>
        /// <returns>Assignment Submission Model</returns>
		[HttpGet]
		[Route("GetAssignmentSubmissions")]
		[Authorize(Policy = "RoleProfessor")]
		public List<AssignmentSubmissionModel> GetAssignmentSubmissions(int assignmentId)
		{
			return _db.AssignmentSubmission.Where(x => x.AssId == assignmentId).Select(x => new AssignmentSubmissionModel
			{
				AssId = x.AssId,
				UserId = x.UserId,
				UserName = x.User.AccountName,
				StudentNumber = x.User.StudentNumber,
				FileType = x.FileType,
				SubmittedDate = x.SubmittedDate,
			}).ToList(); // gets the assignment submitted from a student from the database and returns it
		}

        /// <summary>
        /// Deletes a directory and all of its contents.
        /// </summary>
        /// <param name="targetDir">Target directory</param>
        private void DeleteDirectory(string targetDir)
        {
            if (!Directory.Exists(targetDir))   // method otherwise throws an exception if the directory doesn't exist
                return;
            System.IO.File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir); // gets all the files
            string[] dirs = Directory.GetDirectories(targetDir); // gets all the directories

            foreach (string file in files) // gets all the files from the main directory
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            } // deletes all the files so the directory can be empty

            foreach (string dir in dirs) // deletes all the directories
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        /// <summary>
        /// Student uploads an assignment and assignment gets stripped for comparison.
        /// </summary>
        /// <param name="courseId">course id</param>
        /// <param name="assId">assignment id</param>
        /// <param name="filetype">assignment language</param>
        /// <returns>HTTP response message</returns>
        [HttpPost]
        [Route("UploadAssignment")]
		public async Task<ActionResult> UploadAssignment(int courseId, int assId, string filetype)
        {
            IFormFile postfile;
            string returnMessage;
            var studentId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // gets the student id of who submitted the assignment
            
			var course = _db.Course.FirstOrDefault(x => x.Id == courseId); // gets the course info
			var userRole = _db.UserRole.FirstOrDefault(x => x.UserId == studentId);

			bool isProf = course.ProfId == studentId && userRole.AuthorizationRoleTypeId != 3; // checks if the student is a prof

			var isEnrolled = _db.Enrollment.Any(x => x.UserId == studentId && x.CourseId == courseId); // checks if the student is enrolled in the class

            if (!isEnrolled && !isProf) // makes sure student is in the class
                return BadRequest("Student is not enrolled in this course"); 
            try
            {
				TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); 
				DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone); // gets the current time

				var dbAssignment = _db.Assignment.FirstOrDefault(x => x.AssId == assId); // gets the assignment details form the database

				bool pastDueDate = easternTime > dbAssignment.DueDate && easternTime > dbAssignment.LateDueDate;  // check if its past the due date

				if (pastDueDate) // if past due date & late due date, cannot submit!
					return BadRequest($"Sorry, {dbAssignment.AssignmentName} is past due!");

				postfile = Request.Form.Files.First(); // gets the file submitted 
                if (string.Equals(Path.GetExtension(postfile.FileName), ".zip", StringComparison.OrdinalIgnoreCase)) 
                { // makes sure it's a zip file
					var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ".."); // go up one directory
					pathToSave = Path.Combine(pathToSave, "FlossRepo"); // where its going to be saved
					string path = string.Format("{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}", 
                        pathToSave, course.ProfId, courseId, course.Year, course.Duration, assId, isProf ? -1: studentId); // -1 ID for exemptions, sets the path 

                    DeleteDirectory(path);      // Clears directory of previous assignment submissions

					var filePath = Path.Combine(path, postfile.FileName); // combines the paths
					FileInfo file = new FileInfo(filePath);
                    file.Directory.Create(); // creates the path if it doesn't exist
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await postfile.CopyToAsync(fileStream);
                    } // copies from local upload to filepath


                    string strippedpath = Path.Combine(path, "filtered.txt"); // for the stripped text
                    string temppath = Path.Combine(path, "tmp");
                    Unzip.unpackFiltered(filePath, temppath, filetype);
                    string assignment = CodeStripper.StripAssignment(temppath, filetype); // clears comments and useless stuff for comparison
                    System.IO.File.WriteAllText(strippedpath, assignment);
                    DeleteDirectory(temppath);

					if(!isProf) // if not submitting an exemption
					{
						var assignmentSubmission = _db.AssignmentSubmission.FirstOrDefault(x => x.AssId == assId && x.UserId == studentId);
						if (assignmentSubmission == null) // if its a new submisson 
						{
							assignmentSubmission = new AssignmentSubmission()
							{
								AssId = assId,
								UserId = studentId,
								FilePath = filePath,
								StrippedFilePath = strippedpath,
								FileType = filetype,
								SubmittedDate = easternTime
							};
							_db.AssignmentSubmission.Add(assignmentSubmission); // saves it to the database
						}
						else // updates the submisson
						{
							assignmentSubmission.FilePath = filePath;
							assignmentSubmission.StrippedFilePath = strippedpath;
							assignmentSubmission.FileType = filetype;
							assignmentSubmission.SubmittedDate = easternTime;
						}
					}
					else // submit exemption
					{
						var assignmentExemption = _db.AssignmentExempt.FirstOrDefault(x => x.AssId == assId); // get existing exemption
						if(assignmentExemption == null) 
						{
							assignmentExemption = new AssignmentExempt()
							{
								AssId = assId,
								FilePath = filePath,
								StrippedFilePath = strippedpath,
								FileType = filetype,
							};
							_db.AssignmentExempt.Add(assignmentExemption); 
						} // saves the exemption 
						else
						{
							assignmentExemption.FilePath = filePath;
							assignmentExemption.StrippedFilePath = strippedpath;
							assignmentExemption.FileType = filetype;
						} // if the exemption exists, update it
					}

                    _db.SaveChanges();  // saves to database
                    return Ok();
                }
                else
                    return BadRequest("Failed submission: not a zip file.");
            }
            catch (Exception ex) //if an error occurs
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Downloads assignment for currently authenticated student or passed in studentId. If user is professor of course, downloads exemption.
        /// </summary>
        /// <param name="assignmentId">Assignment id</param>
        /// <param name="studentId">Student id</param>
        /// <returns>Zip file of assignment</returns>
        [HttpGet]
        [Route("DownloadAssignment")]
        [Authorize(Policy = "RoleStudent")]
        public ActionResult<FileStream> DownloadAssignment(int assignmentId, int? studentId = null)
        {
			bool isProf = false;
			if (studentId == null) 
			{
				studentId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // gets the student id

				var course = _db.Assignment.Where(x => x.AssId == assignmentId).Include(x => x.Course).Select(x => x.Course).FirstOrDefault(); // gets the course info
				var userRole = _db.UserRole.FirstOrDefault(x => x.UserId == studentId); // gets the role of the user

				isProf = course.ProfId == studentId && userRole.AuthorizationRoleTypeId != 3; // if is prof
			}


			try
            {
				string path;
				if(isProf) // if professor, download assignment exemption
					path = _db.AssignmentExempt.FirstOrDefault(x => x.AssId.Equals(assignmentId))?.FilePath;
				else // else student, download their submission
					path = _db.AssignmentSubmission.FirstOrDefault(x => x.AssId.Equals(assignmentId) && x.UserId == studentId)?.FilePath;

				if (path == null)
					return Ok("Nothing to see here");

				return new FileStream(path, FileMode.Open, FileAccess.Read); // returns the zip assignment file
			}
            catch (Exception e)
            {
				return BadRequest("Failed to Retrieve Assignment");
			}
        } //returns zip file

        /// <summary>
        /// Deletes a specified assignment.
        /// </summary>
        /// <param name="assignmentId">assignment id</param>
        /// <param name="studentId">student id</param>
        /// <returns>HTTP response message</returns>
		[HttpDelete]
		[Route("DeleteFile")]
		[Authorize(Policy = "RoleProfessor")]
		public ActionResult DeleteFile(int assignmentId, int? studentId = null)
		{
			bool isProf = false;
			if (studentId == null)
			{
				studentId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // gets the student id

				var course = _db.Assignment.Where(x => x.AssId == assignmentId).Include(x => x.Course).Select(x => x.Course).FirstOrDefault(); // gets course info based 
				var userRole = _db.UserRole.FirstOrDefault(x => x.UserId == studentId); // gets the role of the 

				isProf = course.ProfId == studentId && userRole.AuthorizationRoleTypeId != 3; // if is prof
			}


			try
			{
				string path = null;
				if (isProf) // if professor, delete assignment exemption
				{ 
					var assignmentExemption = _db.AssignmentExempt.FirstOrDefault(x => x.AssId.Equals(assignmentId));
					if(assignmentExemption != null) // makes sure it is an record
					{
						path = assignmentExemption.FilePath; // deletes the saved file
						_db.AssignmentExempt.Remove(assignmentExemption); // deletes it from db
					} 
				}
				else // else student, delete their submission
				{ 
					var assignmentSubmission = _db.AssignmentSubmission.FirstOrDefault(x => x.AssId.Equals(assignmentId) && x.UserId == studentId);
					if (assignmentSubmission != null) // makes sure the record exists
					{
						path = assignmentSubmission.FilePath; // deletes the saved file
                        _db.AssignmentSubmission.Remove(assignmentSubmission);// deletes it from db
                    }
				}

				if (path == null)
					return NoContent();

				path = Path.GetFullPath(Path.Combine(path, @"..\"));

				DeleteDirectory(path); // makes sure all the files are deleted 
				_db.SaveChanges(); // makes sure the database is saved

				return Ok();
			}
			catch (Exception e) // in case an error occurs
			{
				return BadRequest("Failed to Retrieve Assignment");
			}
		} 


        /// <summary>
        /// Gets the filepath for the assignment
        /// </summary>
        /// <param name="assignmentId">assignment id</param>
        /// <param name="studentId">student id</param>
        /// <returns>returns filedownloadmodel</returns>
		[HttpGet]
		[Route("GetDownloadName")]
		[Authorize(Policy = "RoleStudent")]
		public ActionResult<FileDownloadModel> GetDownloadName(int assignmentId, int? studentId = null)
		{
			bool isProf = false;
			if (studentId == null) // if the id of the student isnt given to us
			{
				studentId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // retrieve the id from the claim

				var course = _db.Assignment.Where(x => x.AssId == assignmentId).Include(x => x.Course).Select(x => x.Course).FirstOrDefault(); // gets course info
				var userRole = _db.UserRole.FirstOrDefault(x => x.UserId == studentId); // gets the user role

				isProf = course.ProfId == studentId && userRole.AuthorizationRoleTypeId != 3; // if is prof
			}


			try
			{
				string path;
				if (isProf) // if professor, download assignment exemption
					path = _db.AssignmentExempt.FirstOrDefault(x => x.AssId.Equals(assignmentId))?.FilePath;
				else // else student, download their submission
					path = _db.AssignmentSubmission.FirstOrDefault(x => x.AssId.Equals(assignmentId) && x.UserId == studentId)?.FilePath;

				if (path == null) // no assignment
					return NoContent();

				FileDownloadModel result = new FileDownloadModel(); 
				result.FileName = Path.GetFileName(path); // saves the path to the object

				return Ok(result);
			}
			catch (Exception e) // in case an error occurs
			{
				return BadRequest("Failed to Retrieve Assignment");
			}
		} //returns zip file

        /// <summary>
        /// Checks if the assignment is past the due date
        /// </summary>
        /// <param name="assignmentId">assignment id</param>
        /// <returns>bool</returns>
		[HttpGet]
		[Route("IsPastDue")]
		[Authorize(Policy = "RoleStudent")]
		public ActionResult<bool> IsPastDue(int assignmentId)
		{
			TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone); // gets the current time 

			var dbAssignment = _db.Assignment.FirstOrDefault(x => x.AssId == assignmentId); // gets the assignment details

			bool pastDueDate = easternTime > dbAssignment.DueDate && easternTime > dbAssignment.LateDueDate; // does the check

			// if past due date & late due date, cannot submit!
			return pastDueDate;
		}
	}
}