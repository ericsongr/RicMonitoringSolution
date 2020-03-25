using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addedrenttransactionidunderrentertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentTransactionId",
                table: "Renters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Renters_RentTransactionId",
                table: "Renters",
                column: "RentTransactionId");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Renter_RentTransaction",
                table: "Renters",
                column: "RentTransactionId",
                principalTable: "RentTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Renter_RentTransaction",
                table: "Renters");

            migrationBuilder.DropIndex(
                name: "IX_Renters_RentTransactionId",
                table: "Renters");

            migrationBuilder.DropColumn(
                name: "RentTransactionId",
                table: "Renters");
        }
    }
}
