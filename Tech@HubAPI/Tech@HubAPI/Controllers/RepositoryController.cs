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
	[Route("[controller]")]
	[ApiController]
	public class RepositoryController
    {
		private readonly GitService _gitService;

        public RepositoryController(GitService gitService)
        {
            _gitService = gitService;
        }

        [HttpPost]
		public ActionResult<Branch[]> GetBranches([FromBody] GetBranchesForm form)
		{

			Branch[] branches = _gitService.GetBranches(form.Username, form.RepoName);

			// Do we need errorchecking on empty branches?

			return branches;
		}
	}
}
