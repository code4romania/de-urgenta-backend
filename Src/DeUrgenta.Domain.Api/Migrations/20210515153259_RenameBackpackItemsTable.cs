using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class RenameBackpackItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BackpackItem",
                newName: "BackpackItems");

            migrationBuilder.RenameIndex(
                name: "IX_BackpackItem_BackpackId",
                table: "BackpackItems",
                newName: "IX_BackpackItems_BackpackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BackpackItems",
                newName: "BackpackItem");

            migrationBuilder.RenameIndex(
                name: "IX_BackpackItems_BackpackId",
                table: "BackpackItem",
                newName: "IX_BackpackItem_BackpackId");
        }
    }
}
