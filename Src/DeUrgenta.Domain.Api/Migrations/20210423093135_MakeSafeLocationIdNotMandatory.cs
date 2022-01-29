using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class MakeSafeLocationIdNotMandatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation1Id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation2Id",
                table: "Groups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SafeLocation2Id",
                table: "Groups",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SafeLocation1Id",
                table: "Groups",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation1Id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation2Id",
                table: "Groups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SafeLocation2Id",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SafeLocation1Id",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation1Id",
                table: "Groups",
                column: "SafeLocation1Id",
                principalTable: "GroupsSafeLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupsSafeLocations_SafeLocation2Id",
                table: "Groups",
                column: "SafeLocation2Id",
                principalTable: "GroupsSafeLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
