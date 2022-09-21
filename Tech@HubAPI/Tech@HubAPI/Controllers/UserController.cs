using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models;
using Tech_HubAPI.Services;

namespace Tech_HubAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly DatabaseContext _dbContext;

		private readonly HashingService _hashingService;

		private readonly string _defaultWorkingDirectory;

		private string[] validImageExtensions = { ".png", ".jpg" };

		public UserController(DatabaseContext dbContext, HashingService hashingService, IConfiguration configuration)
		{
			_dbContext = dbContext;
			_hashingService = hashingService;
			_defaultWorkingDirectory = configuration["Environment:DefaultWorkingDirectory"].Replace("\\", "/");
		}

		[HttpPost]
		[Route("uploadprofilepicture")]
		//[Authorize]
		public async Task<ActionResult> UploadProfilePicture(IFormFile file)
		{

			if (file == null || file.Length == 0)
			{
				return BadRequest("The file is empty.");
			}

			var path = BitConverter.ToString(_hashingService.HashFile(file)).Replace("-", "");

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

			using (var stream = System.IO.File.Create(_defaultWorkingDirectory + "/profile_pictures/" + path + "." + ext))
			{
				await file.CopyToAsync(stream);
			}

			if (string.IsNullOrEmpty(ext) || Array.IndexOf(validImageExtensions, ext) == -1)
			{
				throw new NotSupportedException(ext + "is not a supported extension.");
			}

			var user = this.GetUser(_dbContext);

			user.ProfilePicturePath = path;

			return Ok();
		}

		[HttpGet]
		[Route("getprofilepicture")]
		public async Task<ActionResult>  GetProfilePicture(string username)
        {
			if (username == null || username.Length == 0)
            {
				return BadRequest("The user is empty.");
            }

			var user = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault();

			if(user == null)
            {
				return BadRequest("The username is not found.");
            }

			var path = user.ProfilePicturePath;

			var ext = path.Substring(path.IndexOf("."));

			var stream = System.IO.File.OpenRead(path);

			var mime = "image/jpeg";

			if(ext == "png")
            {
				mime = "image/png";
            }

			return new FileStreamResult(stream, mime);
        }
	}
}
