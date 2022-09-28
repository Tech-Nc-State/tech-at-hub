using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
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

        
        [HttpPost]
        [Route("add")]
        public IActionResult CreatePermission([FromBody] RepositoryPermission repoPerm)
        {


            RepositoryPermission searchPerm = _dbContext.RepositoryPermissions.Where(r => r.Id == repoPerm.Id).FirstOrDefault();

            if(searchPerm == null)
            {
                _dbContext.RepositoryPermissions.Add(repoPerm);
                _dbContext.SaveChanges();
                return Ok();
            }

            return BadRequest("The permission for this user already exists");
        }

        
        [HttpPut]
        [Route("edit")]
        public IActionResult EditPermissions([FromBody] RepositoryPermission repoPerm)
        {
            RepositoryPermission searchPerm = _dbContext.RepositoryPermissions.Where(r => r.Id == repoPerm.Id).FirstOrDefault();

            if(searchPerm == null)
            {
                return BadRequest("This user does not have persmissions assigned previously.");
            }

            searchPerm.Level = repoPerm.Level;
            _dbContext.RepositoryPermissions.Update(searchPerm);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeletePermissions([FromBody] RepositoryPermission repoPerm)
        {
            RepositoryPermission searchPerm = _dbContext.RepositoryPermissions.Where(r => r.Id == repoPerm.Id).FirstOrDefault();

            if (searchPerm == null)
            {
                return BadRequest("This user does not have persmissions assigned previously.");
            }

            _dbContext.RepositoryPermissions.Remove(searchPerm);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("get")]
        public ActionResult<PermissionLevel?> GetPermissions( int repoId, int userId)
        {
            RepositoryPermission searchPerm = _dbContext.RepositoryPermissions.Where(r => r.RepositoryId == repoId && r.UserId == userId).FirstOrDefault();

            if (searchPerm == null)
            {
                return NotFound("This user does not exist.");
            }
            else
            {
                
                return searchPerm.Level;
            }
      
        }

    }
}
