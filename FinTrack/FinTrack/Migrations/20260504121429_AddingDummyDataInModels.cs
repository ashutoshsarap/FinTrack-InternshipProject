using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddingDummyDataInModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Salary" },
                    { 2, "Groceries" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Date", "Description", "PaymentMode", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 100.00m, 1, new DateTime(2026, 5, 4, 17, 44, 28, 930, DateTimeKind.Local).AddTicks(8252), new DateTime(2026, 5, 4, 17, 44, 28, 930, DateTimeKind.Local).AddTicks(8238), "Salary", 3, 0, new DateTime(2026, 5, 4, 17, 44, 28, 930, DateTimeKind.Local).AddTicks(8254) },
                    { 2, 50.00m, 2, new DateTime(2026, 5, 4, 17, 44, 28, 930, DateTimeKind.Local).AddTicks(8259), new DateTime(2026, 5, 4, 17, 44, 28, 930, DateTimeKind.Local).AddTicks(8257), "Bought Milk and bread", 3, 1, new DateTime(2026, 5, 4, 17, 44, 28, 930, DateTimeKind.Local).AddTicks(8260) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
