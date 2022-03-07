namespace Tech_HubAPI.Forms
{
    public class GetBranchesForm
    {
        public GetBranchesForm(string username, string repoName)
        {
            this.Username = username;
            this.RepoName = repoName;
        }

        public string Username { get; set; }

        public string RepoName { get; set; }
    }
}
