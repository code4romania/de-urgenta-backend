using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class RenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Events",
                newName: "Locality");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Locality",
                table: "Events",
                newName: "City");
        }
    }
}
