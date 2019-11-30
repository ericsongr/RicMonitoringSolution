using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class adjustedpaidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BalancePayDate",
                table: "Renters",
                newName: "BalancePaidDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BalancePaidDate",
                table: "Renters",
                newName: "BalancePayDate");
        }
    }
}
