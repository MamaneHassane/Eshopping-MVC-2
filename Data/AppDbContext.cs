using Eshopping_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshopping_MVC.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ProductCopy> ProductCopies { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasOne(c => c.Cart)
            .WithOne()
            .HasForeignKey<Client>(c => c.CartId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Ajoutez d'autres configurations pour d'autres relations

        base.OnModelCreating(modelBuilder);
    }
}