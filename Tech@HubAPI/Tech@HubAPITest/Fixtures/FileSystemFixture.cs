using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech_HubAPITest.Fixtures
{
    /// <summary>
    /// Provides an abstraction of a clean file system to each test.
    /// </summary>
    public class FileSystemFixture : IDisposable
    {
        public FileSystemFixture(IConfiguration configuration)
        {
            // ensure generated directory names are unique because tests may be run in parallel
            RootDirectory = configuration["Environment:DefaultWorkingDirectory"] + "test-files-" + new Random().Next() + "\\";
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
    }
}
