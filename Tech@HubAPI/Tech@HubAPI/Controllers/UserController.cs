using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Services;
using Tech_HubAPI.Models;
using System.Security.Claims;
using System.Collections.Generic;

namespace Tech_HubAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly DatabaseContext _dbContext;
		private readonly HashingService _hashingService;
		private readonly JwtService _jwt;

		public UserController(DatabaseContext dbContext, HashingService hashingService, JwtService jwt)
		{
			_jwt = jwt;
			_dbContext = dbContext;
			_hashingService = hashingService;
		}

		[HttpPost]
		public ActionResult<User> SignUp([FromBody] SignUpForm form)
        {
			User existingUser = _dbContext.Users.Where(u => u.Username == form.Username).FirstOrDefault();
			if (existingUser != null)
            {
				return BadRequest();
            }
			byte[] salt = _hashingService.GetSalt();
			byte[] hashedPassword = _hashingService.HashPassword(form.Password, salt);
			User user = new User(form.Username, hashedPassword, salt, form.Email, form.FirstName, form.LastName, form.Age, null, null);
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();
			user.Password = null;
			return user;
        }

		[HttpPost]
        [Authorize]
		[Route("change")]
		public IActionResult PasswordChange([FromBody] ChangePasswordForm passwordForm)
		{
			User currUser = _dbContext.Users.Where(u => u.Username == passwordForm.UserName).FirstOrDefault();
			if (currUser == null)
			{
				return NotFound();
			}

			
			byte[] currSalt = currUser.Salt;
			byte[] oldHashedPassword = _hashingService.HashPassword(passwordForm.OldPassword, currSalt);
			if( _hashingService.ByteCheck(currUser.Password,oldHashedPassword))
            {
				if(passwordForm.checkNewPassword(passwordForm.NewPassword, passwordForm.NewPasswordRetyped))
                {
					byte[] newHashedPassword = _hashingService.HashPassword(passwordForm.NewPassword, currSalt);
					currUser.Password = newHashedPassword;
					_dbContext.Users.Update(currUser);
					_dbContext.SaveChanges();
					currUser = _dbContext.Users.Where(u => u.Username == passwordForm.UserName).FirstOrDefault();
					return Ok();
				}

            }
			return Conflict();
		}


	}
}
