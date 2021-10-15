using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class RemoveBackpackInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackpackInvites");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackpackInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    BackpackId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitationReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitationSenderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backpack_invite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackpackInvite_Backpack",
                        column: x => x.BackpackId,
                        principalTable: "Backpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackpackInvite_InvitationReceiver",
                        column: x => x.InvitationReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackpackInvite_InvitationSender",
                        column: x => x.InvitationSenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackpackInvite",
                table: "BackpackInvites",
                columns: new[] { "BackpackId", "InvitationReceiverId", "InvitationSenderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackpackInvite_Backpack",
                table: "BackpackInvites",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackInvite_InvitationReceiver",
                table: "BackpackInvites",
                column: "InvitationReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackInvite_InvitationSender",
                table: "BackpackInvites",
                column: "InvitationSenderId");
        }
    }
}
