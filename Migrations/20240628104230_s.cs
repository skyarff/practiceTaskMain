using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockService.Migrations
{
    public partial class s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(2741),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(2452));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(5722),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(5323));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 282, DateTimeKind.Utc).AddTicks(9720),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 151, DateTimeKind.Utc).AddTicks(9929));

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Upd",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(2452),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(2741));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 152, DateTimeKind.Utc).AddTicks(5323),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 283, DateTimeKind.Utc).AddTicks(5722));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Bill",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 28, 10, 11, 53, 151, DateTimeKind.Utc).AddTicks(9929),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 6, 28, 10, 42, 30, 282, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Companies_CompanyId",
                table: "Stocks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }
    }
}
