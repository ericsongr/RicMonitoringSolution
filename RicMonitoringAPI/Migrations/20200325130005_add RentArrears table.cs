using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class addRentArrearstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentArrears",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RenterId = table.Column<int>(nullable: false),
                    RentTransactionId = table.Column<int>(nullable: false),
                    UnpaidAmount = table.Column<decimal>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentArrears", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_RentArrears_RentTransaction_RentTransactionId",
                        column: x => x.RentTransactionId,
                        principalTable: "RentTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_RentArrears_Renter_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Renters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentArrears_RentTransactionId",
                table: "RentArrears",
                column: "RentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_RentArrears_RenterId",
                table: "RentArrears",
                column: "RenterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentArrears");
        }
    }
}
