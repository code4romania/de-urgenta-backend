using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class ReworkGroupSafeLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation1Id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation2Id",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SafeLocation1Id",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SafeLocation2Id",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SafeLocation1Id",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SafeLocation2Id",
                table: "Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "GroupsSafeLocations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupsSafeLocations_GroupId",
                table: "GroupsSafeLocations",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsSafeLocations_Groups_GroupId",
                table: "GroupsSafeLocations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsSafeLocations_Groups_GroupId",
                table: "GroupsSafeLocations");

            migrationBuilder.DropIndex(
                name: "IX_GroupsSafeLocations_GroupId",
                table: "GroupsSafeLocations");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupsSafeLocations");

            migrationBuilder.AddColumn<Guid>(
                name: "SafeLocation1Id",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SafeLocation2Id",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SafeLocation1Id",
                table: "Groups",
                column: "SafeLocation1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SafeLocation2Id",
                table: "Groups",
                column: "SafeLocation2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation1Id",
                table: "Groups",
                column: "SafeLocation1Id",
                principalTable: "GroupsSafeLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation2Id",
                table: "Groups",
                column: "SafeLocation2Id",
                principalTable: "GroupsSafeLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
