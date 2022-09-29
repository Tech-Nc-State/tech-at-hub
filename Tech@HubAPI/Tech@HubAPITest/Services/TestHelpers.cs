using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Tech_HubAPI.Models;

namespace Tech_HubAPITest.Services;
public class TestHelpers
{
    private readonly ApiService _api;
    private readonly DatabaseService _db;

    public TestHelpers(ApiService api, DatabaseService db)
    {
        _api = api;
        _db = db;
    }

    public async Task<User> CreateUser(string username, string password)
    {
        // create a test user in the db
        var form = new SignUpForm("Bob", "Bobby", username, password, "a@b.com", "1/1/1");
        var content = JsonContent.Create(form, typeof(SignUpForm));
        var resp = await _api.Client.PostAsync("/user", content);
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = _db.DbContext.Users
            .Where(u => u.Username == username)
            .FirstOrDefault();

        return user;
    }

    public async Task<HttpResponseMessage> Login(string username, string password)
    {
        var credentials = new Credentials(username, password);
        var content = JsonContent.Create(credentials, typeof(Credentials));
        var resp = await _api.Client.PostAsync("/auth/login", content);
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        return resp;
    }

    public async Task<string> GetJwtResponseToken(HttpResponseMessage response)
    {
        var body = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
        return body["token"];
    }
}
