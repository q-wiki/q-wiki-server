using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class ImageMinigame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MiniGames",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseInfo",
                table: "MiniGames",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MiniGames");

            migrationBuilder.DropColumn(
                name: "LicenseInfo",
                table: "MiniGames");
        }
    }
}
