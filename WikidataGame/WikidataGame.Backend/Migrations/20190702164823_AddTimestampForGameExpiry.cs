using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AddTimestampForGameExpiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MoveStartedAt",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoveStartedAt",
                table: "Games");
        }
    }
}
