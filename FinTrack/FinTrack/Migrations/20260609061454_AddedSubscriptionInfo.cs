using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubscriptionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionPlan",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionInfoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriptionInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionPlan = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubscriptionInfoId",
                table: "AspNetUsers",
                column: "SubscriptionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SubscriptionInfo_SubscriptionInfoId",
                table: "AspNetUsers",
                column: "SubscriptionInfoId",
                principalTable: "SubscriptionInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SubscriptionInfo_SubscriptionInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SubscriptionInfo");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscriptionInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionInfoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlan",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
