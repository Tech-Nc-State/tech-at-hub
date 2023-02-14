﻿using Microsoft.EntityFrameworkCore;

namespace Tech_HubAPI.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }

        /// <summary>
        /// The Repository Database set.
        /// </summary>
        public DbSet<Repository> Repositories { get; set; }

        /// <summary>
        /// The Repository Database set.
        /// </summary>
        public DbSet<RepositoryPermission> RepositoryPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // The Repository Database Table
            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Owner);
            });

            modelBuilder.Entity<RepositoryPermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User);
                entity.HasOne(e => e.Repository);
            });

            // Issue
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Title);
                entity.HasKey(e => e.AuthorId);
                entity.HasKey(e => e.RepositoryId);
            });

            // Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Body);
            });

            // Label
            modelBuilder.Entity<Label>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Title);
                entity.HasOne(e => e.Color);
            });
        }
    }
}
