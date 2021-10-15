using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class CreateInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    InviteStatus = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SentOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "current_date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invites");
        }
    }
}
