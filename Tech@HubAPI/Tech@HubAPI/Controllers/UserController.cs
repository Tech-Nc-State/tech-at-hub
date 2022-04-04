using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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

		private string[] validImageExtensions = { ".png", ".jpg" };

		public UserController(DatabaseContext dbContext, HashingService hashingService)
		{
			_dbContext = dbContext;
			_hashingService = hashingService;
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

			var path = Encoding.ASCII.GetString(_hashingService.HashFile(file));

			using (var stream = System.IO.File.Create(path))
			{
				await file.CopyToAsync(stream);
			}

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

			if (string.IsNullOrEmpty(ext) || Array.IndexOf(validImageExtensions, ext) == -1)
			{
				throw new NotSupportedException(ext + "is not a supported extension.");
			}

			var user = this.GetUser(_dbContext);

			user.ProfilePicturePath = path;

			return null;
		}
	}
}
