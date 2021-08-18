using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class AddRelationBetweenBackpackAndBackpackItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BackpackId",
                table: "BackpackItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BackpackItem_BackpackId",
                table: "BackpackItem",
                column: "BackpackId");

            migrationBuilder.AddForeignKey(
                name: "FK_BackpackItem_Backpack",
                table: "BackpackItem",
                column: "BackpackId",
                principalTable: "Backpacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackpackItem_Backpack",
                table: "BackpackItem");

            migrationBuilder.DropIndex(
                name: "IX_BackpackItem_BackpackId",
                table: "BackpackItem");

            migrationBuilder.DropColumn(
                name: "BackpackId",
                table: "BackpackItem");
        }
    }
}
