using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class MovedWinnerPropToGameUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_WinningPlayerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_WinningPlayerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "WinningPlayerId",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "GameUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "GameUser");

            migrationBuilder.AddColumn<string>(
                name: "WinningPlayerId",
                table: "Games",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinningPlayerId",
                table: "Games",
                column: "WinningPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_WinningPlayerId",
                table: "Games",
                column: "WinningPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
