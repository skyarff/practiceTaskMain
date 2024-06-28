﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StockService;

#nullable disable

namespace StockService.Migrations
{
    [DbContext(typeof(StockContext))]
    [Migration("20240628104230_s")]
    partial class s
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StockService.Models.Bill", b =>
                {
                    b.Property<int>("BillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BillId"));

                    b.Property<string>("BillNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BillPdf")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("BillTotal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValue(0m);

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2024, 6, 28, 10, 42, 30, 282, DateTimeKind.Utc).AddTicks(9720));

                    b.Property<int?>("ProviderId")
                        .HasColumnType("integer");

                    b.HasKey("BillId");

                    b.HasIndex("ProviderId");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("StockService.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("INN")
                        .HasColumnType("text");

                    b.Property<string>("Logo")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("StockService.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Junior");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<int?>("StockId")
                        .HasColumnType("integer");

                    b.HasKey("EmployeeId");

                    b.HasIndex("StockId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("StockService.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<int?>("BillId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(5722));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<string>("FactoryNumber")
                        .HasColumnType("text");

                    b.Property<string>("InnerArticle")
                        .HasColumnType("text");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValue(0m);

                    b.Property<int?>("ProductCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("ProductionArticle")
                        .HasColumnType("text");

                    b.Property<int>("StorageLocationId")
                        .HasColumnType("integer");

                    b.Property<int?>("UpdId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId");

                    b.HasIndex("BillId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ProductCategoryId");

                    b.HasIndex("StorageLocationId")
                        .IsUnique();

                    b.HasIndex("UpdId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("StockService.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductCategoryId"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProductCategoryId");

                    b.HasIndex("CompanyId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("StockService.Models.Provider", b =>
                {
                    b.Property<int>("ProviderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProviderId"));

                    b.Property<string>("BIK")
                        .HasColumnType("text");

                    b.Property<string>("Bank")
                        .HasColumnType("text");

                    b.Property<string>("CheckingAccount")
                        .HasColumnType("text");

                    b.Property<string>("CorrespondentAccount")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("INN")
                        .HasColumnType("text");

                    b.Property<string>("LegalAdress")
                        .HasColumnType("text");

                    b.Property<string>("ManagerFullname")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProviderId");

                    b.ToTable("Provider");
                });

            modelBuilder.Entity("StockService.Models.Stock", b =>
                {
                    b.Property<int>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StockId"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("StockId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockService.Models.StorageLocation", b =>
                {
                    b.Property<int>("StorageLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StorageLocationId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("LocationPhoto")
                        .HasColumnType("text");

                    b.Property<int>("RackCode")
                        .HasColumnType("integer");

                    b.Property<int>("ShelfCode")
                        .HasColumnType("integer");

                    b.Property<int?>("StockId")
                        .HasColumnType("integer");

                    b.HasKey("StorageLocationId");

                    b.HasIndex("StockId");

                    b.ToTable("StorageLocation");
                });

            modelBuilder.Entity("StockService.Models.Upd", b =>
                {
                    b.Property<int>("UpdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UpdId"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(2741));

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ProviderId")
                        .HasColumnType("integer");

                    b.Property<string>("ScanPdf")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UpdId");

                    b.HasIndex("ProviderId");

                    b.ToTable("Upd");
                });

            modelBuilder.Entity("StockService.Models.Bill", b =>
                {
                    b.HasOne("StockService.Models.Provider", "Provider")
                        .WithMany("Bills")
                        .HasForeignKey("ProviderId");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("StockService.Models.Employee", b =>
                {
                    b.HasOne("StockService.Models.Stock", "Stock")
                        .WithMany("Employees")
                        .HasForeignKey("StockId");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockService.Models.Product", b =>
                {
                    b.HasOne("StockService.Models.Bill", "Bill")
                        .WithMany("Products")
                        .HasForeignKey("BillId");

                    b.HasOne("StockService.Models.Employee", "Employee")
                        .WithMany("Products")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockService.Models.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryId");

                    b.HasOne("StockService.Models.StorageLocation", "StorageLocation")
                        .WithOne("Product")
                        .HasForeignKey("StockService.Models.Product", "StorageLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockService.Models.Upd", "Upd")
                        .WithMany("Products")
                        .HasForeignKey("UpdId");

                    b.Navigation("Bill");

                    b.Navigation("Employee");

                    b.Navigation("ProductCategory");

                    b.Navigation("StorageLocation");

                    b.Navigation("Upd");
                });

            modelBuilder.Entity("StockService.Models.ProductCategory", b =>
                {
                    b.HasOne("StockService.Models.Company", "Company")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockService.Models.Stock", b =>
                {
                    b.HasOne("StockService.Models.Company", "Company")
                        .WithMany("Stocks")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockService.Models.StorageLocation", b =>
                {
                    b.HasOne("StockService.Models.Stock", "Stock")
                        .WithMany("StorageLocations")
                        .HasForeignKey("StockId");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockService.Models.Upd", b =>
                {
                    b.HasOne("StockService.Models.Provider", "Provider")
                        .WithMany("Upds")
                        .HasForeignKey("ProviderId");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("StockService.Models.Bill", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("StockService.Models.Company", b =>
                {
                    b.Navigation("ProductCategories");

                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("StockService.Models.Employee", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("StockService.Models.ProductCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("StockService.Models.Provider", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("Upds");
                });

            modelBuilder.Entity("StockService.Models.Stock", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("StorageLocations");
                });

            modelBuilder.Entity("StockService.Models.StorageLocation", b =>
                {
                    b.Navigation("Product");
                });

            modelBuilder.Entity("StockService.Models.Upd", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
