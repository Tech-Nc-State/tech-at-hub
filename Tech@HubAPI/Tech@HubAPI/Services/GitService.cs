using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Tech_HubAPI.Models;
using Tech_HubAPI.Models.Git;
using Tech_HubAPI.Models.GitModels;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Tech_HubAPI.Services
{
    /// <summary>
    /// Interface to the git subsystem. Use for managing and querying git repositories.
    /// </summary>
    // TODO: Make this class thread safe
    public class GitService
    {
        /// <summary>
        /// All directories containing git repos are expected to end with this postfix
        /// </summary>
        private readonly string _repoDirectoryPostfix = ".git";

        private readonly bool _linux;
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
            _baseGitFolder = executeService.WorkingDirectory.Replace("\\", "/") + "git/";  // TODO: Why is this pointing to wrong place in debug?
            _gitBinPath = configuration["Environment:GitPath"].Replace("\\", "/");
            _linux = configuration["Environment:Platform"] == "Linux";
        }

        /// <summary>
        /// Gets a list of <see cref="Branch"> in the given user/repo name.
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="repoName">name of the repository</param>
        /// <returns>list of Bracnhes</returns>
        /// <exception cref="DirectoryNotFoundException">If the user or repository do not exist</exception>
        public List<Branch> GetBranches(string username, string repoName)
        {
            // Given a username and repoName, list all stored branch names.

            // Check if user directory exists
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                throw new DirectoryNotFoundException("User not found");
            }

            // Check if repo directory exists
            string repoDirectory = userDirectory + repoName + _repoDirectoryPostfix + "/";
            if (!Directory.Exists(repoDirectory))
            {
                // repo no exist
                throw new DirectoryNotFoundException("Repository not found");
            }

            string branchDirectory = repoDirectory + "/refs/heads";

            string[] branchNames = new DirectoryInfo(branchDirectory)
                .GetFiles()
                .Select(f => f.Name)
                .ToArray();

            // assign info to branches
            List<Branch> branches = branchNames
                .Select(name => new Branch(name, System.IO.File.ReadAllText(branchDirectory + "/" + name).Trim()))
                .ToList();

            return branches;
        }

        public void CreateNewBranch(string username, string repository, string branchName, string branchHash)
        {
            // Given a username and repoName, create a new branch repository.

            // Check if user directory exists
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                throw new DirectoryNotFoundException("User not found");
            }

            // Check if repo directory exists
            string repoDirectory = userDirectory + repository + _repoDirectoryPostfix + "/";
            if (!Directory.Exists(repoDirectory))
            {
                // repo no exist
                throw new DirectoryNotFoundException("Repository not found");
            }

            string branchPath = repoDirectory + "refs/heads/" + branchName;

            // Create a new file
            System.IO.File.WriteAllText(branchPath, branchHash);

        }

        /// <summary>
        /// Creates a new git repository for the user
        /// </summary>
        /// <param name="name">Name of the new repository</param>
        /// <param name="username">User to create the repository under</param>
        /// <exception cref="Exception">If a repository with that name already exists for the user</exception>
        public void CreateNewRepository(string name, string username)
        {
            // User Directory
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                // user has no repositories yet, create a user folder for them
                Directory.CreateDirectory(userDirectory);
            }

            // Create a new folder for the git repo
            string repoDirectory = userDirectory + name + _repoDirectoryPostfix + "/";
            if (Directory.Exists(repoDirectory))
            {
                throw new Exception("Repository " + repoDirectory + " already exists.");
            }
            Directory.CreateDirectory(repoDirectory);

            // Set the path prefix and working directory
            string oldExeDirectory = _executeService.ExecutableDirectory;
            string oldWorkingDirectory = _executeService.WorkingDirectory;
            _executeService.ExecutableDirectory = _gitBinPath;
            _executeService.WorkingDirectory = repoDirectory;

            // Run the initialize commands
            try
            {
                _executeService.ExecuteProcess("git", "--bare", "init");
                _executeService.ExecuteProcess("git", "update-server-info");
                _executeService.ExecuteProcess("git", "config", "http.receivepack", "true");

                if (_linux)
                {
                    _executeService.ExecutableDirectory = "/bin/";
                    _executeService.ExecuteProcess("chown", "-R", "www-data:www-data", ".");
                    _executeService.ExecuteProcess("chmod", "-R", "755", ".");
                }
            }
            finally
            {
                _executeService.ExecutableDirectory = oldExeDirectory;
                _executeService.WorkingDirectory = oldWorkingDirectory;
            }
        }

        public void DeleteRepository(string name, string username)
        {
            string userDirectory = _baseGitFolder + username + "/";
            string repoDirectory = userDirectory + name + _repoDirectoryPostfix + "/";
            Directory.Delete(repoDirectory, true);
        }

        /// <summary>
        /// Gets the contents of a directory within a git repository.
        /// </summary>
        /// <param name="username">User who owns the repository</param>
        /// <param name="repository">Name of the repository</param>
        /// <param name="path">Folder path within the repository to get a listing of</param>
        /// <param name="branch">Branch on the repository to search</param>
        /// <returns>Contents of the directory as a list of <see cref="DirectoryEntry"/></returns>
        /// <exception cref="DirectoryNotFoundException">If the user or repository does not exist, or if the path to search does not exist</exception>
        /// <exception cref="FileNotFoundException">If the branch does not exist</exception>
        public List<DirectoryEntry> GetDirectoryListing(
            string username, string repository, string path, string branch)
        {
            string userDirectory = _baseGitFolder + username + "/";

            if (!Directory.Exists(userDirectory))
            {
                throw new Exception("That user doesn't exist");
            }

            string repoDirectory = userDirectory + repository + _repoDirectoryPostfix + "/";
            if (!Directory.Exists(repoDirectory))
            {
                throw new DirectoryNotFoundException("The repository does not exist");
            }

            string branchDirectory = repoDirectory + "/refs/heads";
            if (!Directory.Exists(branchDirectory))
            {
                throw new DirectoryNotFoundException("The branch does not exist");
            }

            string headHash;
            // Throws exception if file is not found
            headHash = System.IO.File.ReadAllText(branchDirectory + "/" + branch).Trim();

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
                    {
                        throw new DirectoryNotFoundException("Invalid pathname");
                    }

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
        /// <summary>
        /// Gets the contents of a given file in a specific branch by reconstructing
        /// the associated Git objects. Returns a <see cref="FileContent"/> object that
        /// contains the file's contents on that branch.
        /// </summary>
        /// <param name="username">The repo owner's username.</param>
        /// <param name="repositoryName">The name of the repository.</param>
        /// <param name="branchName">Selected branch from the repository.</param>
        /// <param name="filePath">Path to a specific file within the repo.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">if given user, repo, or branch isnt found.</exception>
        public FileContent GetFileContents(string username, string repositoryName, string branchName, string filePath)
        {
            // Check if user directory exists
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                throw new DirectoryNotFoundException("User not found");
            }

            // Check if repo directory exists
            string repoDirectory = userDirectory + repositoryName + _repoDirectoryPostfix + "/";
            if (!Directory.Exists(repoDirectory))
            {
                // repo no exist
                throw new DirectoryNotFoundException("Repository not found");
            }

            string branchDirectory = repoDirectory + "/refs/heads";
            if (!Directory.Exists(branchDirectory))
            {
                throw new DirectoryNotFoundException("The branch does not exist");
            }

            // Throws exception if file is not found
            string commitHash = System.IO.File.ReadAllText(branchDirectory + "/" + branchName).Trim();

            // Set up the execute service
            _executeService.ExecutableDirectory = _gitBinPath;
            _executeService.WorkingDirectory = branchDirectory;

            // Run git cat-file
            string rawCommitData = _executeService.ExecuteProcess("git", "cat-file", "-p", commitHash);
            Match treeMatch = Regex.Match(rawCommitData, "tree (?<hash>.+)\n");

            // We will need to go through the entire file path structure
            string currentHash = treeMatch.Groups["hash"].Value.Trim();
            string[] pathArray = filePath.Split('/');
            string currentContents = "";

            // For each directory in the file path
            foreach (string dirName in pathArray)
            {
                // Run git-catfile
                bool matchedDir = false;
                currentContents = _executeService.ExecuteProcess("git", "cat-file", "-p", currentHash);

                // Final element of the path is the name of the file
                bool findFile = dirName == pathArray[pathArray.Length - 1];

                // Go through output and find the appropriate tree to explore.
                string[] currentContentLines = currentContents.Split('\n');
                foreach (string currentContentLine in currentContentLines)
                {
                    Match lineMatch = Regex.Match(currentContentLine, "\\d+ (?<type>blob|tree) (?<hash>[0-9a-f]+)\\s+(?<name>.+)");
                    // line data array: [0] number [1] blob/tree [2] hash [3] name
                    if ((!findFile && lineMatch.Groups["type"].Value == "tree" && lineMatch.Groups["name"].Value == dirName)
                        || (findFile && lineMatch.Groups["type"].Value == "blob" && lineMatch.Groups["name"].Value == dirName))
                    {
                        currentHash = lineMatch.Groups["hash"].Value;
                        matchedDir = true;
                        break;
                    }
                }

                if (!matchedDir)
                {
                    throw new DirectoryNotFoundException("Invalid pathname");
                }
            }

            // run git cat-file one more time to get the file contents
            string rawFileContents = _executeService.ExecuteProcess("git", "cat-file", "-p", currentHash);

            // currentContents now contains the contents of the file
            FileContent fc = new FileContent();
            fc.Name = pathArray[pathArray.Length - 1];
            fc.Size = rawFileContents.Length;

            // Assign the contents
            fc.Contents = rawFileContents;

            // Return the filecontent
            return fc;
        }

        /// <summary>
        /// Returns a list of all Commits that make up the current branch.
        /// </summary>
        /// <param name="username"></param> the repo owner's username
        /// <param name="repository"></param> the repo name
        /// <param name="branch"></param> the branch to get commits of
        /// <returns>A list of Commit objects that makes up the current branch.</returns>
        public List<Commit> GetCommitLog(string username, string repository, string branchName)
        {
            string userDirectory = _baseGitFolder + username + "/";

            if (!Directory.Exists(userDirectory))
            {
                throw new Exception("That user doesn't exist");
            }

            string repoDirectory = userDirectory + repository + _repoDirectoryPostfix + "/";
            if (!Directory.Exists(repoDirectory))
            {
                throw new DirectoryNotFoundException("The repository does not exist");
            }

            string branchDirectory = repoDirectory + "/refs/heads";
            if (!Directory.Exists(branchDirectory))
            {
                throw new DirectoryNotFoundException("The branch does not exist");
            }

            // Get the list of commits from the file system, make sure u find the right one if it exists
            // Make the Commit objects into an array and return this. See GetBranches(), probably.

            // Throws exception if file is not found
            string commitHash = System.IO.File.ReadAllText(branchDirectory + "/" + branchName).Trim();

            // Setup the list to be ready to go.
            List<Commit> commitList = new List<Commit>();

            // Set up the execute service
            _executeService.ExecutableDirectory = _gitBinPath;
            _executeService.WorkingDirectory = branchDirectory;

            // Regex Strings
            // It's not pretty but it works.
            string infoRegex = "tree (?<tree>.+)\nparent (?<parent>.+)\nauthor (?<author>.+) <(?<authorEmail>.+)> (?<authorTimestamp>[0-9]+) (?<authorTimezone>-?[0-9]+).*\ncommitter (?<comitter>.+) <(?<committerEmail>.+)> (?<comitterTimestamp>[0-9]+) (?<comitterTimezone>-?[0-9]+)(\\s)+(?<message>(.+(\\s)*))";
            string parentlessRegex = "tree (?<tree>.+)(\\s)author (?<author>.+) <(?<authorEmail>.+)> (?<authorTimestamp>[0-9]+) (?<authorTimezone>-?[0-9]+).*\\ncommitter (?<comitter>.+) <(?<committerEmail>.+)> (?<comitterTimestamp>[0-9]+) (?<comitterTimezone>-?[0-9]+)(\\s)+(?<message>(.+(\\s)*))";
            string parentRegex = "parent (?<hash>.+)\n";

            // Run git cat-file
            string rawCommitData = _executeService.ExecuteProcess("git", "cat-file", "-p", commitHash); // this could just be branch name.
            Match parentMatch = Regex.Match(rawCommitData, "parent (?<hash>.+)\n");
            Match infoMatch = Regex.Match(rawCommitData, infoRegex);

            // TODO: run cat-file -p on the commit 


            while (parentMatch.Success)
            {
                // Keep trying to rematch and make new Commits to add to the list
                // The commits will probably be added in reverse chronological order.
                Commit newCommit = new Commit(UnixTimeStampToDateTime(double.Parse(infoMatch.Groups["authorTimestamp"].Value)),
                    infoMatch.Groups["authorEmail"].Value,
                    infoMatch.Groups["message"].Value,
                    commitHash);


                // set previous's commits parent
                if (commitList.Count > 0)
                {
                    commitList.Last().Parent = newCommit;
                }
                

                // add new commit to back.
                commitList.Add(newCommit);

                // replace current commit hash
                commitHash = parentMatch.Groups["hash"].Value;

                // get new commit data.
                rawCommitData = _executeService.ExecuteProcess("git", "cat-file", "-p", commitHash);
                parentMatch = Regex.Match(rawCommitData, parentRegex);
                infoMatch = Regex.Match(rawCommitData, infoRegex);
            }

            infoMatch = Regex.Match(rawCommitData, parentlessRegex);
            // Tack on the final commit, since it doesnt have a parent.
            Commit lastCommit = new Commit(UnixTimeStampToDateTime(double.Parse(infoMatch.Groups["authorTimestamp"].Value)),
                    infoMatch.Groups["authorEmail"].Value,
                    infoMatch.Groups["message"].Value,
                    commitHash);

            // set previous's commits parent
            if (commitList.Count > 0)
            {
                commitList.Last().Parent = lastCommit;
            }

            commitList.Add(lastCommit);

            

            return commitList; // TODO: Rework ths to return a single Commit instead of a list.
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        /// Gets a list of <see cref="Tag">s in the given user/repo name.
        /// Will return empty list if no tags exist.
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="repoName">name of the repository</param>
        /// <returns>list of Tags</returns>
        /// <exception cref="DirectoryNotFoundException">If the user or repository do not exist</exception>
        public List<Tag> GetTags(string username, string repositoryName)
        {
            // Given a username and repoName, list all stored tags.

            // Check if user directory exists
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                throw new DirectoryNotFoundException("User not found");
            }

            // Check if repo directory exists
            string repoDirectory = userDirectory + repositoryName + _repoDirectoryPostfix + "/";
            if (!Directory.Exists(repoDirectory))
            {
                // repo no exist
                throw new DirectoryNotFoundException("Repository not found");
            }

            string tagDirectory = repoDirectory + "/refs/tags";

            string[] tagNames = new DirectoryInfo(tagDirectory)
                .GetFiles()
                .Select(f => f.Name)
                .ToArray();

            // assign info to tags
            List<Tag> tags = tagNames
                .Select(name => new Tag(name, System.IO.File.ReadAllText(tagDirectory + "/" + name).Trim()))
                .ToList();

            return tags;
        }
    }
}
