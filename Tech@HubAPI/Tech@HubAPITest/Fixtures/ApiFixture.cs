using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tech_HubAPITest.Fixtures
{
    public class ApiFixture : IDisposable
    {
        public ApiFixture()
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
