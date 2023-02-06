namespace Tech_HubAPI.Models.GitModels
{
    /// <summary>Class <c>Tag</c> contains information
    /// about a tag and its associated commit hash </summary>
    public class Tag
    {
        public Tag(string name, string hash)
        {
            Name = name;
            Hash = hash;
        }


        /// <summary> The name of the tag </summary>
        public string Name { get; set; }
        /// <summary> The commit hash associated with this tag. </summary>
        public string Hash { get; set; }
    }
}
