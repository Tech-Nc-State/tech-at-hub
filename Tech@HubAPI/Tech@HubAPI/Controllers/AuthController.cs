using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitTest.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;

		public AuthController(ILogger<AuthController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public ActionResult Get()
		{
			// Manual implementation of HTTP Basic authentication

			// Ensure the request contains the Authorization header. If not,
			// challenge the user to provide it
			if (!Request.Headers.ContainsKey("Authorization"))
			{
				Response.Headers.Add("WWW-Authenticate", "Basic realm=\"git\"");
				return Unauthorized();
			}

			// Extract the repository name the user wants to access
			string originalUri = Request.Headers["X-Original-URI"];
			var match = Regex.Match(originalUri, "^/git/(?<resource>.+)\\.git/");
			string resource = match.Groups["resource"].Value;

			string authHeader = Request.Headers["Authorization"];

			// If the auth header does not indicate Basic authentication,
			// give the user a reminder
			if (!authHeader.StartsWith("Basic "))
			{
				Response.Headers.Add("WWW-Authenticate", "Basic realm=\"git\"");
				return Unauthorized();
			}

			// Decode the username+password base64 encoding
			authHeader = authHeader.Remove(0, "Basic ".Length);
			byte[] decodedAuthHeaderBytes = Convert.FromBase64String(authHeader);
			string decodedAuthHeader = Encoding.ASCII.GetString(decodedAuthHeaderBytes);

			string username = decodedAuthHeader.Split(":")[0];
			string password = decodedAuthHeader.Split(":")[1];

			// do some processing on the username, password, and resource, to determine
			// if the user should be given access
			bool result = IsAuthorized(username, password, resource);

			return result ? Created("", "") : Unauthorized();
		}

		private bool IsAuthorized(string username, string password, string resource)
		{
			if (username == "a" && password == "b" && resource == "joey/test")
			{
				return true;
			}

			return false;
		}
	}
}
