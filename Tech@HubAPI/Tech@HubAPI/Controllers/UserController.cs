using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Tech_HubAPI.Models;
using Tech_HubAPI.Services;

namespace Tech_HubAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GitService _gitService;
        private readonly DatabaseContext _dbContext;
        private readonly HashingService _hashingService;
        private readonly JwtService _jwt;
        private readonly string _defaultWorkingDirectory;

        public UserController(GitService gitService, DatabaseContext dbContext, HashingService hashingService, JwtService jwt, IConfiguration configuration)
        {
            _gitService = gitService;
            _jwt = jwt;
            _dbContext = dbContext;
            _hashingService = hashingService;
            _defaultWorkingDirectory = configuration["Environment:DefaultWorkingDirectory"].Replace("\\", "/");
        }

        [HttpPost]
        [Route("me/profilepicture")]
        [Authorize]
        public async Task<ActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("The file is empty.");
            }

            using var image = await Image.LoadAsync(file.OpenReadStream());
            image.Mutate(x => x.Resize(96, 96));

            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms);

            var path = BitConverter.ToString(_hashingService.HashFile(ms.ToArray())).Replace("-", "") + ".jpg";
            image.SaveAsJpeg(_defaultWorkingDirectory + "/profile_pictures/" + path);

            var user = this.GetUser(_dbContext);
            _dbContext.Users.Update(user);
            user.ProfilePicturePath = path;
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("{username}/profilepicture")]
        public ActionResult GetProfilePicture(string username)
        {
            if (username == null || username.Length == 0)
            {
                return BadRequest("The user is empty.");
            }

            var user = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("The username is not found.");
            }

            string path;
            if (System.IO.File.Exists(user.ProfilePicturePath))
            {
                path = user.ProfilePicturePath;
            }
            else
            {
                path = _defaultWorkingDirectory + "/profile_pictures/" + user.ProfilePicturePath;
            }

            var ext = path.Substring(path.IndexOf("."));
            var stream = System.IO.File.OpenRead(path);
            var mime = "image/jpeg";

            return new FileStreamResult(stream, mime);
        }

        [HttpGet]
        [Route("{ID}")]
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

            string randomPfpPicture = "";
            if (Directory.Exists("Profile_Pictures_JPG/"))
            {
                var rand = new Random();
                var files = Directory.GetFiles("Profile_Pictures_JPG/", "*.jpg");
                if (files.Length != 0)
                {
                    randomPfpPicture = files[rand.Next(files.Length)];
                }
            }


            var user = new User(form.Username, hashedPassword, salt, form.Email, form.FirstName,
                    form.LastName, "", randomPfpPicture, birthDate);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            user.Password = null;
            user.Salt = null;

            return user;
        }

        [HttpPost]
        [Authorize]
        [Route("password")]
        public IActionResult PasswordChange([FromBody] ChangePasswordForm passwordForm)
        {
            User? currUser = _dbContext.Users.Where(u => u.Username == passwordForm.UserName).FirstOrDefault();
            if (currUser == null)
            {
                return NotFound();
            }

            byte[] currSalt = currUser.Salt;
            byte[] oldHashedPassword = _hashingService.HashPassword(passwordForm.OldPassword, currSalt);
            if (_hashingService.ByteCheck(currUser.Password, oldHashedPassword))
            {
                try
                {
                    passwordForm.Validate();
                    byte[] newHashedPassword = _hashingService.HashPassword(passwordForm.NewPassword, currSalt);
                    currUser.Password = newHashedPassword;
                    _dbContext.Users.Update(currUser);
                    _dbContext.SaveChanges();
                    currUser = _dbContext.Users.Where(u => u.Username == passwordForm.UserName).FirstOrDefault();
                    return Ok();
                }
                catch (ArgumentException ex)
                {
                    return Conflict(ex.Message);
                }
            }
            return Conflict("Incorrect password.");
        }


        [HttpGet]
        [Route("{username}/repos")]
        public ActionResult<List<Repository>> GetRepos(string username)
        {
            if (username == null || username.Length == 0)
            {
                return BadRequest("The user is empty.");
            }

            var user = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("The username is not found.");
            }

            List<string> repos = _gitService.GetRepositories(username);

            List<Repository> repoList = new List<Repository>();

            foreach (var repoName in repos)
            {
                var repo = _dbContext.Repositories.Where(r => r.Name == repoName).FirstOrDefault();
                if (repo == null)
                {
                    continue; // do nothing
                }
                repoList.Add(repo);
            }

            return repoList;
        }
    }
}
