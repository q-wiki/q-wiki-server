using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class CascadingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_NextMovePlayerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Categories_ChosenCategoryId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Games_GameId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Users_OwnerId",
                table: "Tiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_NextMovePlayerId",
                table: "Games",
                column: "NextMovePlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Categories_ChosenCategoryId",
                table: "Tiles",
                column: "ChosenCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Games_GameId",
                table: "Tiles",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Users_OwnerId",
                table: "Tiles",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_NextMovePlayerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Categories_ChosenCategoryId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Games_GameId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Users_OwnerId",
                table: "Tiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_NextMovePlayerId",
                table: "Games",
                column: "NextMovePlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Categories_ChosenCategoryId",
                table: "Tiles",
                column: "ChosenCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Games_GameId",
                table: "Tiles",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Users_OwnerId",
                table: "Tiles",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
