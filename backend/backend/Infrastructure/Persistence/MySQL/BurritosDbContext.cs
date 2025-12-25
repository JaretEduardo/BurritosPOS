using Microsoft.EntityFrameworkCore;
using backend.Domain.Entities;

namespace backend.Infrastructure.Persistence.MySQL
{
    public class BurritosDbContext : DbContext
    {
        public BurritosDbContext(DbContextOptions<BurritosDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
