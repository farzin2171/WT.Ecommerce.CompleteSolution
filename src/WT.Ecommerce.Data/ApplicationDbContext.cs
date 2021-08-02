using Microsoft.EntityFrameworkCore;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<CustomerInformation> customerInformation { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStock> OrderStocks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockOnHold> StockOnHolds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);
        }

    }
}
