using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class ReworkBackpackItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackpackCategory_BackpackItem",
                table: "BackpackItem");

            migrationBuilder.DropTable(
                name: "BackpackCategories");

            migrationBuilder.DropIndex(
                name: "IX_BackpackItem_BackpackCategoryId",
                table: "BackpackItem");

            migrationBuilder.DropColumn(
                name: "BackpackCategoryId",
                table: "BackpackItem");

            migrationBuilder.AddColumn<int>(
                name: "BackpackCategory",
                table: "BackpackItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackpackCategory",
                table: "BackpackItem");

            migrationBuilder.AddColumn<Guid>(
                name: "BackpackCategoryId",
                table: "BackpackItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BackpackCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    BackpackId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackpackCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backpack_BackpackCategory",
                        column: x => x.BackpackId,
                        principalTable: "Backpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackpackItem_BackpackCategoryId",
                table: "BackpackItem",
                column: "BackpackCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackCategories_BackpackId",
                table: "BackpackCategories",
                column: "BackpackId");

            migrationBuilder.AddForeignKey(
                name: "FK_BackpackCategory_BackpackItem",
                table: "BackpackItem",
                column: "BackpackCategoryId",
                principalTable: "BackpackCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
