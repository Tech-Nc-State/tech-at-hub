namespace Tech_HubAPI.Models
{
    public class RepositoryPermission
    {
        public int Id { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public Repository Repository { get; set; }

        public int RepositoryId { get; set; }

        public PermissionLevel Level { get; set; }

    }

    public enum PermissionLevel
    {
        /// <summary>
        /// Can read and clone repo. Can open and comment on issues
        /// and pull requests.
        /// </summary>
        Read,

        /// <summary>
        /// Can read and clone repo. Can manage issues and pull
        /// requests.
        /// </summary>
        Triage,

        /// <summary>
        /// Can read, clone, and push to repo. Can manage issues
        /// and pull requests.
        /// </summary>
        Write,

        /// <summary>
        /// Can read, clone, and push to repo. Can manage issues,
        /// pull requests, and some repo settings.
        /// </summary>
        Maintain,

        /// <summary>
        /// Can read, clone, and push to repo. Can manage issues,
        /// pull requests, and all repo settings.
        /// </summary>
        Admin
    }
}
