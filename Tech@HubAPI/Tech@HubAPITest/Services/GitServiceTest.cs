using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Services;
using Tech_HubAPITest.Fixtures;
using Xunit;

namespace Tech_HubAPITest
{
    public class GitServiceTest : IClassFixture<FileSystemFixture>
    {
        private readonly GitService _gitService;
        private readonly FileSystemFixture _fileSystem;

        public GitServiceTest(IConfiguration configuration, FileSystemFixture fileSystem)
        {
            var executeService = new ExecuteService(configuration);
            executeService.WorkingDirectory = fileSystem.RootDirectory;
            _gitService = new GitService(executeService, configuration);
            _fileSystem = fileSystem;
        }

        [Fact]
        public void TestCreateRepository()
        {
            _gitService.CreateNewRepository("MyRepository", "mkmcary");
            Directory.Exists(_fileSystem.RootDirectory + "git/mkmcary/MyRepository.git").Should().BeTrue();
            File.Exists(_fileSystem.RootDirectory + "git/mkmcary/MyRepository.git/README.md").Should().BeTrue();
        }

        [Fact]
        public void TestGetBranches()
        {
            string[] branches = _gitService.GetBranches("joey", "test");
            // TODO: See if these branches match
            if (branches != null) {
                for (int i = 0; i < branches.Length; i++) {
                    Console.WriteLine(branches[i]);
                }
            }
            true.Should().BeFalse("purposeful fail: not implemented."); // Fail lol
        }
    }
}
