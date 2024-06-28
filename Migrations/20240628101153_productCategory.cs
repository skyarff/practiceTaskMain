using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockService.Migrations
{
    public partial class productCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Companies_CompanyId",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "ProductCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_CompanyId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CompanyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(2452),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(6881));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(5323),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 151, DateTimeKind.Utc).AddTicks(9929),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(4411));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductCategoryId",
                table: "Product",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Companies_CompanyId",
                table: "ProductCategories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Companies_CompanyId",
                table: "ProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CompanyId",
                table: "ProductCategory",
                newName: "IX_ProductCategory_CompanyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(6881),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(2452));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(9703),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(5323));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(4411),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 151, DateTimeKind.Utc).AddTicks(9929));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryId",
                table: "Product",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Companies_CompanyId",
                table: "ProductCategory",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }
    }
}
