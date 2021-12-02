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
        public void TestEcho()
        {
            string output = Execute.ExecuteProcess("echo", "hi");
            output.Should().Be("hi");

        }

        [Fact]
        public void TestNonexistantProgram()
        {
            string output = Execute.ExecuteProcess("jhgetrhb", "bad_argument");
            output.Should().Be("The system cannot find the file specified.");
        }

    }
}
