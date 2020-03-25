using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class changedBalanceAmounttobenullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceAmount",
                table: "Renters",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceAmount",
                table: "Renters",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
