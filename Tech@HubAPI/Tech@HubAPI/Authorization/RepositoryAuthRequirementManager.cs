using System;
using System.Collections.Generic;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Authorization
{
    public static class RepositoryAuthRequirementManager
    {
        static RepositoryAuthRequirementManager()
        {
            foreach (var perm in Enum.GetValues<PermissionLevel>())
            {
                Requirements.Add(("Require" + perm.ToString(), new RepositoryAuthRequirement(perm)));
            }
        }

        public static readonly List<(string RequirementName, RepositoryAuthRequirement Requirement)> Requirements = new List<(string, RepositoryAuthRequirement)>();
    }
}
