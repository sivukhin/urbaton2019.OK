using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanCityCore.Migrations
{
    public partial class AddMoreInfoAboutUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Reports",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "ChatId");
        }
    }
}
