using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tech_HubAPI.Models;
namespace Tech_HubAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public PermissionController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPut]
        [Authorize]
        public IActionResult SetPermission([FromQuery] int userId, [FromQuery] int repoId, [FromQuery] PermissionLevel level)
        {
            RepositoryPermission? searchPerm = _dbContext.RepositoryPermissions
                .Where(r => r.Id == repoId)
                .FirstOrDefault();

            if (searchPerm == null)
            {
                var perm = new RepositoryPermission(userId, repoId, level);
                _dbContext.RepositoryPermissions.Add(perm);
                _dbContext.SaveChanges();
                return Ok();
            }

            return BadRequest("The permission for this user already exists");
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeletePermissions([FromQuery] int userId, [FromQuery] int repoId)
        {
            RepositoryPermission? searchPerm = _dbContext.RepositoryPermissions
                .Where(r => r.Id == repoId)
                .Where(r => r.User.Id == userId)
                .FirstOrDefault();

            if (searchPerm == null)
            {
                return BadRequest("This user does not have persmissions assigned previously.");
            }

            _dbContext.RepositoryPermissions.Remove(searchPerm);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public ActionResult<PermissionLevel?> GetPermissions(int repoId, int userId)
        {
            RepositoryPermission? searchPerm = _dbContext.RepositoryPermissions
                .Where(r => r.RepositoryId == repoId)
                .Where(r => r.User.Id == userId)
                .FirstOrDefault();

            if (searchPerm == null)
            {
                return NotFound("This user does not exist.");
            }

            return searchPerm.Level;
        }
    }
}
