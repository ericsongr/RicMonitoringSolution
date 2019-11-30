using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class RentTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaidDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Balance = table.Column<decimal>(nullable: true),
                    BalanceDateToBePaid = table.Column<DateTime>(nullable: true),
                    IsDepositUsed = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    RoomId = table.Column<int>(nullable: false),
                    RenterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentTransactions_Renters_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Renters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentTransactions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentTransactions_RenterId",
                table: "RentTransactions",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_RentTransactions_RoomId",
                table: "RentTransactions",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentTransactions");
        }
    }
}
