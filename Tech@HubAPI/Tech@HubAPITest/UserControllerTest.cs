using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ApiHelperService _th;

        public UserControllerTest(ApiService api, DatabaseService db, ApiHelperService th)
        {
            _api = api;
            _db = db;
            _th = th;
        }

        [Fact]
        public async Task TestCreateUser()
        {
            var user = await _th.CreateUser("bob", "Passwordyy$_");
            user.Should().NotBeNull();
        }

        [Theory]
        [InlineData("passwordyy")]
        [InlineData("Passwordyy")]
        public async Task TestCreateUserInvalidPassword(string s)
        {
            // create a test user in the db
            var form = new SignUpForm("Bob", "Bobby", "bob", s, "a@b.com", "1/1/1");
            var content = JsonContent.Create(form, typeof(SignUpForm));
            var resp = await _api.Client.PostAsync("/user", content);
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task TestGetUserById()
        {
            var user = await _th.CreateUser("bob", "Passwordyy$_");

            // get the user
            var resp = await _api.Client.GetAsync($"/user/{user.Id}");
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
            await _th.CreateUser("bob", "Passwordyy$_");
            var resp = await _th.Login("bob", "Passwordyy$_");

            var body = JsonSerializer.Deserialize<Dictionary<string, string>>(await resp.Content.ReadAsStringAsync());
            body.Should().ContainKey("token");
            body.Should().ContainKey("expiration");
        }

        [Fact]
        public async Task TestGetSelf()
        {
            await _th.CreateUser("bob", "Passwordyy$_");
            var resp = await _th.Login("bob", "Passwordyy$_");
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
            await _th.CreateUser("bob", "Passwordyy$_");
            var resp = await _th.Login("bob", "Passwordyy$_");
            string token = await _th.GetJwtResponseToken(resp);

            var form = new ChangePasswordForm("bob", "Passwordyy$_", "mynewpassword", "mynewpassword");
            var request = new HttpRequestMessage(HttpMethod.Post, "/user/password");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(form, typeof(ChangePasswordForm));
            resp = await _api.Client.SendAsync(request);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            // attempt to login with the new password
            await _th.Login("bob", "mynewpassword");
        }
    }
}
