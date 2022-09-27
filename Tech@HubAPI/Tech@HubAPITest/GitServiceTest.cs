using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models.Git;
using Tech_HubAPI.Models.GitModels;
using Tech_HubAPI.Services;
using Tech_HubAPITest.Services;
using Xunit;

namespace Tech_HubAPITest
{
    [Collection("DatabaseFileSystemCollection")]
    public class GitServiceTest
    {
        private readonly GitService _gitService;
        private readonly FileSystemService _fileSystem;

        public GitServiceTest(IConfiguration configuration, FileSystemService fileSystem)
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

        [Fact]
        public void TestGitDirectoryListing()
        {
            _fileSystem.ImportFolder("./SampleGitRepos/testDirectoryListing.git", "git/testUser/testDirectoryListing.git");
            List<DirectoryEntry> result;
            string[] entries;

            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "", "master");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("Directory");


            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "Directory", "master");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("File.txt");

            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "", "feature");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("Directory");
            entries.Should().Contain("Directory2");

            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "Directory", "feature");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("File.txt");

            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "Directory2", "feature");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("File2.txt");

            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "Directory", "feature2");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("File4.txt");
            entries.Should().Contain("InnerDirectory");

            result = _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "Directory/InnerDirectory", "feature2");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("File3.txt");

            Action action = () => _gitService.GetDirectoryListing("fakeUser", "testDirectoryListing", "", "master");
            action.Should().Throw<Exception>();

            action = () => _gitService.GetDirectoryListing("testUser", "fakeRepo", "", "master");
            action.Should().Throw<Exception>();

            action = () => _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "", "fake-branch");
            action.Should().Throw<Exception>();

            action = () => _gitService.GetDirectoryListing("testUser", "testDirectoryListing", "fakePath", "master");
            action.Should().Throw<Exception>();
        }
    }
}
