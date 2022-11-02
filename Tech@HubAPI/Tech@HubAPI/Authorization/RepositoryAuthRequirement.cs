using Microsoft.AspNetCore.Authorization;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Authorization
{
    public class RepositoryAuthRequirement : IAuthorizationRequirement
    {
        public RepositoryAuthRequirement(PermissionLevel minimumPermission)
        {
            MinimumPermission = minimumPermission;
        }

        public PermissionLevel MinimumPermission { get; }
    }
}
