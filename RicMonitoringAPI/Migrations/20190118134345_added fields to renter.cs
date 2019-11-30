using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addedfieldstorenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "Renters",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "BalancePayDate",
                table: "Renters",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaidAmount",
                table: "Renters",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "Renters");

            migrationBuilder.DropColumn(
                name: "BalancePayDate",
                table: "Renters");

            migrationBuilder.DropColumn(
                name: "TotalPaidAmount",
                table: "Renters");
        }
    }
}
