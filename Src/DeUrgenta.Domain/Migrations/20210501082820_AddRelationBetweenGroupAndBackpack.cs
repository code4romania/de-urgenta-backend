using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class AddRelationBetweenGroupAndBackpack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Backpacks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Backpacks_GroupId",
                table: "Backpacks",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Backpacks_Groups_GroupId",
                table: "Backpacks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Backpacks_Groups_GroupId",
                table: "Backpacks");

            migrationBuilder.DropIndex(
                name: "IX_Backpacks_GroupId",
                table: "Backpacks");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Backpacks");
        }
    }
}
