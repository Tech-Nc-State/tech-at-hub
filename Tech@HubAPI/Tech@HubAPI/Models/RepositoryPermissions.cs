namespace Tech_HubAPI.Models
{
    public class RepositoryPermissions
    {
        /// <summary>
        /// The Unique ID for this Repository.
        /// </summary>
        public int Id { get; set; }

        public User User { get; set; }

        /// <summary>
        /// The Repository Owner's ID.
        /// </summary>
        public int UserId { get; set; }

        public Repository Repository { get; set; }

        /// <summary>
        /// The Repository Owner's ID.
        /// </summary>
        public int RepositoryId { get; set; }

        public Levels Level { get; set; }

    }

    public enum Levels
    {
        Viewer, Contributor, Maintainer, Admin
    }
}
