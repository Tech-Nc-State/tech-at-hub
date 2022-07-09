using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace Tech_HubAPITest.Services
{
    public class ApiService : IDisposable
    {
        public ApiService()
        {
            var factory = new WebApplicationFactory<Tech_HubAPI.Startup>();
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; private set; }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
