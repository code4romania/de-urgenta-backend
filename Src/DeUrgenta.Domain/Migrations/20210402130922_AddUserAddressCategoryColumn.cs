using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.Migrations
{
    public partial class AddUserAddressCategoryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "UserAddresses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "UserAddresses");
        }
    }
}
