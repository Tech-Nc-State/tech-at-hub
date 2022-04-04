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

        public RepositoryController(GitService gitService)
        {
            _gitService = gitService;
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
            catch (DirectoryNotFoundException)
            {
                return NotFound("Repo not found.");
            }


            return branches;
        }
    }
}
