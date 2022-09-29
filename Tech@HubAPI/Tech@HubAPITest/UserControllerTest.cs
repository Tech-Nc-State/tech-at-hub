using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Tech_HubAPI.Models;
using Tech_HubAPITest.Services;
using Xunit;

namespace Tech_HubAPITest
{
    [Collection("DatabaseFileSystemCollection")]
    public class UserControllerTest
    {
        private readonly ApiService _api;
        private readonly DatabaseService _db;
        private readonly TestHelpers _th;

        public UserControllerTest(ApiService api, DatabaseService db, TestHelpers th)
        {
            _api = api;
            _db = db;
            _th = th;
        }

        [Fact]
        public async Task TestCreateUser()
        {
            var user = await _th.CreateUser("bob", "passwordyy");
            user.Should().NotBeNull();
        }

        [Fact]
        public async Task TestGetUserById()
        {
            var user = await _th.CreateUser("bob", "passwordyy");

            // get the user
            var resp = await _api.Client.GetAsync($"/user/get/{user.Id}");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            user = await resp.Content.ReadFromJsonAsync<User>();

            user.Username.Should().Be("bob");
            user.Password.Should().BeNull();
            user.Email.Should().BeNull();
            user.Salt.Should().BeNull();
            user.BirthDate.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public async Task TestLogin()
        {
            await _th.CreateUser("bob", "passwordyy");
            var resp = await _th.Login("bob", "passwordyy");

            var body = JsonSerializer.Deserialize<Dictionary<string, string>>(await resp.Content.ReadAsStringAsync());
            body.Should().ContainKey("token");
            body.Should().ContainKey("expiration");
        }

        [Fact]
        public async Task TestGetSelf()
        {
            await _th.CreateUser("bob", "passwordyy");
            var resp = await _th.Login("bob", "passwordyy");
            string token = await _th.GetJwtResponseToken(resp);

            var request = new HttpRequestMessage(HttpMethod.Get, "/user/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            resp = await _api.Client.SendAsync(request);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await resp.Content.ReadFromJsonAsync<User>();
            user.Username.Should().Be("bob");
            user.Password.Should().BeNull();
            user.Salt.Should().BeNull();
        }

        [Fact]
        public async Task TestChangePassword()
        {
            await _th.CreateUser("bob", "passwordyy");
            var resp = await _th.Login("bob", "passwordyy");
            string token = await _th.GetJwtResponseToken(resp);

            var form = new ChangePasswordForm("bob", "passwordyy", "mynewpassword", "mynewpassword");
            var request = new HttpRequestMessage(HttpMethod.Post, "/user/change");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(form, typeof(ChangePasswordForm));
            resp = await _api.Client.SendAsync(request);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            // attempt to login with the new password
            await _th.Login("bob", "mynewpassword");
        }
    }
}
