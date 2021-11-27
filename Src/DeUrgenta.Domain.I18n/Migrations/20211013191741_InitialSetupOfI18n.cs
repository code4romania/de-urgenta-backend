using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.I18n.Migrations
{
    public partial class InitialSetupOfI18n : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "i18n");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "i18n",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Culture = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StringResources",
                schema: "i18n",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StringResources_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "i18n",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "i18n",
                table: "Languages",
                columns: new[] { "Id", "Culture", "Name" },
                values: new object[,]
                {
                    { new Guid("a2f64834-36d3-4a94-84f2-33aac1a61ae7"), "en-US", "English" },
                    { new Guid("d0cb5c04-cb67-48c5-9252-6f6361a30a27"), "ro-RO", "Romanian" },
                    { new Guid("5959e6ce-8745-40bc-9806-ca3cd6731c6d"), "hu-HU", "Hungarian" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Culture",
                schema: "i18n",
                table: "Languages",
                column: "Culture",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_LanguageId_Key",
                schema: "i18n",
                table: "StringResources",
                columns: new[] { "LanguageId", "Key" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StringResources",
                schema: "i18n");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "i18n");
        }
    }
}
