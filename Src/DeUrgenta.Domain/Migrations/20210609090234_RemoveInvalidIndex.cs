using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class RemoveInvalidIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Backpack_Owner",
                table: "BackpacksToUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Backpack_Owner",
                table: "BackpacksToUsers",
                columns: new[] { "BackpackId", "IsOwner" },
                unique: true);
        }
    }
}
