using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Services;
using Tech_HubAPI.Models;
using Tech_HubAPI.Forms;
using Tech_HubAPI.Models.GitModels;
using System.Collections.Generic;
using System.IO;
using Tech_HubAPI.Models.Git;
using System.Threading.Tasks;

namespace Tech_HubAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly GitService _gitService;
        private readonly DatabaseContext _dbContext;
        private readonly IAuthorizationService _authorizationService;

        public RepositoryController(GitService gitService, DatabaseContext dbContext, IAuthorizationService authorizationService)
        {
            _gitService = gitService;
            _dbContext = dbContext;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("getbranches")]
        public async Task<ActionResult<List<Branch>>> GetBranches([FromBody] GetBranchesForm form)
        {
            var repo = _dbContext.Repositories
                .Where(r => r.Owner.Username == form.Username)
                .Where(r => r.Name == form.RepoName)
                .FirstOrDefault();
            var result = await _authorizationService.AuthorizeAsync(User, repo, "RequireRead");

            if (!result.Succeeded)
            {
                return Unauthorized("You are not authorized for this repo.");
            }

            List<Branch>? branches = null;
            try
            {
                branches = _gitService.GetBranches(form.Username, form.RepoName);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }


            return branches;
        }

        [HttpPost]
        [Route("getdirectorylisting")]
        public ActionResult<List<DirectoryEntry>> GetDirectoryListing([FromBody] GetDirectoryListingForm form)
        {
            List<DirectoryEntry>? listings = null;
            try
            {
                listings = _gitService.GetDirectoryListing(form.Username, form.RepoName, form.Path, form.Branch);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }


            return listings;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public ActionResult CreateRepository([FromBody] string name, bool isPublic)
        {
            User? currentUser = this.GetUser(_dbContext);

            // do not allow non-users to make repos.
            if (currentUser == null)
            {
                return Unauthorized("Must be logged in to create repo.");
            }

            // TODO: Does this break when there are more than 2 of the same repo? This will certainly work with 0 or 1, but after 2
            //       there might be same-named repos that arent caught because its doing "first" and not "all"
            Repository? existingRepo = _dbContext.Repositories
                .Where(r => r.Owner.Username == currentUser.Username)
                .Where(r => r.Name == name)
                .FirstOrDefault();

            // Only block repo creation if its by the same user.
            // Otherwise, there can be same-named repos, just not same user.
            if (existingRepo != null)
            {
                // repo exists with same user. dont allow dupe
                return BadRequest("Repo with that name already exists for current user.");
            }

            // Create the repo on the Git Server
            _gitService.CreateNewRepository(name, currentUser.Username);
            // Create a new repo object as well.
            var newRepo = new Repository(name, currentUser.Id, isPublic);
            _dbContext.Repositories.Add(newRepo);
            _dbContext.SaveChanges(); 
            // Create new permissions to make the currently logged in user the admin
            var newPerms = new RepositoryPermission(currentUser.Id, newRepo.Id, PermissionLevel.Admin);
            _dbContext.RepositoryPermissions.Add(newPerms);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
