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

namespace Tech_HubAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly GitService _gitService;
        private readonly DatabaseContext _dbContext;

        public RepositoryController(GitService gitService, DatabaseContext dbContext)
        {
            _gitService = gitService;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("getbranches")]
        public ActionResult<List<Branch>> GetBranches([FromBody] GetBranchesForm form)
        {
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
