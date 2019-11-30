using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addfieldsunderrentertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateEndRent",
                table: "Renters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEndRent",
                table: "Renters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MonthsUsed",
                table: "Renters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEndRent",
                table: "Renters");

            migrationBuilder.DropColumn(
                name: "IsEndRent",
                table: "Renters");

            migrationBuilder.DropColumn(
                name: "MonthsUsed",
                table: "Renters");
        }
    }
}
