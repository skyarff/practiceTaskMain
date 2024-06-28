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
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Provider> Providers { get; set; }
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
                entity.Property(e => e.JobTitle).HasDefaultValue("Junior");
                entity.HasIndex(e => e.StockId);
                entity.HasIndex(e => e.Login).IsUnique();

                entity.HasOne(e => e.Stock)
                    .WithMany(s => s.Employees)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(e => e.StockId);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(s => s.StockId);
                entity.HasIndex(e => e.CompanyId);

                entity.HasOne(s => s.Company)
                    .WithMany(c => c.Stocks)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(s => s.CompanyId);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(pc => pc.ProductCategoryId);
                entity.HasIndex(p => p.CompanyId);

                entity.HasOne(pc => pc.Company)
                    .WithMany(c => c.ProductCategories)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(pc => pc.CompanyId);
            });

            modelBuilder.Entity<StorageLocation>(entity =>
            {
                entity.HasKey(sl => sl.StorageLocationId);
                entity.HasIndex(sl => sl.StockId);

                entity.HasOne(sl => sl.Stock)
                    .WithMany(s => s.StorageLocations)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(sl => sl.StockId);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(b => b.BillTotal).HasDefaultValue(0);
                entity.Property(b => b.CreateDate).HasDefaultValue(DateTime.UtcNow);

                entity.HasKey(b => b.BillId);
                entity.HasIndex(b => b.ProviderId);

                entity.HasOne(b => b.Provider)
                    .WithMany(p => p.Bills)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(p => p.ProviderId);
            });

            modelBuilder.Entity<Upd>(entity =>
            {
                entity.HasKey(u => u.UpdId);

                entity.Property(u => u.CreateDate).HasDefaultValue(DateTime.UtcNow);
                entity.HasIndex(u => u.ProviderId);

                entity.HasOne(u => u.Provider)
                    .WithMany(p => p.Upds)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(u => u.ProviderId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.Price).HasDefaultValue(0);
                entity.Property(p => p.CreateDate).HasDefaultValue(DateTime.UtcNow);

                entity.HasIndex(p => p.ProductCategoryId);
                entity.HasIndex(p => p.StorageLocationId).IsUnique();
                entity.HasIndex(p => p.EmployeeId);
                entity.HasIndex(p => p.BillId);
                entity.HasIndex(p => p.UpdId);

                entity.HasOne(p => p.ProductCategory)
                    .WithMany(pc => pc.Products)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(p => p.ProductCategoryId);

                entity.HasOne(p => p.StorageLocation)
                    .WithOne(sl => sl.Product)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey<Product>(p => p.StorageLocationId);
            }); // не дописаны все связи
        }

    }
}
