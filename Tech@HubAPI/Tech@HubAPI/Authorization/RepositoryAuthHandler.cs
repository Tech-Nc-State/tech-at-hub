using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            // Check Auth
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // Authenticated Users
                string? username = context.User.FindFirst("Username")?.Value;
                User? user = _dbContext.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                if (user == null)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                RepositoryPermission? perm = _dbContext.RepositoryPermissions
                    .Where(rp => rp.User.Id == user.Id)
                    .Where(rp => rp.Repository.Id == resource.Id)
                    .FirstOrDefault();

                if (perm != null && perm.Level >= requirement.MinimumPermission)
                {
                    context.Succeed(requirement);
                }
                else if (perm == null && resource.IsPublic && requirement.MinimumPermission == PermissionLevel.Read)
                {
                    // an authenticated user has read access to public repos by default
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else // For unauthenticated users
            {
                // If the repo is private or the operation requires higher than Read access
                if (resource.IsPublic == false || requirement.MinimumPermission != PermissionLevel.Read)
                {
                    // dont allow any viewing
                    context.Fail();
                }
                else
                {
                    //allow
                    context.Succeed(requirement);
                }

            }

            return Task.CompletedTask;
        }
    }
}
