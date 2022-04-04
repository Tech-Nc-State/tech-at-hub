using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Services;
using Tech_HubAPI.Models;
using System.Security.Claims;
using System.Collections.Generic;

namespace Tech_HubAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly HashingService _hashingService;
        private readonly JwtService _jwt;

        public UserController(DatabaseContext dbContext, HashingService hashingService, JwtService jwt)
        {
            _jwt = jwt;
            _dbContext = dbContext;
            _hashingService = hashingService;
        }

        [HttpGet]
        [Route("get/{ID}")]
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
        [Route("change")]
        public IActionResult PasswordChange([FromBody] ChangePasswordForm passwordForm)
        {
            User currUser = _dbContext.Users.Where(u => u.Username == passwordForm.UserName).FirstOrDefault();
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
    }
}
