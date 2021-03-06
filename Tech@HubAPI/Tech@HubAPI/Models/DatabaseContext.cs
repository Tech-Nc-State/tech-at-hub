using Microsoft.EntityFrameworkCore;

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
                //entity.HasOne(e => e.Owner)
            });
        }
    }
}
