using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tech_HubAPI.Services
{
    public class GitService
    {
        /// <summary>
        /// Base Git Folder
        /// </summary>
        public string BaseGit { get; set; } = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\git\\";

        /// <summary>
        /// 
        /// </summary>
        private readonly ExecuteService executeService;

        public GitService(ExecuteService es)
        {
            executeService = es;
        }

        public void CreateNewRepository(string name, string username)
        {
            // User Directory
            string userDirectory = BaseGit + username + "\\";
            if(!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            // Create a new folder for the git repo
            string repoDirectory = userDirectory + name + ".git\\";
            if(Directory.Exists(repoDirectory))
            {
                throw new Exception("Repository " + repoDirectory + " already exists.");
            }
            Directory.CreateDirectory(repoDirectory);

            // Set the path prefix and working directory
            executeService.PathPrefix = "C:\\Program Files\\Git\\bin\\";
            executeService.WorkingDirectory = repoDirectory;

            // Run the initialize commands
            executeService.ExecuteProcess("git", "--bare", "init");
            executeService.ExecuteProcess("git", "update-server-info");
            executeService.ExecuteProcess("git", "config", "http.receivepack", "true");
            // Permission commands??????

            File.Create(repoDirectory + "testfile.txt");
        }

    }
}
