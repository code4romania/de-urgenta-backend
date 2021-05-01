using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class RemoveAdminFromBackpack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Backpacks_Users_AdminUserId",
                table: "Backpacks");

            migrationBuilder.DropIndex(
                name: "IX_Backpacks_AdminUserId",
                table: "Backpacks");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "Backpacks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdminUserId",
                table: "Backpacks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Backpacks_AdminUserId",
                table: "Backpacks",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Backpacks_Users_AdminUserId",
                table: "Backpacks",
                column: "AdminUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
