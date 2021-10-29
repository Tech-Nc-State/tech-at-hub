using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tech_HubAPI.Models
{
	public partial class DatabaseContext : DbContext
	{
		private string _connectionString;

		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{ }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
			}
		}
	}
}
