using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class UserToGroupRemoveConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToGroup_Group",
                table: "UsersToGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToGroup_User",
                table: "UsersToGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToGroup_Group",
                table: "UsersToGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToGroup_User",
                table: "UsersToGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToGroup_Group",
                table: "UsersToGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToGroup_User",
                table: "UsersToGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToGroup_Group",
                table: "UsersToGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToGroup_User",
                table: "UsersToGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
