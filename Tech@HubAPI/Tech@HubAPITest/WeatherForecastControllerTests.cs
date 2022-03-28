using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Controllers;
using Xunit;
using Tech_HubAPITest.Fixtures;
using System.Text.Json;
using System.Net.Http.Json;

namespace Tech_HubAPITest
{
    [Collection("DatabaseCollection")]
    public class WeatherForecastControllerTests : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _api;
        private readonly DatabaseService _db;

        public WeatherForecastControllerTests(ApiFixture api, DatabaseService db)
        {
            _api = api;
            _db = db;
        }

        [Fact]
        public async Task WeatherForecastGet()
        {
            // create a test user in the db
            var form = new SignUpForm("Bob", "Bobby", "bob", "passwordyy", "a@b.com", "1/1/1");
            var content = JsonContent.Create(form, typeof(SignUpForm));
            var resp = await _api.Client.PostAsync("/user", content);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
