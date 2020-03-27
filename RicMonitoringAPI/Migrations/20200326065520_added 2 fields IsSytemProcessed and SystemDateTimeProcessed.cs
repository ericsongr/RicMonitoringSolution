using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class added2fieldsIsSytemProcessedandSystemDateTimeProcessed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemProcessed",
                table: "RentTransactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SystemDateTimeProcessed",
                table: "RentTransactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemProcessed",
                table: "RentTransactions");

            migrationBuilder.DropColumn(
                name: "SystemDateTimeProcessed",
                table: "RentTransactions");
        }
    }
}
