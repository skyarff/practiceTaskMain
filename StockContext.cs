using Microsoft.EntityFrameworkCore;
using StockService.Models;
using System.Numerics;

namespace StockService
{

    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        //public DbSet<Bill> Bills {  get; set; }
        //public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        //public DbSet<Storage> Storages { get; set; }
        //public DbSet<UPD> UPDs { get; set; }

        //public ApplicationContext() => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Employees)
                    .HasForeignKey(e => e.CompanyId);
            });
        }

    }
}
