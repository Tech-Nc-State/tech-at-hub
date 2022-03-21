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

namespace Tech_HubAPITest
{
    public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Api>>
    {
        readonly HttpClient _client;

        //[Setup]
        public WeatherForecastControllerTests(WebApplicationFactory<Api> application)
        {
            _client = application.CreateClient();
        }

        public async Task WeatherForecastGet()
        {
            var response = await _client.GetAsync("/weatherforecast");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
