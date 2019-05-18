using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanCityCore.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Emails_IsSent",
                table: "Emails",
                column: "IsSent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Emails_IsSent",
                table: "Emails");
        }
    }
}
