using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Services;
using Tech_HubAPI.Models;
using Tech_HubAPI.Forms;
using Tech_HubAPI.Models.GitModels;

namespace Tech_HubAPI.Controllers
{
    public class RepositoryController
    {
		private readonly GitService _gitService;

		[HttpPost]
		public ActionResult<Branch[]> GetBranches([FromBody] GetBranchesForm form)
		{
			/*User existingUser = _dbContext.Users.Where(u => u.Username == form.Username).FirstOrDefault();
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
					form.LastName, null, null, birthDate);
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();

			user.Password = null;
			user.Salt = null;*/

			Branch[] branches = _gitService.GetBranches(form.Username, form.RepoName);

			// Do we need errorchecking on empty branches?

			return branches;
		}
	}
}
