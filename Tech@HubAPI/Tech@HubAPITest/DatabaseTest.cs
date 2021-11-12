using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Tech_HubAPI.Models;

namespace Tech_HubAPITest
{
	public class DatabaseTest : IDisposable
	{
		public DatabaseTest()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("developersecrets.json")
				.Build();
			string connectionString = config.GetConnectionString("MySqlDatabase");
			var options = new DbContextOptionsBuilder<DatabaseContext>()
				.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
				.Options;

			DbContext = new DatabaseContext(options);
			DbContext.Database.EnsureCreated();
            
		}

		protected DatabaseContext DbContext { get; private set;}

		public void Dispose()
		{
			DbContext.Database.EnsureDeleted();
		}
	}
}
