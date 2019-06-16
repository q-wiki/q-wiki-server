using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AdditionalTileProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "MiniGames",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TileId",
                table: "MiniGames",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_CategoryId",
                table: "MiniGames",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_TileId",
                table: "MiniGames",
                column: "TileId");

            migrationBuilder.AddForeignKey(
                name: "FK_MiniGames_Categories_CategoryId",
                table: "MiniGames",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MiniGames_Tiles_TileId",
                table: "MiniGames",
                column: "TileId",
                principalTable: "Tiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MiniGames_Categories_CategoryId",
                table: "MiniGames");

            migrationBuilder.DropForeignKey(
                name: "FK_MiniGames_Tiles_TileId",
                table: "MiniGames");

            migrationBuilder.DropIndex(
                name: "IX_MiniGames_CategoryId",
                table: "MiniGames");

            migrationBuilder.DropIndex(
                name: "IX_MiniGames_TileId",
                table: "MiniGames");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MiniGames");

            migrationBuilder.DropColumn(
                name: "TileId",
                table: "MiniGames");
        }
    }
}
