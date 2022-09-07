namespace Tech_HubAPI.Forms
{
    public class GetDirectoryListingForm
    {
        public GetDirectoryListingForm(string username, string repoName, string path, string branch)
        {
            this.Username = username;
            this.RepoName = repoName;
            this.Path = path;
            this.Branch = branch;
        }

        public string Username { get; set; }

        public string RepoName { get; set; }

        public string Path { get; set; }

        public string Branch { get; set; }
    }
}
