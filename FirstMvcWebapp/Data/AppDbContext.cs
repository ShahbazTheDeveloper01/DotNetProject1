using Microsoft.EntityFrameworkCore;
using FirstMvcWebapp.Models;

namespace FirstMvcWebapp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } 

        public DbSet<Product> Products { get; set; }
    }
}
