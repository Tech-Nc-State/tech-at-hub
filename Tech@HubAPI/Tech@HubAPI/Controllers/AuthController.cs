﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tech_HubAPI.Models;
using Tech_HubAPI.Services;

namespace GitTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JwtService _jwt;
        private readonly HashingService _hashing;
        private readonly DatabaseContext _dbContext;

        public AuthController(DatabaseContext dbContext, JwtService jwt, HashingService hashing, ILogger<AuthController> logger)
        {
            _jwt = jwt;
            _hashing = hashing;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            int minLoginInterval = 3;

            // https://delushaandelu.medium.com/jwt-auth-in-asp-net-core-353595b9b7c4
            var user = _dbContext.Users.Where(u => u.Username == credentials.Username).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            
            string? ipNullable = Request.HttpContext.GetServerVariable("REMOTE_ADDR");
            string ip;
            if (ipNullable == null)
            {
                ip = "localhost";
            }
            else
            {
                ip = (string)ipNullable;
            }
            System.Diagnostics.Debug.WriteLine(ip);

            TimeSpan loginInterval;
            DateTime now = DateTime.Now;

            loginInterval = now - user.LastLoginAttempt;

            user.LastLoginAttempt = now;
            user.LastLoginAttemptIp = ip;

            if (loginInterval.Seconds < minLoginInterval)
            {
                return StatusCode((int)HttpStatusCode.TooManyRequests);
            }

            byte[] hashedPasswordAttempt = _hashing.HashPassword(credentials.Password, user.Salt);
            if (!_hashing.ByteCheck(hashedPasswordAttempt, user.Password))
            {
                return Unauthorized();
            }

            var token = _jwt.GenerateToken(new List<Claim>
            {
                new Claim("Username", user.Username),
                new Claim("FullName", user.FirstName + " " + user.LastName),
                new Claim("Id", user.Id.ToString()),
            });

            return Ok(new
            {
                token = _jwt.SerializeToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet]
        [Route("gitclient")]
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
            User? user = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault();

            if (user != null)
            {
                byte[] currSalt = user.Salt;
                byte[] currHashedPassword = _hashing.HashPassword(password, currSalt);

                if (!_hashing.ByteCheck(currHashedPassword, user.Password))
                {
                    return false;
                }

                return resource.StartsWith(username);
            }

            return false;
        }
    }
}
