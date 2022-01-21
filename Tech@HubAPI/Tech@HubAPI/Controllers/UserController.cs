using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly DatabaseContext _dbContext;

		public UserController(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		[HttpGet]
		[Route("get/{ID}")]
		public User GetUser(int ID) 
        {
			var user = _dbContext.Users.Where(u => u.Id == ID).FirstOrDefault();

            user.Password = null;
			user.Age = 0;
			user.Email = null;
			user.Salt = null;

            if (user == null)
            {
				return NotFound();
            }
			else
            {
				return user;
            }
        }
	}
}
