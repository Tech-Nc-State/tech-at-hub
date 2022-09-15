using FluentAssertions;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Forms;
using Tech_HubAPI.Models.Git;
using Tech_HubAPITest.Services;
using Xunit;

namespace Tech_HubAPITest
{
    [Collection("DatabaseFileSystemCollection")]
    public class RepositoryControllerTest
    {
        private readonly ApiService _api;
        private readonly FileSystemService _fileSystem;
        private readonly TestHelpers _testHelpers;

        public RepositoryControllerTest(ApiService api, FileSystemService fileSystem, TestHelpers testHelpers)
        {
            _api = api;
            _fileSystem = fileSystem;
            _testHelpers = testHelpers;
        }

        [Fact]
        public async Task GetDirectoryListingApiTest()
        {
            var user = await _testHelpers.CreateUser("bob", "passwordyy");

            _fileSystem.ImportFolder("./SampleGitRepos/testDirectoryListing.git", "git/bob/testDirectoryListing.git");

            var form = new GetDirectoryListingForm("bob", "testDirectoryListing", "", "master");
            var content = JsonContent.Create(form, typeof(GetDirectoryListingForm));
            var resp = await _api.Client.PostAsync("/repository/getdirectorylisting", content);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var entries = await resp.Content.ReadFromJsonAsync<List<DirectoryEntry>>();
            entries.Should().NotBeNull();
            var result = entries.Select(e => e.Name).ToArray();
            result.Should().Contain("Directory");
        }

    }
}
