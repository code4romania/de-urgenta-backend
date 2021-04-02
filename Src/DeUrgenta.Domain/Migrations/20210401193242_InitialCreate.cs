using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DeUrgenta.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "GroupsSafeLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSafeLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Backpacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    AdminUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backpack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backpacks_Users_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Certification",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    SafeLocation1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SafeLocation2Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_GroupsSafeLocations_SafeLocation1Id",
                        column: x => x.SafeLocation1Id,
                        principalTable: "GroupsSafeLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_GroupsSafeLocations_SafeLocation2Id",
                        column: x => x.SafeLocation2Id,
                        principalTable: "GroupsSafeLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", maxLength: 100, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "BackpackCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    BackpackId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "BackpacksToUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BackpackToUser_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToGroup_Group",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserToGroup_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BackpackItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BackpackCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackpackItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackpackCategory_BackpackItem",
                        column: x => x.BackpackCategoryId,
                        principalTable: "BackpackCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackpackCategories_BackpackId",
                table: "BackpackCategories",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackItem_BackpackCategoryId",
                table: "BackpackItem",
                column: "BackpackCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Backpacks_AdminUserId",
                table: "Backpacks",
                column: "AdminUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_UserId",
                table: "Certifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AdminId",
                table: "Groups",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SafeLocation1Id",
                table: "Groups",
                column: "SafeLocation1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SafeLocation2Id",
                table: "Groups",
                column: "SafeLocation2Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToGroup",
                table: "UsersToGroups",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserToGroup_Group",
                table: "UsersToGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToGroup_User",
                table: "UsersToGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackpackItem");

            migrationBuilder.DropTable(
                name: "BackpacksToUsers");

            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.DropTable(
                name: "UsersToGroups");

            migrationBuilder.DropTable(
                name: "BackpackCategories");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Backpacks");

            migrationBuilder.DropTable(
                name: "GroupsSafeLocations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
