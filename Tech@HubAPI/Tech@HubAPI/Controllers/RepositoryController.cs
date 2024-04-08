using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tech_HubAPI.Forms;
using Tech_HubAPI.Models;
using Tech_HubAPI.Models.Git;
using Tech_HubAPI.Models.GitModels;
using Tech_HubAPI.Services;

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

        [HttpGet]
        [Route("{username}/{repoName}/branches")]
        public async Task<ActionResult<List<Branch>>> GetBranches(string username, string repoName)
        {
            var repo = _dbContext.Repositories
                .Where(r => r.Owner.Username == username)
                .Where(r => r.Name == repoName)
                .FirstOrDefault();
            var result = await _authorizationService.AuthorizeAsync(User, repo, "RequireRead");

            if (!result.Succeeded)
            {
                return Unauthorized("You are not authorized for this repo.");
            }

            try
            {
                return _gitService.GetBranches(username, repoName);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{username}/{repoName}/{branchName}/{filepath}")]
        public ActionResult<FileContent> GetFileContent(string username, string repoName, string branchName, string filepath)
        {
            FileContent fc = null;
            try
            {
                fc = _gitService.GetFileContents(username, repoName, branchName, filepath);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(FileNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            return fc;
        }

        [HttpGet]
        [Route("{username}/{repoName}/listing")]
        public async Task<ActionResult<List<DirectoryEntry>>> GetDirectoryListing(
            string username,
            string repoName,
            [FromQuery] string branch,
            [FromQuery] string? path = null)
        {
            var repo = _dbContext.Repositories
                .Where(r => r.Owner.Username == username)
                .Where(r => r.Name == repoName)
                .FirstOrDefault();
            var result = await _authorizationService.AuthorizeAsync(User, repo, "RequireRead");

            if (!result.Succeeded)
            {
                return Unauthorized("You are not authorized for this repo.");
            }

            try
            {
                return _gitService.GetDirectoryListing(username, repoName, path ?? "", branch);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{username}/{repoName}/tags")]
        public async Task<ActionResult<List<Tag>>> GetTags(string username, string repoName)
        {
            var repo = _dbContext.Repositories
                .Where(r => r.Owner.Username == username)
                .Where(r => r.Name == repoName)
                .FirstOrDefault();
            var result = await _authorizationService.AuthorizeAsync(User, repo, "RequireRead"); // user must have read perms to read branch tags.

            if (!result.Succeeded)
            {
                return Unauthorized("You are not authorized for this repo.");
            }

            try
            {
                return _gitService.GetTags(username, repoName);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Repository> CreateRepository([FromBody] CreateRepositoryForm form)
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
                .Where(r => r.Name == form.Name)
                .FirstOrDefault();

            // Only block repo creation if its by the same user.
            // Otherwise, there can be same-named repos, just not same user.
            if (existingRepo != null)
            {
                // repo exists with same user. dont allow dupe
                return BadRequest("Repo with that name already exists for current user.");
            }

            // Create the repo on the Git Server
            _gitService.CreateNewRepository(form.Name, currentUser.Username);

            // Create a new repo object as well.
            var newRepo = new Repository(form.Name, currentUser.Id, form.IsPublic);
            _dbContext.Repositories.Add(newRepo);
            _dbContext.SaveChanges();

            // Create new permissions to make the currently logged in user the admin
            var newPerms = new RepositoryPermission(currentUser.Id, newRepo.Id, PermissionLevel.Admin);
            _dbContext.RepositoryPermissions.Add(newPerms);
            _dbContext.SaveChanges();

            return Ok(newRepo);
        }

        [HttpGet]
        [Route("{username}/{repoName}/{branchName}/commits")]
        public async Task<ActionResult<List<Commit>>> GetCommits(
            string username,
            string repoName,
            string branchName,
            [FromQuery] int start,
            [FromQuery] int perPage
            )
        {
            var repo = _dbContext.Repositories
                .Where(r => r.Owner.Username == username)
                .Where(r => r.Name == repoName)
                .FirstOrDefault();
            var result = await _authorizationService.AuthorizeAsync(User, repo, "RequireRead");

            if (!result.Succeeded)
            {
                return Unauthorized("You are not authorized for this repo.");
            }

            try
            {
                // Do paging here
                List<Commit> commits = _gitService.GetCommitLog(username, repoName, branchName);

                List<Commit> filtered = commits.Skip(start).Take(perPage).ToList();
                return filtered;
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<List<Repository>>> GetRepos(string username)
        {
            var repos = _dbContext.Repositories
                .Where(r => r.Owner.Username == username);

            if (!repos.Any())
            {
                return NotFound("No repos found for user " + username);
            }

            List<Repository> goodRepos = new List<Repository>();
            foreach (var repo in repos)
            {
                // Verify permission to read repo. Add to return only if good.
                var result = await _authorizationService.AuthorizeAsync(User, repo, "RequireRead");

                if (result.Succeeded)
                {
                    // only add good repos.
                    goodRepos.Add(repo);
                }

            }

            return Ok(goodRepos);
        }
    }
}
