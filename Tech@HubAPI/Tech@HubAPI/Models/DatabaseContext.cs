﻿using Microsoft.EntityFrameworkCore;
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

		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Book>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Author)
					.WithMany(a => a.Books);
			});

			modelBuilder.Entity<Author>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasMany(a => a.Books)
					.WithOne(b => b.Author);
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
			}
		}
	}
}
