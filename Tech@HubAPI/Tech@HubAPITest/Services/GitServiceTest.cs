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

            List<Branch> branches = _gitService.GetBranches("testUser", "testBranches");

            branches.Count.Should().Be(5);
            //branches[0].Name.Should().Be("master");
            string[] branchNames = branches.Select(branch => branch.Name).ToArray();
            branchNames.Should().Contain("master");
            branchNames.Should().Contain("branch1");
            branchNames.Should().Contain("branch2");
            branchNames.Should().Contain("branch3");
            branchNames.Should().Contain("branch4");
        }

        [Fact]
        public void TestGetBranchesNotExisting()
        {
            Action run = () => _gitService.GetBranches("testUser", "testBranches");
            run.Should().Throw<DirectoryNotFoundException>();
        }
    }
}
