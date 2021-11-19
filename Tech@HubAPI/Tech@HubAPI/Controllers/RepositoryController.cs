using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RepositoryController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public RepositoryController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Creates a Repository and adds it to the database
        /// </summary>
        /// <param name="repository"> A repository to add</param>
        /// <returns>OK if repository is added, BadRequest if invalid data is provided</returns>
        [HttpPost]
        public ActionResult CreateRepository(Repository repository)
        {
            Console.WriteLine("It worked 1");
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid repository");
            }
            try
            {
                _dbContext.Repositories.Add(repository);
                _dbContext.SaveChanges();
            } catch (DbUpdateException e)
            {
                return BadRequest("Could not save the repository because: " + e.InnerException.Message);
            }

            Console.WriteLine("It worked 2");
            return Ok();
        }


    }
}