using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addedIsProcessed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "RentTransactions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "RentTransactions");
        }
    }
}
