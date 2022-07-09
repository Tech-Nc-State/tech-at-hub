using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tech_HubAPI.Models;
using Tech_HubAPITest.Services;
using Xunit;

namespace Tech_HubAPITest
{
    [Collection("DatabaseCollection")]
    public class UserControllerTest
    {
        private readonly ApiService _api;
        private readonly DatabaseService _db;

        public UserControllerTest(ApiService api, DatabaseService db)
        {
            _api = api;
            _db = db;
        }

        private async Task<User> CreateUser()
        {
            // create a test user in the db
            var form = new SignUpForm("Bob", "Bobby", "bob", "passwordyy", "a@b.com", "1/1/1");
            var content = JsonContent.Create(form, typeof(SignUpForm));
            var resp = await _api.Client.PostAsync("/user", content);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = _db.DbContext.Users
                .Where(u => u.Username == "bob")
                .FirstOrDefault();

            return user;
        }

        private async Task<HttpResponseMessage> Login(string password)
        {
            var credentials = new Credentials("bob", password);
            var content = JsonContent.Create(credentials, typeof(Credentials));
            var resp = await _api.Client.PostAsync("/auth/login", content);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            return resp;
        }

        private async Task<string> GetJwtResponseToken(HttpResponseMessage response)
        {
            var body = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            return body["token"];
        }

        [Fact]
        public async Task TestCreateUser()
        {
            var user = await CreateUser();
            user.Should().NotBeNull();
        }

        [Fact]
        public async Task TestGetUserById()
        {
            var user = await CreateUser();

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
            await CreateUser();
            var resp = await Login("passwordyy");

            var body = JsonSerializer.Deserialize<Dictionary<string, string>>(await resp.Content.ReadAsStringAsync());
            body.Should().ContainKey("token");
            body.Should().ContainKey("expiration");
        }

        [Fact]
        public async Task TestGetSelf()
        {
            await CreateUser();
            var resp = await Login("passwordyy");
            string token = await GetJwtResponseToken(resp);

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
            await CreateUser();
            var resp = await Login("passwordyy");
            string token = await GetJwtResponseToken(resp);

            var form = new ChangePasswordForm("bob", "passwordyy", "mynewpassword", "mynewpassword");
            var request = new HttpRequestMessage(HttpMethod.Post, "/user/change");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(form, typeof(ChangePasswordForm));
            resp = await _api.Client.SendAsync(request);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            // attempt to login with the new password
            await Login("mynewpassword");
        }
    }
}
