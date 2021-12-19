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

namespace Tech_HubAPITest
{
    public class ExecuteTest
    {
        private readonly Execute executeService;

        public ExecuteTest()
        {
            executeService = new Execute();
        }

        [Fact]
        public void TestEchoCommand()
        {
            string output = executeService.ExecuteProcess("echo", "hi");
            output.Should().Be("hi");
        }

        [Fact]
        public void TestNonexistantProgram()
        {
            string output = executeService.ExecuteProcess("jhgetrhb", "bad_argument");
            output.Should().Be("The system cannot find the file specified.");
        }

        [Fact]
        public void TestFullPathBatchFileExactString()
        {
            // The purpose of this test was to see if the process was failing because echo is an "internal command" without a stored exe.
            // The .bat file just calls "echo Hello World!". 
            string output = executeService.ExecuteProcess(
                Directory.GetParent(
                    Environment.CurrentDirectory)
                .Parent
                .Parent
                .FullName + "\\testExecute.bat");

            output.Should().Be("Hello World!");
        }

        [Fact]
        public void TestBatchFileMultiArg()
        {
            // The purpose of this test was to see if the process was failing because echo is an "internal command" without a stored exe.
            // The .bat file just calls "echo Hello World!". 
            string output = executeService.ExecuteProcess(
                Directory.GetParent(
                    Environment.CurrentDirectory)
                .Parent
                .Parent
                .FullName + "\\testParam.bat",
                "funny",
                "words");
            output.Should().Be("Your first arg was funny\r\nAll your args were: funny words");
        }
    }
}
