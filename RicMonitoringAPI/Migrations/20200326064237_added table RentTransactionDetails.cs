using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addedtableRentTransactionDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentArrearId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentTransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_RentTransactionDetails_RentArrear_RentArrearId",
                        column: x => x.RentArrearId,
                        principalTable: "RentArrears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_RentTransactionDetails_RentTransaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "RentTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentTransactionDetails_RentArrearId",
                table: "RentTransactionDetails",
                column: "RentArrearId");

            migrationBuilder.CreateIndex(
                name: "IX_RentTransactionDetails_TransactionId",
                table: "RentTransactionDetails",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentTransactionDetails");
        }
    }
}
