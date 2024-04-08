using System.Collections.Generic;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.ResponseObjects
{
    /// <summary>
    /// A class to shape the JSON response object sent back to the frontend.
    /// This is structured to make sure we safely send back non-sensitive data.
    /// 
    /// Contains a constructor for Repository to make this class essentially act as a wrapper.
    /// </summary>
    public class RepositoryResponse
    {
        /// <summary>
        /// Construct a new repository.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Name of Repo</param>
        /// <param name="owner">Owner Object</param>
        /// <param name="ownerId">Owner ID</param>
        /// <param name="isPublic">True if public, false for private.</param>
        public RepositoryResponse(string name, int ownerId, bool isPublic)
        {
            Name = name;
            OwnerId = ownerId;
            IsPublic = isPublic;
        }

        public RepositoryResponse(Repository repo)
        {
            Name = repo.Name;
            OwnerId = repo.OwnerId;
            IsPublic = repo.IsPublic;
        }

        /// <summary>
        /// The Unique ID for this Repository.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name of this Repository.
        /// </summary>
        public string Name { get; set; }

        public UserResponse? Owner { get; set; }

        /// <summary>
        /// The Repository Owner's ID.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Whether this Repository is Public or Private.
        /// </summary>
        public bool IsPublic { get; set; }
    }
}
