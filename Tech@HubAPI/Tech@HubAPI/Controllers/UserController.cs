using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models;
using Ubiety.Dns.Core;

namespace Tech_HubAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly DatabaseContext _dbContext;

		public UserController(DatabaseContext dbContext, IConfiguration configuration)
{
			_configuration = configuration;
			_dbContext = dbContext;
		}

		[HttpPost]
		[Route("login")]
		public IActionResult Login([FromBody] string username, [FromBody] string password)
		{
			// https://delushaandelu.medium.com/jwt-auth-in-asp-net-core-353595b9b7c4
			var user = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault();
			// Check password here

			var authClaims = new List<Claim>
			{
				//new Claim(ClaimTypes.Name, user.Username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};	
				
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT: SecretKey"])); 
			var token = new JwtSecurityToken(
				issuer: _configuration["JWT: ValidIssuer"],
				audience: _configuration["JWT: ValidAudience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)); 
				
			return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
		}
	}
}
