using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models;
using Xunit;
using Tech_HubAPI; // This lets me call Execute.cs (same namespace)


namespace Tech_HubAPITest
{
    public class ExecuteTest
    {
        [Fact]
        public void TestEchoFullPathPartialString()
        {
            string output = Execute.ExecuteProcess("C:\\Program Files\\Git\\usr\\bin\\echo.exe", "hi");
            output.Should().Contain("hi");

        }

        [Fact]
        public void TestEchoFullPathExactString()
        {
            string output = Execute.ExecuteProcess("C:\\Program Files\\Git\\usr\\bin\\echo.exe", "hi");
            output.Should().Be("hi");

        }

        [Fact]
        public void TestEchoCommand()
        {
            string output = Execute.ExecuteCommand("echo", "hi");
            output.Should().Be("hi");

        }

        [Fact]
        public void TestEchoCommandPartialString()
        {
            string output = Execute.ExecuteCommand("echo", "hi");
            output.Should().Contain("hi");

        }

        [Fact]
        public void TestNonexistantProgram()
        {
            string output = Execute.ExecuteProcess("jhgetrhb", "bad_argument");
            output.Should().Be("The system cannot find the file specified.");
        }

        [Fact]
        public void TestFullPathBatchFileExactString()
        {
            // The purpose of this test was to see if the process was failing because echo is an "internal command" without a stored exe.
            // The .bat file just calls "echo Hello World!". 
            string output = Execute.ExecuteProcess("C:\\Users\\neil8\\OneDrive\\Documents\\TechAtState\\tech-at-hub\\Tech@HubAPI\\Tech@HubAPITest\\testExecute.bat");
            output.Should().Be("Hello World!");
        }

        [Fact]
        public void TestFullPathBatchFilePartialString()
        {
            // The difference between this test and the above is that the above doesn't account for whitespace
            // whereas this one just ignores the whitespace. IDK where the whitespace comes from.
            string output = Execute.ExecuteProcess("C:\\Users\\neil8\\OneDrive\\Documents\\TechAtState\\tech-at-hub\\Tech@HubAPI\\Tech@HubAPITest\\testExecute.bat");
            output.Should().Contain("Hello World!");
        }


        [Fact]
        public void TestBatchFileMultiArg()
        {
            // The purpose of this test was to see if the process was failing because echo is an "internal command" without a stored exe.
            // The .bat file just calls "echo Hello World!". 
            string output = Execute.ExecuteProcess("C:\\Users\\neil8\\OneDrive\\Documents\\TechAtState\\tech-at-hub\\Tech@HubAPI\\Tech@HubAPITest\\testParam.bat",
                                                    "funny", "words");
            output.Should().Be("Your first arg was funny\r\nAll your args were: funny words");
        }



    }
}
