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
}