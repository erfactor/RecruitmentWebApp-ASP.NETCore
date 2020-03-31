using Microsoft.EntityFrameworkCore.Migrations;

namespace Recruitment.Data.Migrations
{
    public partial class thirdMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admins");

            migrationBuilder.AddColumn<string>(
                name: "AdNameIdentifier",
                table: "Admins",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdNameIdentifier",
                table: "Admins");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
