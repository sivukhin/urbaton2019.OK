using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanCityCore.Migrations
{
    public partial class FixForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Doubler_Responsible",
                table: "ResponsibleDoublerSql");

            migrationBuilder.AddColumn<Guid>(
                name: "OriginalId",
                table: "ResponsibleDoublerSql",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleDoublerSql_OriginalId",
                table: "ResponsibleDoublerSql",
                column: "OriginalId");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Doubler_Responsible",
                table: "ResponsibleDoublerSql",
                column: "OriginalId",
                principalTable: "ResponsibleSql",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Doubler_Responsible",
                table: "ResponsibleDoublerSql");

            migrationBuilder.DropIndex(
                name: "IX_ResponsibleDoublerSql_OriginalId",
                table: "ResponsibleDoublerSql");

            migrationBuilder.DropColumn(
                name: "OriginalId",
                table: "ResponsibleDoublerSql");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Doubler_Responsible",
                table: "ResponsibleDoublerSql",
                column: "Id",
                principalTable: "ResponsibleSql",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
