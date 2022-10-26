using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tech_HubAPI.Models;

namespace Tech_HubAPITest.Services
{
    public class DatabaseService : IDisposable
    {
        public DatabaseService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MySqlDatabase");

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            DbContext = new DatabaseContext(options);
            DbContext.Database.EnsureCreated();
        }

        public DatabaseContext DbContext { get; private set; }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
        }
    }
}
