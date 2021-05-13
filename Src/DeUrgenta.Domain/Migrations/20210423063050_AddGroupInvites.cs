using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class AddGroupInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sub",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GroupInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitationSenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitationReceiverId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_invite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupInvite_Group",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInvite_InvitationReceiver",
                        column: x => x.InvitationReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInvite_InvitationSender",
                        column: x => x.InvitationSenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvite",
                table: "GroupInvites",
                columns: new[] { "GroupId", "InvitationReceiverId", "InvitationSenderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvite_Group",
                table: "GroupInvites",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvite_InvitationReceiver",
                table: "GroupInvites",
                column: "InvitationReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvite_InvitationSender",
                table: "GroupInvites",
                column: "InvitationSenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupInvites");

            migrationBuilder.DropColumn(
                name: "Sub",
                table: "Users");
        }
    }
}
