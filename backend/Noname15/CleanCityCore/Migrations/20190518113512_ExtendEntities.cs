using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanCityCore.Migrations
{
    public partial class ExtendEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ResponsibleList",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Reports",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsibleId",
                table: "Reports",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ResponsibleList");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ResponsibleId",
                table: "Reports");
        }
    }
}
