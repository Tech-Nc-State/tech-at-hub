using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tech_HubAPITest.Services;

namespace Tech_HubAPITest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("developersecrets.json", optional: true)
                .Build();
            services.AddSingleton<IConfiguration>(config);
            services.AddScoped<FileSystemService>();
            services.AddScoped<DatabaseService>();
            services.AddScoped<ApiService>();
            services.AddScoped<TestHelpers>();
        }
    }
}
