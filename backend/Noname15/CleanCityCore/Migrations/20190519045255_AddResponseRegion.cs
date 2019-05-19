using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanCityCore.Migrations
{
    public partial class AddResponseRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseRegion",
                table: "ResponsibleSql",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseRegion",
                table: "ResponsibleSql");
        }
    }
}
