using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockService.Migrations
{
    public partial class addmigrationsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Provider_ProviderId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Stocks_StockId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Employee_EmployeeId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Companies_CompanyId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageLocation_Stocks_StockId",
                table: "StorageLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Upd_Provider_ProviderId",
                table: "Upd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_StockId",
                table: "Employees",
                newName: "IX_Employees_StockId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 11, 48, 16, 974, DateTimeKind.Utc).AddTicks(3930),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(2741));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 11, 48, 16, 974, DateTimeKind.Utc).AddTicks(6863),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(5722));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 11, 48, 16, 974, DateTimeKind.Utc).AddTicks(1058),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 282, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Login",
                table: "Employees",
                column: "Login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Provider_ProviderId",
                table: "Bill",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "ProviderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Stocks_StockId",
                table: "Employees",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Employees_EmployeeId",
                table: "Product",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductCategoryId",
                table: "Product",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "ProductCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Companies_CompanyId",
                table: "ProductCategories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageLocation_Stocks_StockId",
                table: "StorageLocation",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Upd_Provider_ProviderId",
                table: "Upd",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "ProviderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Provider_ProviderId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Stocks_StockId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Employees_EmployeeId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Companies_CompanyId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageLocation_Stocks_StockId",
                table: "StorageLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Upd_Provider_ProviderId",
                table: "Upd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Login",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_StockId",
                table: "Employee",
                newName: "IX_Employee_StockId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(2741),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 11, 48, 16, 974, DateTimeKind.Utc).AddTicks(3930));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(5722),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 11, 48, 16, 974, DateTimeKind.Utc).AddTicks(6863));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 282, DateTimeKind.Utc).AddTicks(9720),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 11, 48, 16, 974, DateTimeKind.Utc).AddTicks(1058));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Provider_ProviderId",
                table: "Bill",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Stocks_StockId",
                table: "Employee",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Employee_EmployeeId",
                table: "Product",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_StorageLocation_Stocks_StockId",
                table: "StorageLocation",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Upd_Provider_ProviderId",
                table: "Upd",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "ProviderId");
        }
    }
}
