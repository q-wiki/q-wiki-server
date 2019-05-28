using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    DeviceId = table.Column<string>(nullable: false),
                    Platform = table.Column<int>(nullable: false),
                    PushChannelUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    SparqlQuery = table.Column<string>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: false),
                    CategoryId = table.Column<string>(maxLength: 36, nullable: false),
                    MiniGameType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    NextMovePlayerId = table.Column<string>(maxLength: 36, nullable: true),
                    WinningPlayerId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_NextMovePlayerId",
                        column: x => x.NextMovePlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Users_WinningPlayerId",
                        column: x => x.WinningPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameUser",
                columns: table => new
                {
                    GameId = table.Column<string>(maxLength: 36, nullable: false),
                    UserId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUser", x => new { x.GameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GameUser_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiniGames",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    GameId = table.Column<string>(maxLength: 36, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: false),
                    AnswerOptionsString = table.Column<string>(nullable: false),
                    CorrectAnswerString = table.Column<string>(nullable: false),
                    IsWin = table.Column<bool>(nullable: false),
                    PlayerId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiniGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiniGames_Users_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tiles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    ChosenCategoryId = table.Column<string>(maxLength: 36, nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    OwnerId = table.Column<string>(maxLength: 36, nullable: true),
                    IsAccessible = table.Column<bool>(nullable: false),
                    GameId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tiles_Categories_ChosenCategoryId",
                        column: x => x.ChosenCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tiles_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "e9019ee1-0eed-492d-8aa7-feb1974fb265", "Nature" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "ddd333f7-ef45-4e13-a2ca-fb4494dce324", "Culture" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "cf3111af-8b18-4c6f-8ee6-115157d54b79", "Geography" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "1b9185c0-c46b-4abf-bf82-e464f5116c7d", "Space" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "6c22af9b-2f45-413b-995d-7ee6c61674e5", "Natural Sciences" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "f9c52d1a-9315-423d-a818-94c1769fffe5", "History" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "4941c348-b4c4-43b5-b3d4-85794c68eec4", "Celebrities" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "2a388146-e32c-4a08-a246-472eff12849a", "Entertainment" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "7f2baca7-cdf4-4e24-855b-c868d9030ba4", "Politics" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "3d6c54d3-0fda-4923-a00e-e930640430b3", "Sports" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT ?answer ?question WHERE {  
                          ? item wdt:P31 wd:Q5119.
                          ? item wdt:P1376 ? land.
                          ? land wdt : P31 wd:Q6256.
                          OPTIONAL { 
                            ?item rdfs:label ? answer;
                                    filter(lang(?answer) = 'en')
                              ? land rdfs:label? question;
                                    filter(lang(?question) = 'en').
                          }
                            }
                        ORDER BY RAND() LIMIT 4", "What is the name of the capital of {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "aca0f5f7-b000-42fb-b713-f5fe43748761", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT ?answer (COUNT(?item) AS ?question)
                        WHERE 
                        {
                          ?item wdt:P31 wd:Q6256.
                          ?item wdt:P30 ?continent.
                          ?continent wdt:P31 wd:Q5107.
                          OPTIONAL {?continent rdfs:label ?answer ;
                                    filter(lang(?answer) = 'en')
                                          }
                        }
                        GROUP BY ?continent ?answer
                        ORDER BY RAND()
                        LIMIT 4", "How many countries are on the continent {0}?" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_NextMovePlayerId",
                table: "Games",
                column: "NextMovePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinningPlayerId",
                table: "Games",
                column: "WinningPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUser_UserId",
                table: "GameUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_GameId",
                table: "MiniGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_PlayerId",
                table: "MiniGames",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CategoryId",
                table: "Questions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_ChosenCategoryId",
                table: "Tiles",
                column: "ChosenCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_GameId",
                table: "Tiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_OwnerId",
                table: "Tiles",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUser");

            migrationBuilder.DropTable(
                name: "MiniGames");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Tiles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
