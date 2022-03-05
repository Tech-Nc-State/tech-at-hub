using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Services;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly DatabaseContext _dbContext;
		private readonly HashingService _hashingService;

		public UserController(DatabaseContext dbContext, HashingService hashingService)
		{
			_dbContext = dbContext;
			_hashingService = hashingService;
		}
		
		[HttpGet]
		[Route("get/{ID}")]
		public ActionResult<User> GetUserById(int ID) 
        {
			var user = _dbContext.Users.Where(u => u.Id == ID).FirstOrDefault();

            if (user == null)
            {
				return NotFound("A user with that ID does not exist.");
            }
			else
            {
				user.Password = null;
				user.Email = null;
				user.Salt = null;
				user.BirthDate = DateTime.MinValue;

				return user;
            }
        }

		[HttpGet]
		[Route("me")]
		public ActionResult<User?> GetSelf()
        {
			var user = this.GetUser(_dbContext);

			if (user != null)
            {
				user.Password = null;
				user.Salt = null;
            }

			return user;
		}

		[HttpPost]
		public ActionResult<User> SignUp([FromBody] SignUpForm form)
        {
			User? existingUser = _dbContext.Users.Where(u => u.Username == form.Username).FirstOrDefault();
			if (existingUser != null)
            {
				return BadRequest("That username already exists.");
            }

			try
            {
				form.Validate();
            }
			catch (ArgumentException e)
            {
				return BadRequest(e.Message);
            }

			byte[] salt = _hashingService.GetSalt();
			byte[] hashedPassword = _hashingService.HashPassword(form.Password, salt);

			DateTime.TryParse(form.BirthDate, out DateTime birthDate);

			var user = new User(form.Username, hashedPassword, salt, form.Email, form.FirstName,
					form.LastName, "", null, birthDate);
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();

			user.Password = null;
			user.Salt = null;

			return user;
		}
	}
}
