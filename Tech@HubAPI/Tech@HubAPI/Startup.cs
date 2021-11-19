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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Reflection.Metadata;
using Ubiety.Dns.Core;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
			services.AddSingleton(new Execute());

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tech_HubAPI", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					}
					,new string[] {}
				}});

				services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
					{
						options.SaveToken = true;
						options.RequireHttpsMetadata = false;
						options.TokenValidationParameters = new TokenValidationParameters()
						{
							ValidateIssuer = true,
							ValidateAudience = true,
							ValidAudience = Configuration["JWT: ValidAudience"],
							ValidIssuer = Configuration["JWT: ValidIssuer"],
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT: SecretKey"]))
						};
					});
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
			app.UseAuthentication();
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
				var dbContext = serviceScope.ServiceProvider.GetService<DatabaseContext>();
				dbContext.Database.EnsureCreated();
			}
		}
	}
}
