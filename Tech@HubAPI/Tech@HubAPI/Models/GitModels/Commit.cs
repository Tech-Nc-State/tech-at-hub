using System;

namespace Tech_HubAPI.Models.GitModels
{
    /// <summary>
    /// Contains information about a single Git commit object.
    /// Details its timestamp, username, message, hash, and diff
    /// </summary>
    public class Commit
    {
        public Commit(DateTime timestamp, string username, string message, string hash)
        {
            Timestamp = timestamp;
            Username = username;
            Message = message;
            Hash = hash;
        }

        /// <summary>
        /// The UNIX timestamp of the commit.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The username of the user that made this commit.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The commit message of this commit.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The SHA hash of this commit.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// The parent commit of this commit. Equal to null if this is HEAD. Not used yet.
        /// </summary>
        public Commit Parent { get; set; }

    }
}
