using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
	}
}
