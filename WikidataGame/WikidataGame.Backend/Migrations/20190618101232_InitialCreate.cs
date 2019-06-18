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
                    MapWidth = table.Column<int>(nullable: false),
                    MapHeight = table.Column<int>(nullable: false),
                    AccessibleTilesCount = table.Column<int>(nullable: false),
                    NextMovePlayerId = table.Column<string>(maxLength: 36, nullable: true),
                    StepsLeftWithinMove = table.Column<int>(nullable: false),
                    MoveCount = table.Column<int>(nullable: false),
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
                    Status = table.Column<int>(nullable: false),
                    PlayerId = table.Column<string>(maxLength: 36, nullable: false),
                    TileId = table.Column<string>(maxLength: 36, nullable: false),
                    CategoryId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiniGames_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_MiniGames_Tiles_TileId",
                        column: x => x.TileId,
                        principalTable: "Tiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                          ?item wdt:P31 wd:Q5119.
                          ?item wdt:P1376 ?land.
                          ?land wdt:P31 wd:Q6256.
                          OPTIONAL { 
                            ?item rdfs:label ?answer;
                                    filter(lang(?answer) = 'en')
                              ?land rdfs:label ?question;
                                    filter(lang(?question) = 'en').
                          }
                            }
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4", "What is the name of the capital of {0}?" });

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
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                        LIMIT 4", "Which continent has {0} countries?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "9a70639b-3447-475a-905a-e866a0c98a1c", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT ?answer ?question
                        WITH {
                          SELECT DISTINCT ?state ?continent ?stateLabel ?continentLabel WHERE {
                            ?state wdt:P31/wdt:P279* wd:Q3624078;
                                 p:P463 ?memberOfStatement.
                            ?memberOfStatement a wikibase:BestRank;
                                               ps:P463 wd:Q1065.
                            MINUS { ?memberOfStatement pq:P582 ?endTime. }
                            MINUS { ?state wdt:P576|wdt:P582 ?end. }
                            ?state p:P30 ?continentStatement.
                          ?continentStatement a wikibase:BestRank;
                                              ps:P30 ?continent.
                            VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                            MINUS { ?continentStatement pq:P582 ?endTime. }
                          } ORDER BY MD5(CONCAT(STR(?state), STR(NOW())))
                        } AS %states
                        WITH {
                          SELECT ?state ?continent WHERE {
                            INCLUDE %states.
                            {
                              SELECT DISTINCT ?continent WHERE {
                                VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                              } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW())))
                              LIMIT 1
                            }
                          }
                        } AS %selectedContinent
                        WITH {
                          SELECT DISTINCT ?state ?continent WHERE {
                            INCLUDE %selectedContinent.
                          }
                          LIMIT 1
                        } AS %threeStates
                        WITH {
                          # dump continent for false answers (needed for sorting)
                          SELECT ?state ?empty WHERE {
                            INCLUDE %states.
                            FILTER NOT EXISTS { INCLUDE %selectedContinent. }
                          }
                          LIMIT 3
                        } AS %oneState
                        WHERE {
                            { INCLUDE %oneState. } UNION
                            { INCLUDE %threeStates. }

                          SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'.
                            ?state  rdfs:label ?answer.
                            ?continent rdfs:label ?question.
                          }
                        }
                        ORDER BY DESC(?question)", "Which country is a part of continent {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "46679c4f-ef97-445d-9a70-d95a5337720f", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT DISTINCT ?question ?answer
                        WITH {
                          SELECT DISTINCT (?state as ?country) WHERE {
                            ?state wdt:P31/wdt:P279* wd:Q3624078;
                                   p:P463 ?memberOfStatement.
                            ?memberOfStatement a wikibase:BestRank;
                                                 ps:P463 wd:Q1065.
                            MINUS { ?memberOfStatement pq:P582 ?endTime. }
                            MINUS { ?state wdt:P576|wdt:P582 ?end. }
                          }
                        } AS %states
                        WITH { 
                              SELECT DISTINCT ?country ?sea WHERE {
                                  BIND(wd:Q545 AS ?sea).
                                  ?sea wdt:P205 ?country.
                                }
                            } as %basins
                        WITH { 
                              SELECT DISTINCT ?country ?sea WHERE {
                                  INCLUDE %basins.
                                } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 3
                            } as %threeBasins
                        WITH {
                          SELECT DISTINCT ?country ?sea ?noSea
                            WHERE {
                              INCLUDE %states.
                              ?country wdt:P30 wd:Q46.
                              BIND(wd:Q545 as ?noSea).
                            FILTER NOT EXISTS { INCLUDE %basins.}
                          } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 1
                        } AS %oneOther
                        WHERE {
                          { INCLUDE %oneOther. } UNION
                          { INCLUDE %threeBasins. }
                          SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'. 
                            ?country rdfs:label ?answer.
                            ?noSea rdfs:label ?question. 
                          }
                        }
                        order by DESC(?question)", "Which country is no basin country of the Baltic Sea?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "4f6c477e-7025-44b4-a3b0-f3ebd8902902", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT DISTINCT ?question ?answer
                        WITH {
                          SELECT DISTINCT (?state as ?country) WHERE {
                            ?state wdt:P31/wdt:P279* wd:Q3624078;
                                   p:P463 ?memberOfStatement.
                            ?memberOfStatement a wikibase:BestRank;
                                                 ps:P463 wd:Q1065.
                            MINUS { ?memberOfStatement pq:P582 ?endTime. }
                            MINUS { ?state wdt:P576|wdt:P582 ?end. }
                          }
                        } AS %states
                        WITH { 
                              SELECT DISTINCT ?country ?sea WHERE {
                                  BIND(wd:Q1247 AS ?sea).
                                  {
                                    ?sea wdt:P205 ?country.
                                  }
                                  UNION
                                  {
                                    INCLUDE %states.
                                    ?country wdt:P361 ?region.
                                    VALUES ?region {wd:Q664609 wd:Q166131 wd:Q778 wd:Q93259 wd:Q19386 wd:Q5317255}.
                                  }
                                } ORDER BY MD5(CONCAT(STR(?country), STR(NOW())))
                            } as %basins
                        WITH { 
                            SELECT DISTINCT ?country ?sea
                            WHERE {
                              INCLUDE %basins.
                                } LIMIT 3
                            } as %threeBasins
                        WITH {
                          SELECT DISTINCT ?country ?noSea
                            WHERE {
                              INCLUDE %states.
                              ?country wdt:P361 ?region.
                              BIND(wd:Q1247 as ?noSea).
                              VALUES ?region {wd:Q12585 wd:Q653884}.
                              FILTER NOT EXISTS {?country wdt:P31 wd:Q112099.}
                              FILTER NOT EXISTS {?country wdt:P31 wd:Q13107770.}
                              FILTER NOT EXISTS {?country wdt:P361 wd:Q27611.}
                              FILTER NOT EXISTS {INCLUDE %basins.}
                          } ORDER BY MD5(CONCAT(STR(?country), STR(NOW())))
                          LIMIT 1
                        } AS %oneOther
                        WHERE {
                          { INCLUDE %oneOther. } UNION
                          { INCLUDE %threeBasins. } 
                          SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'.
                            ?country rdfs:label ?answer.
                            ?noSea rdfs:label ?question.
                          }
                        }
                        order by DESC(?noSea)", "Which country is no basin country of the {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "a6a470de-9efb-4fde-9388-6eb20f2ff1f4", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT DISTINCT ?question ?answer
                        WITH {
                          SELECT DISTINCT (?state as ?country) WHERE {
                            ?state wdt:P31/wdt:P279* wd:Q3624078;
                                   p:P463 ?memberOfStatement.
                            ?memberOfStatement a wikibase:BestRank;
                                                 ps:P463 wd:Q1065.
                            MINUS { ?memberOfStatement pq:P582 ?endTime. }
                            MINUS { ?state wdt:P576|wdt:P582 ?end. }
                          }
                          ORDER BY MD5(CONCAT(STR(?state), STR(NOW())))
                        } AS %states
                        WITH { 
                              SELECT DISTINCT ?country WHERE {
                                  BIND(wd:Q4918 AS ?sea).
                                  ?sea wdt:P205 ?country.
                                } LIMIT 3
                            } as %threeBasins
                        WITH {
                          SELECT DISTINCT ?country ?noSea
                            WHERE {
                              BIND(wd:Q4918 AS ?noSea).
                              INCLUDE %states.
                              ?country wdt:P361 ?region.
                              VALUES ?region { wd:Q7204 wd:Q984212 wd:Q27449 wd:Q263686 wd:Q50807777 wd:Q27468 wd:Q27381 }.
                              FILTER NOT EXISTS {?country wdt:P31 wd:Q51576574.}
                          } LIMIT 1
                        } AS %oneOther
                        WHERE {
                          { INCLUDE %oneOther. } UNION
                          { INCLUDE %threeBasins. }
                          SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'.
                            ?country rdfs:label ?answer.
                            ?noSea rdfs:label ?question.
                          }
                        }
                        order by DESC(?noSea)", "Which country is no basin country of the {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "29fed1d0-d306-4946-8109-63b8aaf0262e", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT DISTINCT ?answer ?question WHERE {
                        { SELECT DISTINCT ?river ?continent (avg(?length2) as ?length)
                            WHERE
                            {
                              ?river wdt:P31/wdt:P279* wd:Q355304;
                                 wdt:P2043 ?length2;
                                 wdt:P30 ?continent.
                              {
                                SELECT DISTINCT ?continent WHERE {
                                  VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15  } # ohne Ozeanien
                                } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) LIMIT 1
                               } 
                            }
                            group by ?river ?continent
                        }
                        OPTIONAL {?continent rdfs:label ?question;
                            filter(lang(?question) = 'en')
                            ?river rdfs:label ?answer ;
                            filter(lang(?answer) = 'en')
                        }
                    }
                    order by desc(?length)
                    limit 4", "What is the longest river in {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "f88a4dc0-8187-43c4-8775-593822bf4af1", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 1, @"SELECT ?question (CONCAT( ?ans, ' (', ?country, ')' ) as ?answer) WHERE {
                      { SELECT DISTINCT (?answer as ?ans) (MAX(?image) as ?question) ?country WHERE { 
                        ?landmark wdt:P31/wdt:P279* wd:Q2319498;
                                 wikibase:sitelinks ?sitelinks;
                                 wdt:P18 ?image;
                                 wdt:P17 ?cntr.
                        ?landmark wdt:P1435 ?type.
                        FILTER(?sitelinks >= 10)

                        SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'.
                            ?cntr rdfs:label ?country.
                            ?landmark rdfs:label ?answer.}
                        }
                        GROUP BY ?answer ?country
                        ORDER BY MD5(CONCAT(STR(?question), STR(NOW())))
                        LIMIT 4 
                      }
                    }", "Which famous monument is this: {0}?" });

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
                name: "IX_MiniGames_CategoryId",
                table: "MiniGames",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_GameId",
                table: "MiniGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_PlayerId",
                table: "MiniGames",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_TileId",
                table: "MiniGames",
                column: "TileId");

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
