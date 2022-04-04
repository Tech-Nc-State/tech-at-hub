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
    [Collection("FileSystemCollection")]
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

        [Fact]
        public void TestGetDirectoryListing() {
            _fileSystem.ImportFolder("./SampleGitRepos/listDirectoryTest.git", "git/testUser/listDirectoryTest.git");
            List<DirectoryEntry> result;
            string[] entries;

            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "", "master");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("rootDir");
            entries.Should().Contain("rootFile.txt");
            entries.Should().Contain("anotherRootFile.txt");


            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "rootDir", "master");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("nestedDir");
            entries.Should().Contain("nestedFile.txt");
            entries.Should().Contain("anotherNestedFile.txt");


            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "rootDir/nestedDir", "master");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("doublyNestedFile.txt");
            entries.Should().Contain("anotherDoublyNestedFile.txt");

            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "", "alternate-branch");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("rootDir");
            entries.Should().Contain("altRootDir");
            entries.Should().Contain("rootFile.txt");
            entries.Should().Contain("anotherRootFile.txt");

            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "altRootDir", "alternate-branch");
            result.Should().NotBeNull();
            entries = result.Select(e => e.Name).ToArray();
            entries.Should().Contain("altNestedFile.txt");

            Action action = () => _gitService.GetDirectoryListing("fakeUser", "listDirectoryTest", "", "master");
            action.Should().Throw<Exception>();

            result = _gitService.GetDirectoryListing("testUser", "fakeRepo", "", "master");
            result.Should().BeNull();

            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "", "fake-branch");
            result.Should().BeNull();

            result = _gitService.GetDirectoryListing("testUser", "listDirectoryTest", "fakePath", "master");
            result.Should().BeNull();
        }
    }
}
