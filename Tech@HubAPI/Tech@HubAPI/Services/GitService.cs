using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            _baseGitFolder = executeService.WorkingDirectory.Replace("\\", "/") + "git/";
            _gitBinPath = configuration["Environment:GitPath"].Replace("\\", "/");
        }

        // TODO:
        // GetBranches(string username, string repoName)
        // Direcetory structure: username/reponame.git // inside git folder
        // Directory and File classes

        public string[] GetBranches(string username, string repoName)
        {
            // Given a username and repoName, list all stored branch names.

            // Check if user directory exists
            string userDirectory = _baseGitFolder + username + "/";
            if (!Directory.Exists(userDirectory))
            {
                // not sure if we want to return something besides null to indicate that the
                // error occured for the reason "no user"? 
                return null;
            }

            // Check if repo directory exists
            string repoDirectory = userDirectory + repoName + "/";
            if (!Directory.Exists(repoDirectory))
            {
                // repo no exist
                return null;
            }

            // TODO: Check .git/refs/heads and return the names of those tiles
            // string branchDirectory = repoDirectory + 

            // TODO: Also open each file, copy the SHA hash, and attach that to the string
            // reutrn those strings in an array.

            return null; // TODO: Replace
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
    }
}
