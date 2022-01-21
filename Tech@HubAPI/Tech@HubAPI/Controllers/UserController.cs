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
		public User SignUp(SignUpForm form)
        {
			IQueryable query = _dbContext.Users.Where(u => u.Username == form.username);
			if (query != null)
            {
				throw new Exception();
            }
			byte[] salt = _hashingService.GetSalt();
			byte[] hashedPassword = _hashingService.HashPassword(form.password, salt);
			return new User(form.username, hashedPassword, salt, form.email, form.firstName, form.lastName, form.age, null, null);
        }
	}
}
