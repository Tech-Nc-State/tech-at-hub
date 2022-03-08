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
	public class RepositoryController : ControllerBase
    {
		private readonly GitService _gitService;

        public RepositoryController(GitService gitService)
        {
            _gitService = gitService;
        }

        [HttpPost]
		public ActionResult<Branch[]> GetBranches([FromBody] GetBranchesForm form)
		{
			Branch[] branches = null;
			try
			{
				branches = _gitService.GetBranches(form.Username, form.RepoName);
			}
			catch (Exception ex)
            {
				return NotFound("Error reading files: " + ex.Message);
            }
			if (branches == null)
            {
				return NotFound("Branches not found for requested repository.");
            }


			return branches;
		}
	}
}
