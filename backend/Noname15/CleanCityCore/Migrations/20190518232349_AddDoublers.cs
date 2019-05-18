using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanCityCore.Migrations
{
    public partial class AddDoublers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponsibleList",
                table: "ResponsibleList");

            migrationBuilder.RenameTable(
                name: "ResponsibleList",
                newName: "ResponsibleSql");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsibleId",
                table: "Emails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponsibleSql",
                table: "ResponsibleSql",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ResponsibleDoublerSql",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsibleDoublerSql", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_Doubler_Responsible",
                        column: x => x.Id,
                        principalTable: "ResponsibleSql",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponsibleDoublerSql");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponsibleSql",
                table: "ResponsibleSql");

            migrationBuilder.DropColumn(
                name: "ResponsibleId",
                table: "Emails");

            migrationBuilder.RenameTable(
                name: "ResponsibleSql",
                newName: "ResponsibleList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponsibleList",
                table: "ResponsibleList",
                column: "Id");
        }
    }
}
