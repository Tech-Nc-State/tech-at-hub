using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPITest.Services;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

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
        }
    }
}
