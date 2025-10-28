using GenericRepoWebApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace GenericRepoWebApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                 .HasOne(o => o.Prodcut)
                 .WithMany(p => p.Orders)
                 .HasForeignKey(o => o.ProductId);
        }
    }
}
