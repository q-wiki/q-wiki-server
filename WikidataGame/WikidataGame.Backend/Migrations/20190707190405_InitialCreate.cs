using System;
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
                    PushToken = table.Column<string>(nullable: true),
                    PushRegistrationId = table.Column<string>(nullable: true)
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
                    MoveStartedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_NextMovePlayerId",
                        column: x => x.NextMovePlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameUser",
                columns: table => new
                {
                    GameId = table.Column<string>(maxLength: 36, nullable: false),
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    IsWinner = table.Column<bool>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tiles_Users_OwnerId",
                        column: x => x.OwnerId,
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
                values: new object[] { "cf3111af-8b18-4c6f-8ee6-115157d54b79", "Geography" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "1b9185c0-c46b-4abf-bf82-e464f5116c7d", "Space" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "6c22af9b-2f45-413b-995d-7ee6c61674e5", "Chemistry" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT DISTINCT ?state ?capital ?answer ?question WHERE {
                      ?state wdt:P31/wdt:P279* wd:Q3624078;
                             p:P463 ?memberOfStatement.
                      ?memberOfStatement a wikibase:BestRank;
                                           ps:P463 wd:Q1065.
                      MINUS { ?memberOfStatement pq:P582 ?endTime. }
                      MINUS { ?state wdt:P576|wdt:P582 ?end. }
  
                      ?state p:P36 ?capitalStatement.
                      ?capitalStatement a wikibase:BestRank;
                                          ps:P36 ?capital.
                      MINUS { ?capitalStatement pq:P582 ?capitalEnd. } # exclude former capitals
                      MINUS { ?capitalStatement pq:P459 ?capitalType. } # exclude lands that have more than one capital
                      MINUS { ?capitalStatement pq:P642 ?capitalType2. } # exclude lands that have more than one capital II
                      #MINUS { ?capital wdt:P576|wdt:P582 ?end2. }  
  
                      OPTIONAL { 
                        ?state rdfs:label ?answer;
                        filter(lang(?answer) = 'en').
                        ?capital rdfs:label ?question;
                        filter(lang(?question) = 'en').
                      }
                    } 
                    ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
                    LIMIT 4", "What is the name of the capital of {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "aca0f5f7-b000-42fb-b713-f5fe43748761", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"SELECT ?continent ?answer ?question WHERE {
                        { SELECT ?continent ?answer (COUNT(?item) AS ?question) WHERE {
                            ?item wdt:P31 wd:Q6256.
                            ?item wdt:P30 ?continent.
                            ?continent wdt:P31 wd:Q5107.
                            MINUS {VALUES ?continent {wd:Q51}}. # w/o Antarctica
                            OPTIONAL {?continent rdfs:label ?answer ;
                                                filter(lang(?answer) = 'en')
                                    }
                            } GROUP BY ?continent ?answer}
                        }
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
                values: new object[] { "4f6c477e-7025-44b4-a3b0-f3ebd8902902", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"# Which country is no basin country of the Caribbean Sea?
                        SELECT DISTINCT ?question ?answer
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
                        order by DESC(?noSea)", "Which country is no basin country of the Caribbean Sea?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "a6a470de-9efb-4fde-9388-6eb20f2ff1f4", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"# Which country is no basin country of the Mediterranean Sea?
                        SELECT DISTINCT ?question ?answer
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
                          SELECT DISTINCT ?country WHERE {
                            BIND(wd:Q4918 AS ?sea).
                            ?sea wdt:P205 ?country.
                          } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 3 # random three
                        } as %threeBasins

                        WITH {
                          SELECT DISTINCT ?country ?noSea WHERE {
                            BIND(wd:Q4918 AS ?noSea).
                            INCLUDE %states.
                            ?country wdt:P361 ?region.
                            VALUES ?region { wd:Q7204 wd:Q984212 wd:Q27449 wd:Q263686 wd:Q50807777 wd:Q27468 wd:Q27381 }.
                            FILTER NOT EXISTS {?country wdt:P31 wd:Q51576574.}
                          } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 1 # random one
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
                        order by DESC(?noSea)", "Which country is no basin country of the Mediterranean Sea?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "29fed1d0-d306-4946-8109-63b8aaf0262e", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 2, @"# What is the longest river in {continent}?
                        SELECT DISTINCT ?answer ?question 
                        WITH {
                          SELECT DISTINCT ?continent WHERE {
                            VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15  } # ohne Ozeanien
                          } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) LIMIT 1
                        } as %continent

                        WHERE {
                          { 
                            SELECT DISTINCT ?river ?continent (avg(?length2) as ?length) WHERE {
                              INCLUDE %continent.
                              ?river wdt:P31/wdt:P279* wd:Q355304;
                                     wdt:P2043 ?length2;
                                     wdt:P30 ?continent.
                            }
                            group by ?river ?continent
                          }
                          OPTIONAL {
                            ?continent rdfs:label ?question;
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
                values: new object[] { "bc7a22ee-4985-44c3-9388-5c7dd6b8762e", "cf3111af-8b18-4c6f-8ee6-115157d54b79", 0, @"# sort countries by number of inhabitants (ascending)
                        SELECT (?stateLabel AS ?answer) ?question
                        WITH {
                          # subquery: get 4 random countries with their average number of inhabitants
                          SELECT DISTINCT ?state ?stateLabel (ROUND(AVG(?population) / 1000) * 1000 AS ?population) {

                            {
                              # subquery: list of all countries in the world
                              SELECT DISTINCT ?state ?stateLabel ?population ?dateOfCensus WHERE {
                                ?state wdt:P31/wdt:P279* wd:Q3624078;
                                       p:P463 ?memberOfStatement;
                                       p:P1082 [
                                         ps:P1082 ?population;
                                                  pq:P585 ?dateOfCensus
                                       ].
                                ?memberOfStatement a wikibase:BestRank;
                                                     ps:P463 wd:Q1065.
                                MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                MINUS { ?state wdt:P576|wdt:P582 ?end. }
                                ?state p:P30 ?continentStatement.
                                ?continentStatement a wikibase:BestRank;
                                                      ps:P30 ?continent.
                                VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                                MINUS { ?continentStatement pq:P582 ?endTime. }
                                SERVICE wikibase:label {
                                  bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'.
                                }
                                FILTER(YEAR(?dateOfCensus) > YEAR(NOW()) - 5)
                              }
                            }
                          } GROUP BY ?state ?stateLabel
                        } AS %allStates

                        WITH {
                          SELECT DISTINCT ?state ?stateLabel ?population WHERE {
                            INCLUDE %allStates.
                          } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) LIMIT 4
                        } AS %states

                        WHERE {
                          # fill the question (hard-coded) and sort by population (= correct sort order needed for sorting game)
                          INCLUDE %states.
                          BIND('number of inhabitants' AS ?question).
                        } ORDER BY ?population", "Sort countries by {0} (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "a4a7289a-3053-4ad7-9c60-c75a18305243", "1b9185c0-c46b-4abf-bf82-e464f5116c7d", 0, @"# sort planets by average distance to sun
                        # NOTE: there are only three planets with apoapsis and periapsis in AU; 4 planets in total
                        SELECT ?answer ?question WHERE 
                        {
                          { SELECT DISTINCT ?answer ?avgDistanceToSun WHERE 
                            {
                                # fetch planets in our solar system
                                ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                ?planet p:P2243/psv:P2243 [wikibase:quantityAmount ?apoapsis; wikibase:quantityUnit ?apoapsisUnit].
                                ?planet p:P2244/psv:P2244 [wikibase:quantityAmount ?periapsis; wikibase:quantityUnit ?periapsisUnit].

                                # FILTER (?apoapsisUnit = wd:Q1811 && ?periapsisUnit = wd:Q1811)
                                BIND ((?apoapsis + ?periapsis) / 2 as ?avgDistanceToSun)
                                FILTER (?apoapsisUnit = wd:Q828224 && ?periapsisUnit = wd:Q828224)
                                SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?planet  rdfs:label ?answer.} 
                            } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4
                          }
                          BIND('average distance to sun' as ?question)
                        } ORDER BY ?avgDistanceToSun", "Sort planets by {0} (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "2ed01768-9ab6-4895-8cbf-09dbc6f957e0", "1b9185c0-c46b-4abf-bf82-e464f5116c7d", 0, @"# sort planets by radius
                        SELECT ?answer ?question WHERE {
                          {SELECT ?planet ?answer ?radius WHERE {
                            ?planet wdt:P397 wd:Q525;
                                    p:P2120 [
                                      ps:P2120 ?radius;
                                               pq:P518 wd:Q23538
                                    ].
                            SERVICE wikibase:label { 
                              bd:serviceParam wikibase:language 'en'.
                              ?planet  rdfs:label ?answer.}
                          } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4}
                          BIND ('radius' as ?question)
                        }
                        ORDER BY ?radius", "Sort planets by {0} (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "14d93797-c61c-4415-b1ed-359d180237ff", "1b9185c0-c46b-4abf-bf82-e464f5116c7d", 2, @"#Which of these moons belongs to the planet {0}?
                        SELECT ?question ?answer 
                        WITH {
                          # subquery 1: get all moons of planets of our solar system
                          SELECT ?moon ?parent ?question ?answer WHERE {
                          {
                            SELECT ?moon ?moonLabel ?parent WHERE {
                              ?moon wdt:P31/wdt:P279* wd:Q2537;
                                    wdt:P397 ?parent.
                              ?parent wdt:P361+ wd:Q544.
                              BIND (?parent as ?planet).
                              SERVICE wikibase:label { bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. }
                            }
                          }
                          FILTER(!CONTAINS(?moonLabel, '/'))
                        } ORDER BY MD5(CONCAT(STR(?moonLabel), STR(NOW()))) # order by random
                        } as %moons

                        WITH {
                          # subquery 2:
                          # get one random planet
                          # get all moons out of list 1 which belong to that planet
                          SELECT ?moon ?parent WHERE {
                            INCLUDE %moons.
                            {
                              SELECT DISTINCT ?parent WHERE {
                                {
                                  SELECT ?moon ?moonLabel ?parentLabel ?parent WHERE {
                                    ?moon wdt:P31/wdt:P279* wd:Q2537;
                                          wdt:P397 ?parent.
                                    ?parent wdt:P361+ wd:Q544.
                                    SERVICE wikibase:label { bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. }
                                  }
                                }
                                FILTER(!CONTAINS(?moonLabel, '/'))
                              } 
                              GROUP BY ?parent
                            }
                          } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) # order by random
                            LIMIT 1
                        } AS %selectedPlanet

                        WITH {
                          # subquery 3: get one moon out of list 2 (= correct answer)
                          SELECT DISTINCT ?moon ?parent WHERE {
                            INCLUDE %selectedPlanet.
                          } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) 
                          LIMIT 1
  
                        } AS %oneMoon

                        WITH {
                        # subquery 4 get three false answers (question/parent must be empty here!)
                          SELECT DISTINCT ?moon ?empty WHERE {
                            INCLUDE %moons.
                            FILTER NOT EXISTS { INCLUDE %selectedPlanet. }
                          } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) 
                          LIMIT 3
                        } AS %threeMoons

                        WITH {
                          # another subquery because of dubios server errors
                          SELECT * WHERE {

                             {INCLUDE %threeMoons } UNION {INCLUDE %oneMoon}
                          }
                        } AS %final WHERE {
                          INCLUDE %final.
  
                          SERVICE wikibase:label {
                            bd:serviceParam wikibase:language 'en'.
                            ?parent  rdfs:label ?question.
                            ?moon rdfs:label ?answer.
                          }
                        } ORDER BY DESC(?question)", "Which of these moons belongs to planet {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "5f7e813a-3cfa-4617-86d1-514b481b37a8", "6c22af9b-2f45-413b-995d-7ee6c61674e5", 2, @"# What's the chemical symbol for {element}?
                        SELECT ?question ?answer WHERE {
                          ?element wdt:P31 wd:Q11344;
                                   wdt:P1086 ?number;
                                   wdt:P246 ?answer.
                          FILTER(1 <= ?number &&
                                 ?number <= 118)
                          SERVICE wikibase:label {
                            bd:serviceParam wikibase:language 'en'.
                            ?element  rdfs:label ?question.
                          }
                        }
                        ORDER BY MD5(CONCAT(STR(?question), STR(NOW()))) # order by random
                        LIMIT 4", "What's the chemical symbol for {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "40677b0f-9d5f-46d2-ab85-a6c40afb5f87", "6c22af9b-2f45-413b-995d-7ee6c61674e5", 2, @"SELECT ?question ?answer WHERE {
                      ?element wdt:P31 wd:Q11344;
                               wdt:P1086 ?number;
                               wdt:P246 ?question.
                      FILTER(1 <= ?number &&
                             ?number <= 118)
                      SERVICE wikibase:label {
                        bd:serviceParam wikibase:language 'en'.
                        ?element  rdfs:label ?answer.
                      }
                    }
                    ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
                    LIMIT 4", "Which element has the chemical symbol {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "e8f99165-baa3-47b2-be35-c42ab2d5f0a0", "6c22af9b-2f45-413b-995d-7ee6c61674e5", 0, @"#sort chemical elements by number in period system
                        SELECT ?question ?answer WHERE {
                          BIND ('number in period system' as ?question).
                          {
                            SELECT ?item ?element ?number ?symbol WHERE {
                              ?item wdt:P31 wd:Q11344;
                                    wdt:P1086 ?number;
                                    wdt:P246 ?symbol.
                              FILTER(1 <= ?number &&
                                     ?number <= 118)
                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?element.
                              }
                            }
                            ORDER BY MD5(CONCAT(STR(?element), STR(NOW()))) # order by random
                            LIMIT 4
                          }
                          BIND (?element as ?answer).
                        } ORDER BY ASC(?number)", "Sort chemical elements by {0} (ascending)." });

            migrationBuilder.CreateIndex(
                name: "IX_Games_NextMovePlayerId",
                table: "Games",
                column: "NextMovePlayerId");

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
