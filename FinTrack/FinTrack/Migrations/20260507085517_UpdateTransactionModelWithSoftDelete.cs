using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionModelWithSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletedAt",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date", "IsDeleted", "UpdatedAt", "deletedAt" },
                values: new object[] { new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4796), new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4781), false, new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4796), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date", "IsDeleted", "UpdatedAt", "deletedAt" },
                values: new object[] { new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4800), new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4799), false, new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4800), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "deletedAt",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 6, 22, 57, 43, 580, DateTimeKind.Local).AddTicks(4180), new DateTime(2026, 5, 6, 22, 57, 43, 580, DateTimeKind.Local).AddTicks(4161), new DateTime(2026, 5, 6, 22, 57, 43, 580, DateTimeKind.Local).AddTicks(4181) });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 6, 22, 57, 43, 580, DateTimeKind.Local).AddTicks(4184), new DateTime(2026, 5, 6, 22, 57, 43, 580, DateTimeKind.Local).AddTicks(4183), new DateTime(2026, 5, 6, 22, 57, 43, 580, DateTimeKind.Local).AddTicks(4185) });
        }
    }
}
