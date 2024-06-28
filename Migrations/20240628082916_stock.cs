using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockService.Migrations
{
    public partial class stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Stock_StockId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Companies_CompanyId",
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageLocation_Stock_StockId",
                table: "StorageLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stock",
                table: "Stock");

            migrationBuilder.RenameTable(
                name: "Stock",
                newName: "Stocks");

            migrationBuilder.RenameIndex(
                name: "IX_Stock_CompanyId",
                table: "Stocks",
                newName: "IX_Stocks_CompanyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(6881),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 7, 40, 5, 217, DateTimeKind.Utc).AddTicks(4199));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(9703),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 7, 40, 5, 217, DateTimeKind.Utc).AddTicks(6868));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(4411),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 7, 40, 5, 217, DateTimeKind.Utc).AddTicks(1908));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Stocks_StockId",
                table: "Employee",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageLocation_Stocks_StockId",
                table: "StorageLocation",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Stocks_StockId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageLocation_Stocks_StockId",
                table: "StorageLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.RenameTable(
                name: "Stocks",
                newName: "Stock");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_CompanyId",
                table: "Stock",
                newName: "IX_Stock_CompanyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 7, 40, 5, 217, DateTimeKind.Utc).AddTicks(4199),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(6881));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 7, 40, 5, 217, DateTimeKind.Utc).AddTicks(6868),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 7, 40, 5, 217, DateTimeKind.Utc).AddTicks(1908),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 8, 29, 16, 562, DateTimeKind.Utc).AddTicks(4411));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stock",
                table: "Stock",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Stock_StockId",
                table: "Employee",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Companies_CompanyId",
                table: "Stock",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageLocation_Stock_StockId",
                table: "StorageLocation",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "StockId");
        }
    }
}
