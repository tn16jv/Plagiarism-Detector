using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Floss.Database.Context;
using Floss.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Floss.Api.Controllers
{
    [Route("api/[controller]")]
	[Authorize(Policy = "RoleStudent")]
	[ApiController]
    public class CourseController : ControllerBase
    {

		FlossContext _db;

		public CourseController(FlossContext _db)
		{
			this._db = _db;
		}

        /// <summary>
        /// gets the course
        /// </summary>
        /// <param name="courseId">course id</param>
        /// <returns>course</returns>
		[HttpGet]
        [Route("GetCourse")]
        [Authorize(Policy = "RoleProfessor")]
        public ActionResult<Course> GetCourse(int courseId)
        {
            var userId = int.Parse(this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // get user id from claim

            var isProfOfCourse = _db.Course.Any(x => x.Id == courseId && x.ProfId == userId); // checks to see if its a prof of the course

            if (!isProfOfCourse) // if it is
                return BadRequest("You are not the professor of this course!");

            return Ok(_db.Course.FirstOrDefault(x => x.Id == courseId)); // return database result of the course
        }

        /// <summary>
        /// gets a list of courses for a prof
        /// </summary>
        /// <param name="profId">prof id</param>
        /// <returns>list of courses</returns>
        [HttpGet]
		[Route("GetCourses")]
		[Authorize(Policy = "RoleProfessor")]
		public ActionResult<List<Course>> GetCourses(int profId)
		{
            try
            {
                return Ok(_db.Course.Where(x => x.ProfId == profId).ToList()); // returns database list of all course that prof teaches
            }
			catch(Exception e) // incase an error occurs
            {
                return BadRequest("Failed to Get Prof Courses");
            }
		}

        /// <summary>
        /// gets a list of enrolled courses for the student
        /// </summary>
        /// <param name="userId">student user id</param>
        /// <returns>list of courses</returns>
		[HttpGet]
		[Route("GetStudentCourses")]
		public ActionResult<List<Course>> GetStudentCourses(int userId)
		{
            try
            {
                return Ok(_db.Enrollment.Where(x => x.UserId == userId).Select(x => x.Course).ToList()); // returns database list of all course that the student is in
            }
            catch(Exception e)  // incase an error occurs
            {
                return BadRequest("Failed to Get Student Courses");
            }			
		}

        /// <summary>
        /// gets course info 
        /// </summary>
        /// <param name="courseId">course id</param>
        /// <returns>course</returns>
		[HttpGet]
		[Route("GetCourseInfo")]
		public Course GetCourseInfo(int courseId)
		{
			return _db.Course.FirstOrDefault(x => x.Id == courseId); // return course info
		} 

        /// <summary>
        /// adds a course to the database
        /// </summary>
        /// <param name="course"></param>
        /// <returns>string</returns>
		[HttpPut]
		[Route("AddCourse")]
		[Authorize(Policy = "RoleProfessor")]
		public async Task<ActionResult> AddCourse(Course course)
		{
            try
            {
                course.DepartmentName = course.DepartmentName.ToUpper();
                course.CourseCode = course.CourseCode.ToUpper();
                course.Duration = course.Duration.ToUpper(); // sets up details for the course

                _db.Course.Add(course);
                _db.SaveChanges(); // saves it to database
                return Ok();
            }
            catch(Exception e) // in case an error occurs
            {
                return BadRequest("Failed to Add Course");
            }			
		}

        /// <summary>
        /// updates a current course
        /// </summary>
        /// <param name="course">course</param>
        /// <returns>string</returns>
        [HttpPut]
        [Route("UpdateCourse")]
        [Authorize(Policy = "RoleProfessor")]
        public async Task<ActionResult> UpdateCourse(Course course)
        {
            try
            {                
                course.DepartmentName = course.DepartmentName.ToUpper();
                course.CourseCode = course.CourseCode.ToUpper();
                course.Duration = course.Duration.ToUpper(); // sets up details for the course
                _db.Course.Update(course);
                _db.SaveChanges(); // saves it to database
                return Ok();
            }
            catch (Exception e) // in case an error occurs
            {
                return BadRequest("Failed to Update Course");
            }
        }
    }
}