using Microsoft.EntityFrameworkCore;

namespace Tech_HubAPI.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// The Repository Database set.
        /// </summary>
        public DbSet<Repository> Repositories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Author)
                    .WithMany(a => a.Books);
                entity.HasIndex(a => a.Title).IsUnique();
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(a => a.Books)
                    .WithOne(b => b.Author);
            });

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
