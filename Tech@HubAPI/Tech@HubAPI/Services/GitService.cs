using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tech_HubAPI.Models.Git;
using Tech_HubAPI.Models.GitModels;

namespace Tech_HubAPI.Services
{
    public class GitService
    {
        private readonly bool _windows;
        private readonly string _baseGitFolder;
        private readonly string _gitBinPath;
        private readonly ExecuteService _executeService;

        /// <summary>
        /// Initializes a new <see cref="GitService"/>.
        /// </summary>
        /// <param name="executeService"><see cref="ExecuteService"/> this class will make use of. The
        /// git filesystem is constructed in the working directory of this <see cref="ExecuteService"/></param>
        /// <param name="configuration">Configuration this class will make use of.</param>
        public GitService(ExecuteService executeService, IConfiguration configuration)
        {
            _executeService = new ExecuteService(configuration);
            _windows = configuration["Environment:Platform"] == "Windows";
            _baseGitFolder = executeService.WorkingDirectory.Replace("\\", "/") + "git/";  // TODO: Why is this pointing to wrong place in debug?
            _gitBinPath = configuration["Environment:GitPath"].Replace("\\", "/");
        }

        public bool UseTestGitFolder { get; set; }


        /// <summary>
        /// Gets a list of <c>Branch</c>es in the given user/repo name.
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="repoName">name of the repository</param>
        /// <returns>list of Bracnhes if they exist, null otherwise.</returns>
        public List<Branch> GetBranches(string username, string repoName)
        {
            // Given a username and repoName, list all stored branch names.

            // Check if user directory exists
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                // not sure if we want to return something besides null to indicate that the
                // error occured for the reason "no user"? 
                throw new DirectoryNotFoundException();
            }

            // Check if repo directory exists
            string repoDirectory = userDirectory + repoName + ".git/";
            if (!Directory.Exists(repoDirectory))
            {
                // repo no exist
                throw new DirectoryNotFoundException();
            }
            
            string branchDirectory = repoDirectory 
                + (UseTestGitFolder ? "git_folder" : ".git")
                + "/refs/heads";

            string[] branchNames = new DirectoryInfo(branchDirectory)
                .GetFiles()
                .Select(f => f.Name)
                .ToArray();

            // assign info to branches
            List<Branch> branches = branchNames
                .Select(name => new Branch(name, File.ReadAllText(branchDirectory + "/" + name)))
                .ToList();

            return branches; 
        }

        public void CreateNewRepository(string name, string username)
        {
            // User Directory
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            // Create a new folder for the git repo
            string repoDirectory = userDirectory + name + ".git" + "/";
            if(Directory.Exists(repoDirectory))
            {
                throw new Exception("Repository " + repoDirectory + " already exists.");
            }
            Directory.CreateDirectory(repoDirectory);

            // Set the path prefix and working directory
            string oldExeDirectory = _executeService.ExecutableDirectory;
            _executeService.ExecutableDirectory = _gitBinPath;
            _executeService.WorkingDirectory = repoDirectory;

            // Run the initialize commands
            _executeService.ExecuteProcess("git", "--bare", "init");
            _executeService.ExecuteProcess("git", "update-server-info");
            _executeService.ExecuteProcess("git", "config", "http.receivepack", "true");

            if (!_windows)
            {
                // Permission commands
                _executeService.WorkingDirectory = oldExeDirectory;
                _executeService.ExecuteProcess("chown", "-R", "www-data:www-data", ".");
                _executeService.ExecuteProcess("chmod", "-R", "755", ".");
            }

            // create the repository README
            File.Create(repoDirectory + "README.md")
                .Close();
        }
        
        public List<DirectoryEntry> GetDirectoryListing(
            string username, string repository, string path, string branch)
        {
            string userDirectory = _baseGitFolder + username + "/";

            if (!Directory.Exists(userDirectory))
                return null;
            

            string repoDirectory = userDirectory + repository + ".git" + "/";

            if (!Directory.Exists(repoDirectory))
                return null;

            string oldExeDirectory = _executeService.ExecutableDirectory;
            _executeService.WorkingDirectory = repoDirectory + "/refs/heads";
            string headHash = _executeService.ExecuteProcess("cat", "branch");

            if (headHash.Contains("No such file or directory"))
                return null;

            _executeService.ExecutableDirectory = _gitBinPath;
            string headOutput = _executeService.ExecuteProcess(
                "git", "cat-file", "-p", "currentHash");
            string[] headOutputLines = headOutput.Split('\n');
            string rootHash = headOutputLines[0].Substring(5); //tree X (start of hash)

            string currentHash = rootHash;
            string[] pathArray = path.Split('/');
            string currentContents = "";

            foreach (string dirName in pathArray)
            {
                bool matchedDir = false;
                currentContents = _executeService.ExecuteProcess(
                    "git", "cat-file", "-p", currentHash);
                string[] currentContentsLines = currentContents.Split('\n');
                foreach (string currentContentLine in currentContentsLines)
                {
                    string[] lineData = currentContentLine.Split(new char[] { ' ', '\t' });
                    // line data array: [0] number [1] blob/tree [2] hash [3] name
                    if (lineData[1].Equals("tree") && lineData[3].Equals(dirName))
                    {
                        currentHash = lineData[2];
                        matchedDir = true;
                        break;
                    }
                }

                if (!matchedDir)
                    return null;
            }

            string[] directoryLines = currentContents.Split('\n');

            List<DirectoryEntry> directoryListing = new List<DirectoryEntry>();

            foreach (string directoryLine in directoryLines)
            {
                string[] lineData = directoryLine.Split(new char[] { ' ', '\t' });
                string name = lineData[3];
                bool isDir = lineData[1].Equals("tree");
                directoryListing.Add(new DirectoryEntry(name, isDir));
            }

            return directoryListing;
        }
    }
}
