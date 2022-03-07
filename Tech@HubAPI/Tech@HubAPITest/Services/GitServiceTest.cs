using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models.GitModels;
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
            _gitService.UseTestGitFolder = true;
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
            _fileSystem.ImportFolder("./SampleGitRepos/testBranches.git", "git/testUser/testBranches.git");

            Branch[] branches = _gitService.GetBranches("testUser", "testBranches");

            branches.Length.Should().Be(5);
            branches[0].Should().Be("master");
            branches[1].Should().Be("branch1");
            branches[2].Should().Be("branch2");
            branches[3].Should().Be("branch3");
            branches[4].Should().Be("branch4");
        }
    }
}
