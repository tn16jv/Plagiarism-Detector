using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Floss.Api.Models;
using Floss.Database.Context;
using Floss.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Floss.Api.Controllers
{
    /// <summary>
    /// Assignment Controller
    /// </summary>
    [Route("api/[controller]")]
	[Authorize(Policy = "RoleStudent")]
	[ApiController]
    public class AssignmentController : ControllerBase
    {
        FlossContext _db;
        
		public AssignmentController(FlossContext _db)
		{
			this._db = _db;
		}

        /// <summary>
        /// Retrieves a list of assignments for a given course.
        /// </summary>
        /// <param name="courseId">course id</param>
        /// <returns> List of Assignment models </returns>
		[HttpGet]
        [Route("GetAssignments")]
		[Authorize(Policy = "RoleProfessor")]
		public ActionResult<List<Assignment>> GetAssignments(int courseId)
        {
            var userId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // gets the id of the user who made this call

			var isProfOfCourse = _db.Course.Any(x => x.Id == courseId && x.ProfId == userId); //checks if its the prof course

			if (!isProfOfCourse) // if they arent the prof for that class
				return BadRequest("You are not the professor of this course!");

			return Ok(_db.Assignment.Where(x => x.CourseId == courseId).ToList()); // returns list
        }

        /// <summary>
        /// Retrieves a assignment specified by course and assignment IDs.
        /// </summary>
        /// <param name="courseId">Course ID that the assignment is from</param>
        /// <param name="assignmentId">ID of the assignment itself</param>
        /// <returns>An assignment model</returns>
        [HttpGet]
        [Route("GetAssignment")]
        [Authorize(Policy = "RoleProfessor")]
        public ActionResult<Assignment> GetAssignment(int courseId, int assignmentId)
        {
            var userId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // gets the id of the user who made this call

            var isProfOfCourse = _db.Course.Any(x => x.Id == courseId && x.ProfId == userId);// checks if its the prof course

            if (!isProfOfCourse) // if they arent the prof for that class
                return BadRequest("You are not the professor of this course!");

            return Ok(_db.Assignment.FirstOrDefault(x => x.CourseId == courseId && x.AssId == assignmentId));  // returns list
        }

        /// <summary>
        /// Get a list of assignments for the viewing student from a specified course.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns>List of Assignment models</returns>
        [HttpGet]
		[Route("GetAssignmentsForStudent")]
		public ActionResult<List<Assignment>> GetAssignmentsForStudent(int courseId)
		{
			var userId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);  // gets the id of the user who made this call

            var isEnrolled = _db.Enrollment.Any(x => x.UserId == userId && x.CourseId == courseId); // checks if the student is in the course

			if (!isEnrolled) // If they student isnt enrolled in the course 
				return BadRequest("Student is not enrolled in this course");

			var assignments = _db.Assignment.Where(x => x.CourseId == courseId).Select(x => new StudentAssignmentModel
			{
				AssId = x.AssId,
				AssignmentName = x.AssignmentName,
				CourseId = x.CourseId,
				DueDate = x.DueDate,
				LateDueDate = x.LateDueDate,
				SubmittedDate = x.AssignmentSubmissions.FirstOrDefault(u => u.UserId == userId).SubmittedDate
			}).ToList(); // creates the assignment list

			return Ok(assignments); // returns the list
		}

        /// <summary>
        /// After accepting an Assignment model, it adds that new assignment to the course.
        /// </summary>
        /// <param name="assignment">assignment</param>
        /// <returns>An HTTP response</returns>
		[HttpPost]
        [Route("AddAssignment")]
		[Authorize(Policy = "RoleProfessor")]
		public async Task<ActionResult> AddAssignment(Assignment assignment)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // time
            try
            {
                var exists = _db.Assignment.Any(x => x.AssignmentName == assignment.AssignmentName && x.CourseId == assignment.CourseId); // check if that assignment exists already
                if (exists) // if it exists, dont override it
                    return BadRequest(string.Format("Assignment with name '{0}' already exists for this course.",assignment.AssignmentName));

                //Converts the UTC datetime to EDT datetime for due dates
                assignment.DueDate = TimeZoneInfo.ConvertTimeFromUtc(assignment.DueDate, easternZone); // sets the due time
                assignment.LateDueDate = TimeZoneInfo.ConvertTimeFromUtc(assignment.LateDueDate, easternZone);  // sets the late due date  
                _db.Assignment.Add(assignment); // adds assignment to the database
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e) // an error occured
            {
                return BadRequest("Failed to Add Assignment");
            }
        }

        /// <summary>
        /// Updates an existing assignment.
        /// </summary>
        /// <param name="assignment">assignment</param>
        /// <returns>An HTTP response</returns>
        [HttpPut]
        [Route("UpdateAssignment")]
        [Authorize(Policy = "RoleProfessor")]
        public async Task<ActionResult> UpdateAssignment(Assignment assignment)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // gets the curretn time
            try
            {
                //Converts the UTC datetime to EDT datetime for due dates
                assignment.DueDate = TimeZoneInfo.ConvertTimeFromUtc(assignment.DueDate, easternZone); // sets the due time
                assignment.LateDueDate = TimeZoneInfo.ConvertTimeFromUtc(assignment.LateDueDate, easternZone);// sets the late due date  
                _db.Assignment.Update(assignment); // update assignment to the database
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e) // if some error occured
            {
                return BadRequest("Failed to Update Assignment");
            }
        }

        /// <summary>
        /// Retrieve an assignment information.
        /// </summary>
        /// <param name="assignmentId">assignment id</param>
        /// <returns>Either an HTTP response message or an Assignment's Info</returns>
		[HttpGet]
		[Route("GetAssignmentInfo")]
		[Authorize(Policy = "RoleStudent")]
		public ActionResult<AssignmentInfoModel> GetAssignmentInfo(int assignmentId)
		{
			var assignment = _db.Assignment.Include(x => x.Course).FirstOrDefault(x => x.AssId == assignmentId); // retrieves the assignment with that id

			if (assignment == null) // return nothing if it cant get it
				return NoContent();

			AssignmentInfoModel infoModel = new AssignmentInfoModel
			{
				CourseName = $"{assignment.Course.DepartmentName} {assignment.Course.CourseCode}",
				YearDuration = $"{assignment.Course.Year}/{assignment.Course.Duration}",
				AssignmentName = assignment.AssignmentName,
				DueDate = assignment.DueDate,
				LateDueDate = assignment.LateDueDate,
			}; // sets the infomodel with the assignment details

			return Ok(infoModel);
		}

	}
}