using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models;
using Xunit;
using Tech_HubAPI.Services; // This lets me call Execute.cs (same namespace)
using System.IO;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using Tech_HubAPITest.Services;

namespace Tech_HubAPITest
{
    [Collection("FileSystemCollection")]
    public class ExecuteServiceTest
    {
        private readonly ExecuteService _executeService;
        private readonly FileSystemService _fileSystem;

        public ExecuteServiceTest(IConfiguration configuration, FileSystemService fileSystem)
        {
            _executeService = new ExecuteService(configuration);
            _fileSystem = fileSystem;
        }

        [Fact]
        public void TestEchoCommand()
        {
            string output = _executeService.ExecuteProcess("echo", "hi");
            output.Should().Be("hi");
        }

        [Fact]
        public void TestEchoMultipleArgs()
        {
            string output = _executeService.ExecuteProcess("echo", "hi", "there");
            output.Should().Be("hi there");
        }

        [Fact]
        public void TestAccessFileSystem()
        {
            _executeService.WorkingDirectory = _fileSystem.RootDirectory;
            string output = _executeService.ExecuteProcess("mkdir", "MyFolder");
            output.Should().Be("");
            Directory.Exists(_fileSystem.RootDirectory + "MyFolder").Should().BeTrue();
        }

        [Fact]
        public void TestNonexistantProgram()
        {
            Action action = () => _executeService.ExecuteProcess("jhgetrhb", "bad_argument");
            action.Should().Throw<Win32Exception>(); // exception says "File not found"
        }
    }
}
