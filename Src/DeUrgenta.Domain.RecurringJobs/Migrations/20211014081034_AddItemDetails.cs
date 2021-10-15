using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.RecurringJobs.Migrations
{
    public partial class AddItemDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemDetails",
                schema: "jobs",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ItemDetails_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "jobs",
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemDetails",
                schema: "jobs");
        }
    }
}
