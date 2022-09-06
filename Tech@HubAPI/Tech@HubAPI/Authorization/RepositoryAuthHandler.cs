using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Authorization
{
    public class RepositoryAuthHandler : AuthorizationHandler<RepositoryAuthRequirement, Repository>
    {
        private readonly DatabaseContext _dbContext;

        public RepositoryAuthHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RepositoryAuthRequirement requirement, Repository resource)
        {
            // is user authenticated? => context.User.Identity?.IsAuthenticated
            // users username => context.User.FindFirst("Username")?.Value

            // requirement is met => context.Succeed(requirement)

            return Task.CompletedTask;
        }
    }
}
