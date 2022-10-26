namespace Tech_HubAPI.Models
{
    public class FileContent
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The size of the file in bytes.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The Contents of the file.
        /// TODO: Consider changing the type of contents to handle other file types?
        /// </summary>
        public string Contents { get; set; }
        //TODO: Add reference to the last commit on this file
        // public Commit LastCommit {get; set;}
    }
}
