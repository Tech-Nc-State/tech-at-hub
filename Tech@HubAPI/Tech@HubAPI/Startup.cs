using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tech_HubAPI.Models;
using Microsoft.EntityFrameworkCore;
using Tech_HubAPI.Services;

namespace Tech_HubAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string connectionString = Configuration.GetConnectionString("MySqlDatabase");

			services.AddDbContext<DatabaseContext>(options =>
			{
				options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
			});

			services.AddSingleton(Configuration);
			services.AddScoped<ExecuteService>();
			services.AddScoped<GitService>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tech_HubAPI", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech_HubAPI v1"));
			}

			// Uncomment when the other web-servers are configured for HTTPS
			//app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// create the database from our provided models
			if (env.IsDevelopment())
			{
				var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
				using var serviceScope = serviceScopeFactory.CreateScope();
				// TODO: We need to like actually run the database at some point lol
				//var dbContext = serviceScope.ServiceProvider.GetService<DatabaseContext>();
				//dbContext.Database.EnsureCreated();
			}
		}
	}
}
