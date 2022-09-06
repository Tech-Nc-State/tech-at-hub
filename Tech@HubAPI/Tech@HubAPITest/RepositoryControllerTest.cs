using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
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
        private readonly ApiHelperService _apiHelper;

        public RepositoryControllerTest(ApiService api, FileSystemService fileSystem, ApiHelperService testHelpers)
        {
            _api = api;
            _fileSystem = fileSystem;
            _apiHelper = testHelpers;
        }

        [Fact]
        public async Task GetDirectoryListingApiTest()
        {
            var user = await _apiHelper.CreateUser("bob", "passwordyy");
            var token = await _apiHelper.GetJwtResponseToken(await _apiHelper.Login("bob", "passwordyy"));
            await _apiHelper.CreateRepository(token, new CreateRepositoryForm
            {
                Name = "testDirectoryListing",
                IsPublic = true,
            });
            _fileSystem.ImportFolder("./SampleGitRepos/testDirectoryListing.git", "git/bob/testDirectoryListing.git");

            var resp = await _api.Client.GetAsync("/repository/bob/testDirectoryListing/listing?branch=master&path=");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var entries = await resp.Content.ReadFromJsonAsync<List<DirectoryEntry>>();
            entries.Should().NotBeNull();
            var result = entries.Select(e => e.Name).ToArray();
            result.Should().Contain("Directory");
        }
    }
}
