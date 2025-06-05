using Microsoft.EntityFrameworkCore;
using NotifyAPI.Models;

namespace NotifyAPI.Data
{
    public class AppDbContext : DbContext
    {
      
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            
        }
    }
}
