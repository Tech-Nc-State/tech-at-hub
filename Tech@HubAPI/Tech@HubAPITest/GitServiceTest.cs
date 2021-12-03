using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Services;
using Xunit;

namespace Tech_HubAPITest
{
    public class GitServiceTest
    {
        private readonly GitService gitService;

        public GitServiceTest()
        {
            gitService = new GitService(new ExecuteService());
            gitService.BaseGit = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\git\\";
        }

        [Fact]
        public void TestCreateRepository()
        {
            gitService.CreateNewRepository("MyRepository", "mkmcary");
        }
    }
}
