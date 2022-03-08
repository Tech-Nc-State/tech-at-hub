﻿using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models.Git;
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

            string[] branches = _gitService.GetBranches("testUser", "testBranches");
            branches.Length.Should().Be(5);
            branches.Should().Contain("master");
            branches.Should().Contain("branch1");
            branches.Should().Contain("branch2");
            branches.Should().Contain("branch3");
            branches.Should().Contain("branch4");
        }
    }
}