using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DeUrgenta.Domain.Migrations
{
    public partial class BackpackRelated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "BackpackId",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BackpackInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    BackpackId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitationSenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitationReceiverId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "BackpacksToUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsOwner = table.Column<bool>(type: "boolean", nullable: false),
                    BackpackId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackpackToUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackpackToUser_Backpack",
                        column: x => x.BackpackId,
                        principalTable: "Backpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackpackToUser_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BackpackId",
                table: "Groups",
                column: "BackpackId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Backpack_User_Owner",
                table: "BackpacksToUsers",
                columns: new[] { "BackpackId", "UserId", "IsOwner" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackpackToUser",
                table: "BackpacksToUsers",
                columns: new[] { "UserId", "BackpackId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackpackToUser_Backpack",
                table: "BackpacksToUsers",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackToUser_User",
                table: "BackpacksToUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Backpacks_BackpackId",
                table: "Groups",
                column: "BackpackId",
                principalTable: "Backpacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Backpacks_BackpackId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "BackpackInvites");

            migrationBuilder.DropTable(
                name: "BackpacksToUsers");

            migrationBuilder.DropIndex(
                name: "IX_Groups_BackpackId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "BackpackId",
                table: "Groups");

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
    }
}
