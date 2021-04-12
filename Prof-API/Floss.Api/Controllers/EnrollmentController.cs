using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Floss.Api.Helpers;
using Floss.Api.Models;
using Floss.Database.Context;
using Floss.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Floss.Api.Controllers
{
    [Route("api/[controller]")]
	[Authorize(Policy = "RoleProfessor")]
	[ApiController]
    public class EnrollmentController : ControllerBase
    {

		FlossContext _db;

		public EnrollmentController(FlossContext _db)
		{
			this._db = _db;
		}

        /// <summary>
        /// gets a list of students in a course
        /// </summary>
        /// <param name="courseId">course id</param>
        /// <returns>list of users</returns>
		[HttpGet]
		[Route("GetEnrolledStudents")]
		[Authorize(Policy = "RoleProfessor")]
		public ActionResult<List<User>> GetEnrolledStudents(int courseId)
		{
			return Ok(_db.Enrollment.Where(x => x.CourseId == courseId).Select(x => x.User).ToList()); // get list of users from a course
		}

        /// <summary>
        /// removes a student from a course
        /// </summary>
        /// <param name="studentId">student id</param>
        /// <param name="courseId">course id</param>
        /// <returns>string</returns>
        [HttpDelete]
        [Route("RemoveStudentFromCourse")]
        [Authorize(Policy = "RoleProfessor")]
        public ActionResult RemoveStudentFromCourse(int studentId, int courseId)
        {
            try
            {
                var enrollment = _db.Enrollment.FirstOrDefault(x => x.UserId == studentId && x.CourseId == courseId); // does a check
                if (enrollment != null) // makes sure the student is enrolled
                {
                    _db.Enrollment.Remove(enrollment);
                    _db.SaveChanges(); // removes them from db
                }
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// enrolls students to a course
        /// </summary>
        /// <param name="model">enrollstudentmodel</param>
        /// <returns>string</returns>
        [HttpPost]
		[Route("EnrollStudents")]
		[Authorize(Policy = "RoleProfessor")]
		public ActionResult EnrollStudents(EnrollStudentModel model)
		{
			try
			{
				if (model.StudentEmails == null || model.StudentEmails == "")
					return Ok(); // No point in throwing error, since nothing broke, they just clicked enroll with an empty user set

				var emailArr = model.StudentEmails.Split(new Char[] { ' ', ',', ';', ':' });
				var emailList = emailArr.Where(x => !string.IsNullOrWhiteSpace(x)).ToList(); // remove excess records and sets up list

				foreach (var emailString in emailList) // for each email
				{
					var email = EmailValidator.ValidateStudentEmail(emailString); // makes sure a student is in a class
					if (email == null)
						continue;

					string emailAddress = email.Address.ToLower();

					var student = _db.User.Include(x => x.Enrollment).FirstOrDefault(x => x.Email.Equals(emailAddress)); // gets student record from db 


					if (student == null) // if doesn't exist in DB
					{
						student = new User // create new user
						{
							Domain = email.Host.ToLower(),
							AccountName = email.User.ToLower(),
							DisplayName = "",
							FullName = "",
							Email = emailAddress,
							StudentNumber = "", 
							UserRoles = new List<UserRole>(),
						};

						var role = new UserRole
						{
							AuthorizationRoleTypeId = 3,
						}; // makes them a student

						student.UserRoles.Add(role); // assign role of student
						_db.User.Add(student);
					} 
					else
					{
						if (student.Enrollment.Any(x => x.CourseId == model.CourseId)) // if student is already enrolled, skip em
							continue;
					}

					var enrollment = new Enrollment
					{
						CourseId = model.CourseId,
					}; // saves the course id

					student.Enrollment.Add(enrollment);

					_db.SaveChanges(); // saves it to db
				}
			}
			catch(Exception e) // in case an error occurs
			{
				return BadRequest(e.Message);
			}

			return Ok();
		}
	}
}