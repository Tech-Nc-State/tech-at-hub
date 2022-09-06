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
        public ActionResult CreateRepository([FromBody] string name)
        {
            try
            {
                _gitService.CreateNewRepository(name, this.GetUser(_dbContext).Username);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
