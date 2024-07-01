using Microsoft.EntityFrameworkCore;
using StockService.Models;

namespace StockService
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Upd> Upds { get; set; }
        public DbSet<Provider> Providers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.CompanyId);
                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(s => s.StockId);
                entity.HasIndex(e => e.CompanyId);
                entity.HasIndex(e => e.Name).IsUnique();

                entity.HasOne(s => s.Company)
                    .WithMany(c => c.Stocks)
                    .OnDelete(DeleteBehavior.SetNull)
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

            modelBuilder.Entity<StorageLocation>(entity =>
            {
                entity.HasKey(sl => sl.StorageLocationId);
                entity.HasIndex(sl => sl.StockId);

                entity.HasOne(sl => sl.Stock)
                    .WithMany(s => s.StorageLocations)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(sl => sl.StockId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.Price).HasDefaultValue(0);


                entity.HasIndex(p => p.ProductCategoryId);
                entity.HasIndex(p => p.StorageLocationId).IsUnique();
                entity.HasIndex(p => p.EmployeeId);
                entity.HasIndex(p => p.BillId);
                entity.HasIndex(p => p.UpdId);

                entity.HasOne(p => p.ProductCategory)
                    .WithMany(pc => pc.Products)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasForeignKey(p => p.ProductCategoryId);

                entity.HasOne(p => p.StorageLocation)
                    .WithOne(sl => sl.Product)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey<Product>(p => p.StorageLocationId);

                entity.HasOne(p => p.Employee)
                    .WithMany(e => e.Products)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasForeignKey(p => p.EmployeeId);

                entity.HasOne(p => p.Bill)
                    .WithMany(b => b.Products)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasForeignKey(p => p.BillId);

                entity.HasOne(p => p.Upd)
                    .WithMany(u => u.Products)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasForeignKey(p => p.UpdId);

            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(b => b.BillTotal).HasDefaultValue(0);

                entity.HasIndex(b => b.BillNumber).IsUnique();

                entity.HasKey(b => b.BillId);
                entity.HasIndex(b => b.ProviderId);

                entity.HasOne(b => b.Provider)
                    .WithMany(p => p.Bills)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(p => p.ProviderId);
            });

            modelBuilder.Entity<Upd>(entity =>
            {
                entity.HasKey(u => u.UpdId);
                entity.HasIndex(u => u.DocumentNumber).IsUnique();
                entity.HasIndex(u => u.ProviderId);

                entity.HasOne(u => u.Provider)
                    .WithMany(p => p.Upds)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(u => u.ProviderId);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(p => p.ProviderId);
                entity.HasIndex(p => p.Name).IsUnique();
            });
        }
    }
}
