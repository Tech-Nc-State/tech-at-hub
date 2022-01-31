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

		[HttpPost]
		public ActionResult<User> SignUp([FromBody] SignUpForm form)
        {
			User existingUser = _dbContext.Users.Where(u => u.Username == form.Username).FirstOrDefault();
			if (existingUser != null)
            {
				return BadRequest();
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
			DateTime birthDate;
			if (!DateTime.TryParse(form.BirthDate, out birthDate))
			{
				return BadRequest();
			}

			User user = null;
			user = new User(form.Username, hashedPassword, salt, form.Email, form.FirstName,
					form.LastName, null, null, birthDate);
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();
			user.Password = null;
			user.Salt = null;
			return user;
        }
	}
}
