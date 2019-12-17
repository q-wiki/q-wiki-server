using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class QuestionContributions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "MiniGames",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "QuestionRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRatings_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("14d93797-c61c-4415-b1ed-359d180237ff"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"),
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"),
                column: "Status",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_QuestionId",
                table: "MiniGames",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRatings_QuestionId",
                table: "QuestionRatings",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MiniGames_Questions_QuestionId",
                table: "MiniGames",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MiniGames_Questions_QuestionId",
                table: "MiniGames");

            migrationBuilder.DropTable(
                name: "QuestionRatings");

            migrationBuilder.DropIndex(
                name: "IX_MiniGames_QuestionId",
                table: "MiniGames");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "MiniGames");
        }
    }
}
