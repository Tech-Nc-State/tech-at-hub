using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                .Select(name => new Branch(name, File.ReadAllText(branchDirectory + "/" + name).Trim()))
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
            if (Directory.Exists(repoDirectory))
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
                // _executeService.ExecuteProcess("chown", "-R", "www-data:www-data", ".");
                //_executeService.ExecuteProcess("chmod", "-R", "755", ".");
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
            {
                throw new Exception("That user doesn't exist");
            }

            string repoDirectory = userDirectory + repository + ".git/";
            if (!Directory.Exists(repoDirectory))
            {
                throw new Exception("The repository does not exist");
            }

            string branchDirectory = repoDirectory
                + (UseTestGitFolder ? "git_folder" : ".git")
                + "/refs/heads";
            if (!Directory.Exists(branchDirectory))
            {
                throw new Exception("The branch does not exist");
            }

            string headHash;
            // Throws exception if file is not found
            headHash = File.ReadAllText(branchDirectory + "/" + branch).Trim();

            _executeService.ExecutableDirectory = _gitBinPath;
            _executeService.WorkingDirectory = branchDirectory;
            string headOutput = _executeService.ExecuteProcess(
                "git", "cat-file", "-p", headHash.Trim());
            var rootHashMatch = Regex.Match(headOutput, "^tree (?<hash>.+)\n");

            string currentHash = rootHashMatch.Groups["hash"].Value;
            string[] pathArray = path.Split('/');
            string currentContents = _executeService.ExecuteProcess(
                    "git", "cat-file", "-p", currentHash);

            if (!string.IsNullOrEmpty(path))
            {
                foreach (string dirName in pathArray)
                {
                    bool matchedDir = false;
                    string[] currentContentsLines = currentContents.Split('\n');
                    foreach (string currentContentLine in currentContentsLines)
                    {
                        Match lineMatch = Regex.Match(currentContentLine, "\\d+ (?<type>blob|tree) (?<hash>[0-9a-f]+)\\s+(?<name>.+)");
                        // line data array: [0] number [1] blob/tree [2] hash [3] name
                        if ("tree".Equals(lineMatch.Groups["type"].Value) && dirName.Equals(lineMatch.Groups["name"].Value))
                        {
                            currentHash = lineMatch.Groups["hash"].Value;
                            matchedDir = true;
                            break;
                        }
                    }

                    if (!matchedDir)
                        throw new Exception("Invalid pathname");

                    currentContents = _executeService.ExecuteProcess(
                        "git", "cat-file", "-p", currentHash);
                }
            }

            string[] directoryLines = currentContents.Split('\n');

            List<DirectoryEntry> directoryListing = new List<DirectoryEntry>();

            foreach (string directoryLine in directoryLines)
            {
                //Add Regex here
                string[] lineData = directoryLine.Split(new char[] { ' ', '\t' });
                string name = lineData[3];
                bool isDir = lineData[1].Equals("tree");
                directoryListing.Add(new DirectoryEntry(name, isDir));
            }

            return directoryListing;
        }
    }
}
