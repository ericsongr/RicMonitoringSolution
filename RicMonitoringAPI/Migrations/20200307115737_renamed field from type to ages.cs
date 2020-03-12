using Microsoft.EntityFrameworkCore.Migrations;

namespace RicMonitoringAPI.Migrations
{
    public partial class renamedfieldfromtypetoages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "BookedPersons",
                newName: "Ages");

            migrationBuilder.RenameIndex(
                name: "IX_BookedPersons_Type",
                table: "BookedPersons",
                newName: "IX_BookedPersons_Ages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ages",
                table: "BookedPersons",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_BookedPersons_Ages",
                table: "BookedPersons",
                newName: "IX_BookedPersons_Type");
        }
    }
}
