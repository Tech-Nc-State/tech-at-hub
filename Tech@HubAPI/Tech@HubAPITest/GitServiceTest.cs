using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Tech_HubAPI.Models;
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
        public void TestCreateBranch()
        {
            _gitService.CreateNewRepository("MyRepo", "nsfogg");
            Directory.Exists(_fileSystem.RootDirectory + "git/nsfogg/MyRepo.git").Should().BeTrue();

            _gitService.CreateNewBranch("nsfogg", "MyRepo", "newBranch", "branchHash");
            File.Exists(_fileSystem.RootDirectory + "git/nsfogg/MyRepo.git/refs/heads/newBranch").Should().BeTrue();

            List<Branch> branches = _gitService.GetBranches("nsfogg", "MyRepo");
            string[] branchNames = branches.Select(branch => branch.Name).ToArray();
            branchNames.Should().Contain("newBranch");
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
        public void TestGetTags()
        {
            _fileSystem.ImportFolder("./SampleGitRepos/testTags.git", "git/testUser/testTags.git");

            List<Tag> tags = _gitService.GetTags("testUser", "testTags");

            tags.Count.Should().Be(4);
            string[] tagNames = tags.Select(tag => tag.Name).ToArray();
            tagNames.Should().Contain("hello");
            tagNames.Should().Contain("howdu");
            tagNames.Should().Contain("testtag");
            tagNames.Should().Contain("released");

        }

        [Fact]
        public void TestGetTagsNotExisting()
        {
            Action run = () => _gitService.GetTags("testUser", "testTags");
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

        [Fact]
        public void TestGetFileContents()
        {
            _fileSystem.ImportFolder("./SampleGitRepos/testBranches.git", "git/testUser/testBranches.git");

            FileContent fc = _gitService.GetFileContents("testUser", "testBranches", "master", "text.txt");
            fc.Contents.Should().Be("hello\nthere");
        }

        [Fact]
        public void TestGetCommitLog()
        {
            _fileSystem.ImportFolder("./SampleGitRepos/testGetCommitLog.git", "git/testUser/testGetCommitLog.git");

            List<Commit> commits = _gitService.GetCommitLog("testUser", "testGetCommitLog", "master");

            commits.Should().HaveCount(6);

            commits[0].Hash.Should().Be("d8dce1f129397da22bb5558cc0c2410e79813009");
            commits[0].Message.Should().Be("Added objects");
            commits[0].Username.Should().Be("nzbennet@ncsu.edu");
            commits[0].Parent.Should().Be(commits[1]);

            commits[1].Hash.Should().Be("d7a951afe807ba119f41227f1ba749603c8c6e23");
            commits[1].Message.Should().Be("Modified item1.txt");
            commits[1].Username.Should().Be("nzbennet@ncsu.edu");
            commits[1].Parent.Should().Be(commits[2]);

            commits[2].Hash.Should().Be("c3d1e54122e9417fe87a66add541a3cb7d67149c");
            commits[2].Message.Should().Be("Commit 4");
            commits[2].Username.Should().Be("nzbennet@ncsu.edu");
            commits[2].Parent.Should().Be(commits[3]);

            commits[3].Hash.Should().Be("3097cc7fc35f27351cc8e99f2ff7b445961a62e1");
            commits[3].Message.Should().Be("Commit 3");
            commits[3].Username.Should().Be("nzbennet@ncsu.edu");
            commits[3].Parent.Should().Be(commits[4]);

            commits[4].Hash.Should().Be("c7326eaa7436fcd75462f62caf5f9befb05ebabf");
            commits[4].Message.Should().Be("Commit 2");
            commits[4].Username.Should().Be("nzbennet@ncsu.edu");
            commits[4].Parent.Should().Be(commits[5]);

            commits[5].Hash.Should().Be("69c62533d3da440af734fd9aa3d743ef150fc779");
            commits[5].Message.Should().Be("Commit 1");
            commits[5].Username.Should().Be("nzbennet@ncsu.edu");
            commits[5].Parent.Should().BeNull();


        }
    }
}
