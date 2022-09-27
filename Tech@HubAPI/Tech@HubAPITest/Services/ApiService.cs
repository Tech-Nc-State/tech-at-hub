using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using Tech_HubAPI.Services;

namespace Tech_HubAPITest.Services
{
    public class ApiService : IDisposable
    {
        public class CustomWebApplicationFactory : WebApplicationFactory<Tech_HubAPI.Startup>
        {
            private readonly IConfiguration _configuration;
            private readonly FileSystemService _fileSystem;

            public CustomWebApplicationFactory(IConfiguration configuration, FileSystemService fileSystem)
            {
                _configuration = configuration;
                _fileSystem = fileSystem;
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    // remove the default git service
                    var descriptor = services
                        .SingleOrDefault(d => d.ServiceType == typeof(GitService));
                    services.Remove(descriptor);

                    // create the git service that should be used by the tests
                    var executeService = new ExecuteService(_configuration);
                    executeService.WorkingDirectory = _fileSystem.RootDirectory;
                    var gitService = new GitService(executeService, _configuration);

                    services.AddSingleton(gitService);
                });
            }
        }

        public ApiService(IConfiguration configuration, FileSystemService fileSystem)
        {
            var factory = new CustomWebApplicationFactory(configuration, fileSystem);
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; private set; }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
