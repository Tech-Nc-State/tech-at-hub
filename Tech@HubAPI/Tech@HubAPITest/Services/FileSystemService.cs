using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech_HubAPITest.Services
{
    /// <summary>
    /// Provides an abstraction of a clean file system to each test.
    /// </summary>
    public class FileSystemService : IDisposable
    {
        public FileSystemService()
        {
            RootDirectory = "test-files\\";
            RootDirectory = RootDirectory.Replace("\\", "/");

            Directory.CreateDirectory(RootDirectory);
        }

        /// <summary>
        /// Root directory of the virtual file system. Contains the trailing
        /// slash.
        /// </summary>
        public string RootDirectory { get; private set; }

        public void Dispose()
        {
            Directory.Delete(RootDirectory, true);
        }

        public void ImportFolder(string name, string destName = "")
        {
            CopyDirectory(name, RootDirectory + "/" + destName, true);
        }

        //Taken from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
