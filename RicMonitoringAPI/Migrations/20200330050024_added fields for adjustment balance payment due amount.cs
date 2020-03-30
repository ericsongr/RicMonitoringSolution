using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addedfieldsforadjustmentbalancepaymentdueamount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdjustmentBalancePaymentDueAmount",
                table: "RentTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "AdjustmentNotes",
                table: "RentTransactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentBalancePaymentDueAmount",
                table: "RentTransactions");

            migrationBuilder.DropColumn(
                name: "AdjustmentNotes",
                table: "RentTransactions");
        }
    }
}
