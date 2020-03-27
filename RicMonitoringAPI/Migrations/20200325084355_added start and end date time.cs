using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addedstartandenddatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "MonthlyRentBatch");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessStartDateTime",
                table: "MonthlyRentBatch",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcesssEndDateTime",
                table: "MonthlyRentBatch",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessStartDateTime",
                table: "MonthlyRentBatch");

            migrationBuilder.DropColumn(
                name: "ProcesssEndDateTime",
                table: "MonthlyRentBatch");

            migrationBuilder.AddColumn<int>(
                name: "ProcessedDate",
                table: "MonthlyRentBatch",
                nullable: false,
                defaultValue: 0);
        }
    }
}
