using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class AddBlogPostsAndEventsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ContentBody = table.Column<string>(type: "text", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    OrganizedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContentBody = table.Column<string>(type: "text", nullable: false),
                    OccursOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
