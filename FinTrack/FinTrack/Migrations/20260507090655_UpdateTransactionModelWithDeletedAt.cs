using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionModelWithDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "deletedAt",
                table: "Transactions",
                newName: "DeletedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Transactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date", "DeletedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 7, 14, 36, 54, 894, DateTimeKind.Local).AddTicks(397), new DateTime(2026, 5, 7, 14, 36, 54, 894, DateTimeKind.Local).AddTicks(385), null, new DateTime(2026, 5, 7, 14, 36, 54, 894, DateTimeKind.Local).AddTicks(398) });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date", "DeletedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 7, 14, 36, 54, 894, DateTimeKind.Local).AddTicks(401), new DateTime(2026, 5, 7, 14, 36, 54, 894, DateTimeKind.Local).AddTicks(400), null, new DateTime(2026, 5, 7, 14, 36, 54, 894, DateTimeKind.Local).AddTicks(402) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Transactions",
                newName: "deletedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "deletedAt",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date", "UpdatedAt", "deletedAt" },
                values: new object[] { new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4796), new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4781), new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4796), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date", "UpdatedAt", "deletedAt" },
                values: new object[] { new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4800), new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4799), new DateTime(2026, 5, 7, 14, 25, 16, 675, DateTimeKind.Local).AddTicks(4800), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
