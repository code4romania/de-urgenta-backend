using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class ReworkUserLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.CreateTable(
                name: "UserLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", maxLength: 100, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLocations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_UserId",
                table: "UserLocations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLocations");

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", maxLength: 100, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                column: "UserId");
        }
    }
}
