using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
    /// <summary>
    /// The Repository Class will be used to represent all the data needed to track a
    /// repository within the system.  A repository is identified by a unique ID.  It also
    /// has a title, an owner, a file path to the repository contents on disk, and
    /// whether the repository is public or private.
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// The Unique ID for this Repository.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Title of this Repository.
        /// </summary>
        public string? Title { get; set; }

        //public User owner { get; set; }

        /// <summary>
        /// The Repository Owner's ID.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Whether this Repository is Public or Private.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// The File Path to this Repository's contents on disk.
        /// </summary>
        public string? FilePath { get; set; }
    }
}
