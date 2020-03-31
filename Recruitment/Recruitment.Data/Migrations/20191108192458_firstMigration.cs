using Microsoft.EntityFrameworkCore.Migrations;

namespace Recruitment.Data.Migrations
{
    public partial class firstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HrMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    JobOfferId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HrEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.JobOfferId);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobOfferId = table.Column<int>(nullable: false),
                    UserEmail = table.Column<string>(nullable: true),
                    OfferName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CommunicationEmail = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Info = table.Column<string>(nullable: true),
                    CvFile = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_Applications_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "JobOfferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobOffers",
                columns: new[] { "JobOfferId", "Description", "HrEmail", "Name" },
                values: new object[] { 100, "You need to be the king in the router magic", null, "Server Manager" });

            migrationBuilder.InsertData(
                table: "JobOffers",
                columns: new[] { "JobOfferId", "Description", "HrEmail", "Name" },
                values: new object[] { 101, "You need to be able to handle phone calls and making coffee", null, "Account Manager" });

            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "ApplicationId", "CommunicationEmail", "CvFile", "Info", "JobOfferId", "Name", "OfferName", "Phone", "State", "UserEmail" },
                values: new object[] { 100, "tomterka1@gmail.com", "nofile", "info", 100, "Andrew Dudes", "ServerManger", "132123123", "Idle", "tomterka1@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobOfferId",
                table: "Applications",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_OfferName",
                table: "Applications",
                column: "OfferName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "HrMembers");

            migrationBuilder.DropTable(
                name: "JobOffers");
        }
    }
}
