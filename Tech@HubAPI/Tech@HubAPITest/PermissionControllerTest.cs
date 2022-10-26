using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Tech_HubAPI.Models;
using Tech_HubAPITest.Services;
using Xunit;

namespace Tech_HubAPITest
{
    [Collection("DatabaseFileSystemCollection")]
    public class PermissionControllerTest
    {
        private readonly ApiService _api;
        private readonly ApiHelperService _apiHelper;

        public PermissionControllerTest(ApiService api, ApiHelperService apiHelper)
        {
            _api = api;
            _apiHelper = apiHelper;
        }

        [Fact]
        public async Task TestPermissionsUnauthenticated()
        {
            var resp = await _api.Client.PutAsync($"/permission/set", null);
            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            resp = await _api.Client.DeleteAsync($"/permission/delete");
            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreatePermissionNonexistentRepoTest()
        {
            var user = await _apiHelper.CreateUser("bob", "passwordy");
            string token = await _apiHelper.GetJwtResponseToken(await _apiHelper.Login("bob", "passwordy"));

            var perm = new RepositoryPermission(user.Id, 1, PermissionLevel.Write);

            var request = new HttpRequestMessage(HttpMethod.Post, $"/permission/add");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(perm, typeof(RepositoryPermission));
            var resp = await _api.Client.SendAsync(request);
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
