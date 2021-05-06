using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class BackpackToUsersCorrectConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Backpack_User_Owner",
                table: "BackpacksToUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Backpack_Owner",
                table: "BackpacksToUsers",
                columns: new[] { "BackpackId", "IsOwner" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Backpack_Owner",
                table: "BackpacksToUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Backpack_User_Owner",
                table: "BackpacksToUsers",
                columns: new[] { "BackpackId", "UserId", "IsOwner" },
                unique: true);
        }
    }
}
