﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly JwtService _jwt;
        private readonly string _defaultWorkingDirectory;

        public UserController(DatabaseContext dbContext, HashingService hashingService, JwtService jwt, IConfiguration configuration)
        {
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

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || Array.IndexOf(validImageExtensions, ext) == -1)
            {
                throw new NotSupportedException(ext + "is not a supported extension.");
            }

            var tempPath = fixImage(file, ext);

            var tempFile = File.ReadAllBytes(tempPath);

            var path = BitConverter.ToString(_hashingService.HashFile(tempFile)).Replace("-", "");

            using (var stream = System.IO.File.Create(_defaultWorkingDirectory + "/profile_pictures/" + path + ".jpg"))
            {
                await file.CopyToAsync(stream);
            }

            var user = this.GetUser(_dbContext);

            user.ProfilePicturePath = path;

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

            var path = user.ProfilePicturePath;
            var ext = path.Substring(path.IndexOf("."));
            var stream = System.IO.File.OpenRead(path);
            var mime = "image/jpeg";

            if (ext == "png")
            {
                mime = "image/png";
            }

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

            var user = new User(form.Username, hashedPassword, salt, form.Email, form.FirstName,
                    form.LastName, "", null, birthDate);
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
        private Path fixImage(IFormFile file, string ext)
        {

            Path tempPath = System.IO.Path.GetTempPath();
            tempPath = Path.combine(tempPath, "tempFile." + ext);
            using StreamWriter sw = new StreamWriter(tempPath);
            file.CopyTo(sw);

            Image image = Image.FromFile(tempPath);

            Bitmap resizedImg = new Bitmap(96, 96);

            double ratioX = (double)resizedImg.Width / (double)img.Width;
            double ratioY = (double)resizedImg.Height / (double)img.Height;
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            int newHeight = Convert.ToInt32(img.Height * ratio);
            int newWidth = Convert.ToInt32(img.Width * ratio);

            using (Graphics g = Graphics.FromImage(resizedImg))
            {
                g.DrawImage(img, 0, 0, newWidth, newHeight);
            }
            resizedImg.Save(tempPath);
            return tempPath;
        }
    }
}
