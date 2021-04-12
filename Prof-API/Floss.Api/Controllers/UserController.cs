using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
	[Authorize(Policy = "RoleStudent")]
	[ApiController]
    public class UserController : ControllerBase
    {
		FlossContext _db;

		public UserController(FlossContext _db)
		{
			this._db = _db;
		}

        /// <summary>
        /// get a user from the db
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user model</returns>
		[HttpGet]
		[Route("GetUser")]
		[Authorize(Policy = "RoleStudent")]
		public ActionResult<UserModel> GetUser(int? userId = null)
		{
			if(userId == null) 
				userId = int.Parse(this.User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value); // get id from claims

			var x = _db.User.Include("UserRoles.AuthorizationRoleType").FirstOrDefault(u => u.Id == userId); // get role

			if (x == null)
				return BadRequest("User not found in database!");

			var role = x.UserRoles.FirstOrDefault().AuthorizationRoleType; //role

			var user = new UserModel // set up user info
			{
				Id = x.Id,
				StudentNumber = x.StudentNumber,
				AccountName = x.AccountName,
				DisplayName = x.DisplayName,
				Domain = x.Domain,
				Email = x.Email,
				FullName = x.FullName,
				RoleName = role.RoleName,
				RoleId = role.Id,
			};

			return Ok(user); // returns user
		}

        /// <summary>
        /// gets a list of users
        /// </summary>
        /// <returns>list of users</returns>
		[HttpGet]
		[Route("GetUsers")]
		[Authorize(Policy = "RoleProfessor")]
		public ActionResult<List<User>> GetUsers()
		{
            // gets all users from db
			var users = _db.User.Select(x => 
				new UserModel
				{
					Id = x.Id,
					StudentNumber = x.StudentNumber,
					AccountName = x.AccountName,
					DisplayName = x.DisplayName,
					Domain = x.Domain,
					Email = x.Email,
					FullName = x.FullName,
					RoleName = x.UserRoles.FirstOrDefault().AuthorizationRoleType.RoleName,
				})
				.ToList(); // makes list of users

			return Ok(users); // return it
		}

        /// <summary>
        /// update a user in the database
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>string</returns>
        [HttpPut]
        [Route("UpdateUser")]
        [Authorize(Policy = "RoleStudent")]
        public async Task<ActionResult> UpdateUser(User user)
        {
            try
            {                
                _db.User.Update(user); // changes user
                _db.SaveChanges();
                return Ok(); //updates a user
            }
            catch (Exception e) // if error occurs
            {
                return BadRequest("Failed to Update User");
            }
        }

        /// <summary>
        /// Update user by an admin 
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns>string</returns>
		[HttpPut]
		[Route("AdminUpdateUser")]
		[Authorize(Policy = "RoleSystemAdmin")]
		public async Task<ActionResult> AdminUpdateUser(UserModel userModel)
		{
			try
			{
				var user = _db.User.Include(x => x.UserRoles).FirstOrDefault(x => x.Id == userModel.Id); // gets user from db
                // sets up user info 
				user.Id = userModel.Id;
				user.StudentNumber = userModel.StudentNumber;
				user.AccountName = userModel.AccountName;
				user.DisplayName = userModel.DisplayName;
				user.Domain = userModel.Domain;
				user.Email = userModel.Email;
				user.FullName = userModel.FullName;

				var role = new UserRole // sets role
				{
					AuthorizationRoleTypeId = userModel.RoleId,
				};

				// if user already has a role
				if (user.UserRoles.Any())
				{
					var currentRole = user.UserRoles.FirstOrDefault(); // get the role
					currentRole.AuthorizationRoleTypeId = role.AuthorizationRoleTypeId; // update it
				}
				else
				{
					user.UserRoles.Add(role); // add new one
				}

				_db.User.Update(user); // save to db
				_db.SaveChanges();
				return Ok();
			}
			catch (Exception e) // if any errors occur
			{
				return BadRequest("Failed to Update User");
			}
		}


		[HttpPost]
		[Route("AddUser")]
		[Authorize(Policy = "RoleSystemAdmin")]
		public async Task<ActionResult> AddUser(UserModel userModel)
		{
			try
			{
				var email = EmailValidator.ValidateBrockEmail(userModel.Email); // check if its a valid email

				if(email == null) // cant be not valid
					return BadRequest(string.Format("The email address you entered was invalid (@brocku.ca only)"));

				string emailAddress = email.Address.ToLower();

				var existingUser = _db.User.FirstOrDefault(x => x.Email.Equals(emailAddress)); // check if its an existing user

				if (existingUser != null)
					return BadRequest(string.Format("A user with email '{0}' already exists in the database.", emailAddress));

				User user = new User // create new user
				{
					Domain = email.Host.ToLower(),
					AccountName = email.User.ToLower(),
					DisplayName = userModel.DisplayName,
					FullName = userModel.DisplayName,
					Email = emailAddress,
					StudentNumber = userModel.StudentNumber, 
					UserRoles = new List<UserRole>(),
				};

				var role = new UserRole // role
				{
					AuthorizationRoleTypeId = userModel.RoleId,
				};

				user.UserRoles.Add(role); // assign role of student
				_db.User.Add(user);

				_db.SaveChanges(); // save to db
				return Ok();
			}
			catch (Exception e) // if any errors occur
			{
				return BadRequest("Failed to add user");
			}
		}
	}
}