﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    PushPlatform = table.Column<int>(nullable: false),
                    PushToken = table.Column<string>(nullable: true),
                    PushRegistrationId = table.Column<string>(nullable: true),
                    ProfileImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    FriendId = table.Column<Guid>(nullable: false),
                    RelationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.UserId, x.FriendId });
                    table.UniqueConstraint("AK_Friends_RelationId", x => x.RelationId);
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    RecipientId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRequests_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MapWidth = table.Column<int>(nullable: false),
                    MapHeight = table.Column<int>(nullable: false),
                    AccessibleTilesCount = table.Column<int>(nullable: false),
                    NextMovePlayerId = table.Column<Guid>(nullable: true),
                    StepsLeftWithinMove = table.Column<int>(nullable: false),
                    MoveCount = table.Column<int>(nullable: false),
                    MoveStartedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_AspNetUsers_NextMovePlayerId",
                        column: x => x.NextMovePlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SparqlQuery = table.Column<string>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    MiniGameType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
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
                name: "GameUser",
                columns: table => new
                {
                    GameId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
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
                        name: "FK_GameUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MapIndex = table.Column<int>(nullable: false),
                    ChosenCategoryId = table.Column<Guid>(nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: true),
                    IsAccessible = table.Column<bool>(nullable: false),
                    GameId = table.Column<Guid>(nullable: true)
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
                        name: "FK_Tiles_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "MiniGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: false),
                    AnswerOptionsString = table.Column<string>(nullable: false),
                    CorrectAnswerString = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    TileId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false)
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
                        name: "FK_MiniGames_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiniGames_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MiniGames_Tiles_TileId",
                        column: x => x.TileId,
                        principalTable: "Tiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProblemType = table.Column<int>(nullable: false),
                    MinigameId = table.Column<Guid>(nullable: true),
                    MinigameType = table.Column<int>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: true),
                    ProvidedAnswers = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_MiniGames_MinigameId",
                        column: x => x.MinigameId,
                        principalTable: "MiniGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"), "Chemistry" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), "Geography" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), "History" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("1b9185c0-c46b-4abf-bf82-e464f5116c7d"), "Space" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), "Nature" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("ddd333f7-ef45-4e13-a2ca-fb4494dce324"), "Culture" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("4871ba53-b2a6-4687-9da4-1b8e50ba793f"), "Food" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("2a388146-e32c-4a08-a246-472eff12849a"), "Entertainment" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("3d6c54d3-0fda-4923-a00e-e930640430b3"), "Sports" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"), new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"), new Guid("72b3fa13-3526-4bd5-964c-442a3f3a5d31"), 2, @"# What's the chemical symbol for {element}?
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
                            LIMIT 4", 2, "What's the chemical symbol for {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                           SELECT DISTINCT ?question ?item (?name as ?answer)

                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q127282.
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allFishes

                                WITH {
                                  SELECT DISTINCT ?item ?status ?name WHERE {
                                    {Include %allFishes}
                                    ?item wdt:P141 ?status.
                                    ?status wdt:P279 wd:Q515487.
                                  } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                                } as %endangered
                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allFishes}
                                    ?item wdt:P141 ?status.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.} 
                                    VALUES ?status {wd:Q211005}.
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WHERE {
                                  {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", 2, "Which animal is {0}" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            # This query includes: artiodactyla, primates, marsupials
                            SELECT DISTINCT ?question (?name as ?answer) ?image
                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25336;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allMarsupials

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q7380;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allPrimates

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25329;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allArtiodactyla

                            WITH{
                             SELECT *
                              WHERE{
                                {include %allMarsupials}
                                UNION
                                {include %allPrimates}
                                UNION 
                                {include %allArtiodactyla}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                                ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND('Which animal is in the image?' as ?question)
                             } ORDER BY DESC(?image)
                            ", 2, "Which animal is is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            # This query includes: rodentia, carnivora, marsupial
                            SELECT DISTINCT ?question (?name as ?answer) ?image

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q10850;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allRodentia

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25306;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25336;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allMarsupial

                            WITH{
                             SELECT *
                              WHERE{
                                 {include %allRodentia}
                                 UNION
                                {include %allCarnivora}
                                UNION
                                {include %allMarsupial}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND('Which animal is in the image?' as ?question)
                             } ORDER BY DESC(?image)
                            ", 2, "Which animal is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("50120520-4441-48c1-b387-1c923a038194"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                              # This query includes: carnivore, artiodactyla, primates
                              SELECT DISTINCT ?question (?name as ?answer) ?image
                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10850;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allRodentia

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q7380;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allPrimates

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q25329;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allArtiodactyla

                                WITH{
                                 SELECT *
                                  WHERE{
                                    {include %allRodentia}
                                    UNION
                                    {include %allPrimates}
                                    UNION 
                                    {include %allArtiodactyla}
                                  }
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  Limit 4
                                } as %allAnimals

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allAnimals}
                                  }  
                                    ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allAnimals}
                                    FILTER NOT EXISTS{Include %selectedAnimal}
                                  } 
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                   {INCLUDE %selectedAnimal} 
                                   UNION 
                                   {INCLUDE %decoyAnimals}       
                                   BIND('Which animal is in the image?' as ?question)
                                 } ORDER BY DESC(?image)
                            ", 2, "Which animal is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                                # This query includes: primates + artiodactyla + rodentia
                                SELECT DISTINCT ?question (?name as ?answer) ?image

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q7380;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allPrimates


                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q25329;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allArtiodactyla

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10850;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allRodents

                                WITH{
                                 SELECT *
                                  WHERE{
                                     {include %allPrimates}
                                     UNION
                                    {include %allRodents}
                                    UNION
                                    {include %allArtiodactyla}
                                  }
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                } as %allAnimals


                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allAnimals}
                                  }  
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allAnimals}
                                    FILTER NOT EXISTS{Include %selectedAnimal}
                                  } 
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                   {INCLUDE %selectedAnimal} 
                                   UNION 
                                   {INCLUDE %decoyAnimals}       
                                   BIND('Which animal is in the image?' as ?question)
                                 } ORDER BY DESC(?image)
                            ", 2, "Which animal is this" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q5113;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allBirds

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allBirds}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedBird

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allBirds}
                            FILTER NOT EXISTS{Include %selectedBird}
                          } 
                          LIMIT 3
                        } as %decoyBirds

                        WHERE {
                          {INCLUDE %selectedBird} UNION {INCLUDE %decoyBirds}       
                          BIND('Which animal is in the image?' as ?question)
                         } ORDER BY DESC(?image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q127282;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allFishes

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allFishes}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedFish

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allFishes}
                            FILTER NOT EXISTS{Include %selectedFish}
                          } 
                          LIMIT 3
                        } as %decoyFish

                        WHERE {
                          {
                           INCLUDE %selectedFish} UNION {INCLUDE %decoyFish}       
                           BIND('Which animal is in the image?' as ?question)
                         } ORDER BY DESC(?image)", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q10908;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allAmphibia

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAmphibia}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAmphibia

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAmphibia}
                            FILTER NOT EXISTS{Include %selectedAmphibia}
                          } 
                          LIMIT 3
                        } as %decoyAmphibia

                        WHERE {
                          {
                           INCLUDE %selectedAmphibia} UNION {INCLUDE %decoyAmphibia}       
                           BIND('Which animal is in the image?' as ?question)
                         } ORDER BY DESC(?image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q122422;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allReptiles

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allReptiles}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedReptile

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allReptiles}
                            FILTER NOT EXISTS{Include %selectedReptile}
                          } 
                          LIMIT 3
                        } as %decoyReptiles

                        WHERE {
                          {
                           INCLUDE %selectedReptile} UNION {INCLUDE %decoyReptiles}       
                           BIND('Which animal is in the image?' as ?question)
                         }
                        ORDER BY DESC(?image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        # which of these 
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25329;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allArtiodactyla

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q7380;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allPrimates


                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25306;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allCarnivora


                        WITH{
                         SELECT *
                          WHERE{
                             {include %allArtiodactyla}
                             UNION
                            {include %allPrimates}
                             UNION
                            {include %allCarnivora}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          #LIMIT 4
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND('Which animal is in the image?' as ?question)
                         }
                        ORDER BY DESC(?image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        # which of these 
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25336;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allMarsupials

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q10850;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allRodentias


                        WITH{
                         SELECT *
                          WHERE{
                             {include %allMarsupials}
                             UNION
                            {include %allRodentias}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          #LIMIT 4
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND('Which animal is in the image?' as ?question)
                         }
                        ORDER BY DESC(?Image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("bae2897b-61c9-448e-bedf-8fc069dd62b0"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                       # which of these 
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q28425;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allBats

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q10850;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allRodentias


                        WITH{
                         SELECT *
                          WHERE{
                             {include %allBats}
                             UNION
                            {include %allRodentias}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          #LIMIT 4
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND('Which animal is in the image?' as ?question)
                         }
                        ORDER BY DESC(?image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                           SELECT DISTINCT ?question ?item (?name as ?answer)

                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q122422.
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allReptiles

                                WITH {
                                  SELECT DISTINCT ?item ?status ?name WHERE {
                                    {Include %allReptiles}
                                    ?item wdt:P141 ?status.
                                    ?status wdt:P279 wd:Q515487.
                                  } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                                } as %endangered
                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allReptiles}
                                    ?item wdt:P141 ?status.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.} 
                                    VALUES ?status {wd:Q211005}.
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WHERE {
                                  {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", 2, "Which animal is {0}" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25336;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allMarsupials

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25306;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allCarnivora

                        WITH{
                         SELECT *
                          WHERE{
                            {include %allMarsupials}
                             UNION
                            {include %allCarnivora}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND('Which animal is in the image?' as ?question)
                         } ORDER BY DESC(?image)
                        ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            SELECT DISTINCT ?question (?name as ?answer) ?image

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q10850;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allRodents

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25329;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allArtiodactyla

                            WITH{
                             SELECT *
                              WHERE{
                                 {include %allRodents}
                                 UNION
                                {include %allArtiodactyla}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                                ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND('Which animal is in the image?' as ?question)
                             } ORDER BY DESC(?image)
                            ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                                # This query includes: primates + artiodactyla + rodentia
                                SELECT DISTINCT ?question (?name as ?answer) ?image

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q7380;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allPrimates


                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q25329;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allArtiodactyla

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10850;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allRodents

                                WITH{
                                 SELECT *
                                  WHERE{
                                     {include %allPrimates}
                                     UNION
                                    {include %allRodents}
                                    UNION
                                    {include %allArtiodactyla}
                                  }
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                } as %allAnimals


                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allAnimals}
                                  }  
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allAnimals}
                                    FILTER NOT EXISTS{Include %selectedAnimal}
                                  } 
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                   {INCLUDE %selectedAnimal} 
                                   UNION 
                                   {INCLUDE %decoyAnimals}       
                                   BIND('Which animal is in the image?' as ?question)
                                 } ORDER BY DESC(?image)
                            ", 2, "Which animal is in the image" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("b95607f9-8cd6-48e8-bc99-c9c305e812be"), new Guid("ddd333f7-ef45-4e13-a2ca-fb4494dce324"), new Guid("acc3d752-2880-4882-ba16-e3deb3ee9cee"), 0, @"
                                SELECT DISTINCT ?question ?answer 

                                WITH{
                                  SELECT DISTINCT ?item ?itemLabel ?inventor ?inventorLabel
                                  WHERE 
                                    { 
                                      ?inventor wdt:P31 wd:Q5; wdt:P106 wd:Q205375.
                                      ?item wdt:P61 ?inventor.
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                        ?item rdfs:label ?question.
                                        ?inventor rdfs:label ?answer.
                                                         }
                                    }
                                  ORDER BY (MD5(CONCAT(STR(?inventor), STR(NOW())))) 
                                  LIMIT 1
                                 } as %selectedInventor

                                WITH{
                                    SELECT ?inventor ?inventorLabel
                                    WHERE 
                                    { 
                                      ?inventor (wdt:P31|wdt:P106) wd:Q205375.
                                      ?item wdt:P61 ?inventor.
                                      FILTER NOT EXISTS {INCLUDE %selectedInventor}
                                    } 
                                   ORDER BY (MD5(CONCAT(STR(?inventor), STR(NOW())))) 
                                  LIMIT 3
                                } as %decoyInventors

                                WHERE{
                                  {INCLUDE %selectedInventor}
                                  UNION
                                  {INCLUDE %decoyInventors}
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                        ?item rdfs:label ?question.
                                        ?inventor rdfs:label ?answer.
                                                         }
                                }

                                Order by DESC(?question)
                                ", 2, "Who invented {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("30556891-ae34-4151-b55f-cd5a8b814235"), new Guid("ddd333f7-ef45-4e13-a2ca-fb4494dce324"), new Guid("70a291e1-4513-4e41-87c5-2746f40a4e0c"), 2, @"
                               SELECT ?question ?answer
                                WITH{
                                SELECT DISTINCT ?creator
                                  WHERE 
                                  { 
                                    ?painting wdt:P1343 wd:Q66362718;
                                            wdt:P170 ?creator.
                                  }
                                   ORDER BY (MD5(CONCAT(STR(?creator), STR(NOW()))))                    
                                   LIMIT 16
                                  } as %selectedArtists

                                WITH{
                                    SELECT DISTINCT ?creator ?creatorLabel (SAMPLE(?inception) as ?firstPaintingInception) (SAMPLE(?painting) as ?firstPainting) (GROUP_CONCAT(DISTINCT ?paintingLabel; SEPARATOR=', ') AS ?paintingNames)
                                            WHERE{
                                                  INCLUDE %selectedArtists.
                                                  {SELECT *
                                                    where{ 
                                                   ?creator wdt:P106 wd:Q1028181.
                                                   ?painting wdt:P170 ?creator.
                                                   ?painting wdt:P571 ?inception.
                                                   Filter(datatype(YEAR(?inception))!='') .  
                                                   SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                          ?creator rdfs:label ?creatorLabel}
                                                  }
                                              order by ?inception
                                            }}
                                  group by ?creator ?creatorLabel
                                  ORDER BY (MD5(CONCAT(STR(?creator), STR(NOW()))))  
                                  LIMIT 4
                                } as %firstPainting

                                    WHERE{
                                    INCLUDE %firstPainting.
                                    SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                           ?creator rdfs:label ?answer.
                                                           ?painting rdfs:label ?paintingLabel}
                                    BIND(?creatorLabel as ?answer)
                                    BIND('order the artists by the inception of their first painting' as ?question)
                                }
                                  order by ?firstPaintingInception
                                ", 2, "Sort artist by release of first painting." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"), new Guid("ddd333f7-ef45-4e13-a2ca-fb4494dce324"), new Guid("d834932d-1203-4039-9baf-68322b176bae"), 1, @"
                            SELECT ?question ?answer ?painting ?image
                            WITH{
                            SELECT DISTINCT ?creator ?painting ?image ?paintingLabel
                              WHERE 
                              { 
                                ?painting wdt:P1343 wd:Q66362718.
                                ?painting wdt:P18 ?image.
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                       ?painting rdfs:label ?paintingLabel}
                                filter(lang(?paintingLabel) = 'en').
                              }   
                               ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW())))) 
                               LIMIT 4
                            } as %allPaintings

                            WITH{
                                SELECT DISTINCT ?painting ?paintingLabel ?image
                                     WHERE{
                                          INCLUDE %allPaintings.
                                        }
                               LIMIT 1
                            } as %selectedPainting

                            WITH{
                              SELECT DISTINCT ?painting ?paintingLabel
                                     WHERE{
                                          INCLUDE %allPaintings
                                          FILTER NOT EXISTS{INCLUDE %selectedPainting}
                                        }
                               LIMIT 3
                            } as %decoyPainting

                            WHERE{
                                {INCLUDE %selectedPainting.}
                                UNION
                                {INCLUDE %decoyPainting}

                                BIND(?paintingLabel as ?answer)
                                BIND('What is the name of the painting?' as ?question)
                            }
                              order by DESC(?image)
                            ", 2, "What is the name of the painting?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("ac86282f-1fa1-48ba-a088-f4c202ea0177"), new Guid("4871ba53-b2a6-4687-9da4-1b8e50ba793f"), new Guid("f20a404e-4d02-4d45-a2bf-cd152b2cbf43"), 2, @"
                                    SELECT ?question ?answer 
                                    WITH{
                                       SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                       WHERE {
                                        ?item wdt:P31 wd:Q134768. 
                                        ?item wdt:P495 ?origin.
                                        ?origin wdt:P30 ?originContinent.
                                       }
                                        ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                    } AS %cocktails

                                    WITH{
                                       SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                       WHERE {
                                        ?item wdt:P31 wd:Q2536409. 
                                        ?item wdt:P495 ?origin.
                                        ?origin wdt:P30 ?originContinent.
                                        }
                                        ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                    } AS %ibaCocktails
     
                                    WITH{
                                      SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                      WHERE
                                      {
                                       {INCLUDE %cocktails}
                                       UNION
                                       {INCLUDE %ibaCocktails}
                                      }
                                       ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                       LIMIT 1
                                    } AS %selectedCountry

                                    WITH{
                                       SELECT ?originContinent
                                       WHERE{
                                         ?originContinent wdt:P31 wd:Q5107.
                                         INCLUDE %selectedCountry
                                       }
                                    } as %selectedContinent

                                    WITH{
                                      SELECT DISTINCT ?empty ?origin ?originLabel ?answer
                                      WHERE{
                                         ?origin wdt:P31 wd:Q3624078.
                                         FILTER NOT EXISTS { INCLUDE %selectedCountry. }
                                         {
                                           ?originContinent wdt:P31 wd:Q5107.
                                           INCLUDE %selectedContinent
                                         }
                                         ?origin wdt:P30 ?originContinent.
                                       }
                                      ORDER BY (MD5(CONCAT(STR(?origin), STR(NOW())))) 
                                      LIMIT 3
                                    } AS %decoyCountries

                                    WHERE{
                                      {INCLUDE %selectedCountry}
                                      UNION
                                      {INCLUDE %decoyCountries}
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                 ?item rdfs:label ?question.
                                                                 ?origin rdfs:label ?answer.
                                                               }
                                    }
                                    ORDER BY DESC(?question)
                                       ", 2, "Where is {0} from?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3609a5f7-c90a-4ecf-a713-0224fa8a4215"), new Guid("4871ba53-b2a6-4687-9da4-1b8e50ba793f"), new Guid("0ef8c0e0-640b-49b4-8aee-b5ab8f1a6773"), 2, @"
                             SELECT DISTINCT(?foodLabel as ?question) (?dishOfLabel as ?answer)
                             WITH{
                                  SELECT DISTINCT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?food); SEPARATOR=', ')) as ?food) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?foodLabel); SEPARATOR=', ')) as ?foodLabel)
                                    ?dishOf
                                  WHERE{
                                        ?food (wdt:P31|wdt:P279) wd:Q746549. 
                                        ?food (wdt:P495|wdt:P17) ?dishOf.
                                        MINUS { ?dishOf pq:P582 ?endTime. }
                                        MINUS { ?dishOf wdt:P576|wdt:P582 ?end. }
                                        ?food rdfs:label ?foodLabel
                                        filter langMatches(lang(?foodLabel), 'en')
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                                 }
                                 group by ?dishOf
                                 order by ?dishOf
                                 } as %foods

                                 WITH{
                                       SELECT DISTINCT ?food (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?foodLabel); SEPARATOR=', ')) as ?foodLabel)
                                       (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?dishOf); SEPARATOR=', ')) as ?dishOf) 
                                              (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?dishOfLabel); SEPARATOR=', ')) as ?dishOfLabel)
                                   WHERE{
                                        {include %foods}
                                        {?dishOf wdt:P31 wd:Q6256.}
                                        UNION
                                        {?dishOf wdt:P31 wd:Q3624078.}
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                               ?dishOf rdfs:label ?dishOfLabel}
                                      }
                                      GROUP BY ?food
                                 } as %countryfoods

                                 WITH{
                                      SELECT *
                                      WHERE{
                                       {include %countryfoods} 
                                      }
                                      ORDER BY (MD5(CONCAT(STR(?food), STR(NOW())))) 
                                      LIMIT 1
                                 } as %selectedFood

                                 WITH{
                                    SELECT ?food ?dishOfLabel
                                         WHERE{
                                              {include %countryfoods}
                                                FILTER NOT EXISTS {include %selectedFood}
                                         }
                                      ORDER BY (MD5(CONCAT(STR(?food), STR(NOW())))) 
                                      LIMIT 3
                                } as %decoyfoods

                                WHERE{
                                     {include %selectedFood}
                                       UNION
                                     {include %decoyfoods}
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                              ?dishOf rdfs:label ?dishOfLabel}
                                }
                                order by DESC(?question)
                                    ", 2, "Where is {0} from?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"), new Guid("4871ba53-b2a6-4687-9da4-1b8e50ba793f"), new Guid("984112bc-d178-4ac5-8940-3e4fd2fc3105"), 0, @"
                        #Structure is important to get only one drink of a inception year and avoid duplicates
                        SELECT DISTINCT ?question  ?answer ?year
                        WITH{
                                SELECT (Sample(GROUP_CONCAT(DISTINCT sample(?softDrink); SEPARATOR=', ')) AS ?softDrink) (Sample(GROUP_CONCAT( DISTINCT sample(?softDrinkLabel); SEPARATOR=', ')) AS ?softDrinkLabel) (year(?inception) as ?year) 
                                WHERE {
                                     ?softDrink (wd:wd31|wdt:P279)* wd:Q147538.
                                     ?softDrink wdt:P571 ?inception.
                                     Filter(?softDrink != wd:Q180289)
                                     ?softDrink rdfs:label ?softDrinkLabel
                                                               #makes sure to get only known drinks in germany by checking if item has a german label
                                     filter langMatches(lang(?softDrinkLabel), 'de')
                                     SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                             ?softDrinkLabel rdfs:label ?answer.
                                                                           }
                                     BIND(Year(?inception) as ?year)
                                }
                                group by ?inception
                                ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))                
                        } as %allSoftDrinks

                        WITH{
                          SELECT ?softDrinkLabel ?year ?empty
                          WHERE{
                            INCLUDE %allSoftDrinks.
                          }
                          ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))
                          LIMIT 4
                        } as %selectedSoftDrink

                        WHERE{
                          INCLUDE %selectedSoftDrink
                          SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                  ?softDrinkLabel rdfs:label ?answer.
                                                 }
                          BIND('Order Softdrinks by inception' as ?question)       
                        }
                        order by ?year
                        ", 2, "Sort these softdrinks by inception" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("15679f96-27bd-4e88-b367-4eb05e5f6d95"), new Guid("4871ba53-b2a6-4687-9da4-1b8e50ba793f"), new Guid("fee91818-2fb5-4845-affa-2504d4191ee1"), 1, @"
                            SELECT ?question (?dishLabel as ?answer) ?image
                            WITH{
                              SELECT DISTINCT ?dish ?dishLabel (SAMPLE(?image)as ?image)
                              WHERE{
                                 ?dish wdt:P279 wd:Q746549.
                                 ?dish wdt:P18 ?image.
                                FILTER NOT EXISTS{?dish wdt:P31 wd:Q19861951}
                                 SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                        ?dish rdfs:label ?dishLabel}
                                 filter(lang(?dishLabel) = 'en').
                              }
                              group by ?dish ?dishLabel
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                            } as %allDishes

                            WITH{
                              SELECT ?dishLabel ?image ?dish
                              WHERE { 
                                INCLUDE %allDishes
                              }
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                              LIMIT 1
                            } as %selectedDish

                            WITH{
                              SELECT ?dishLabel ?dish
                              WHERE{
                                INCLUDE %allDishes
                                FILTER NOT EXISTS{INCLUDE %selectedDish}
                              }
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                              LIMIT 3
                            } as %decoyDishes

                            WHERE{
                              {INCLUDE %selectedDish}
                              UNION
                              {INCLUDE %decoyDishes}
                              BIND('What dish is this?' as ?question)
                            }
                            ORDER BY DESC(?image)
                            ", 2, "What dish is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"), new Guid("2a388146-e32c-4a08-a246-472eff12849a"), new Guid("f8717bdd-75df-4064-9394-af163034a1c0"), 0, @"
                            # Fetch how many successful (measured by box office revenue) movies an actor played in

                            SELECT ?question ?answer ?movieCount
                            WITH{
                               # select the 500 movies with the highest box office revenue
                             SELECT DISTINCT ?movie WHERE {
                                  ?movie wdt:P31 wd:Q11424.
                                  ?movie p:P2142/psv:P2142 [wikibase:quantityAmount ?boxOffice; wikibase:quantityUnit ?currency].
                                  # only look at us dollars
                                  FILTER(?currency = wd:Q4917)
                                }
                                ORDER BY DESC(?boxOffice)
                                LIMIT 500
                            } as %topGrossingMovies

                            WITH{
                            SELECT DISTINCT ?actor ?actorLabel (COUNT(?movie) as ?movieCount) 
                            WHERE {
                              {
                               INCLUDE %topGrossingMovies
                              }
                              # get all actors that played in those movies
                              ?movie wdt:P161 ?actor.
                              #filter(lang(?actorLabel) = 'en').
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                            }
                            GROUP BY ?actor ?actorLabel
                            ORDER BY DESC(?movieCount)
                            } as %countedMovies

                            WITH{
                              #group actors by number of appearances
                              SELECT DISTINCT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?actor); SEPARATOR=', ')) as ?actors)
                              (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?actorLabel); SEPARATOR=', ')) as ?actorsLabel) ?movieCount
                              WHERE{
                                INCLUDE %countedMovies.
                                 SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                         ?actor rdfs:label ?actorLabel
                                                        }
                              }
                              GROUP BY ?movieCount
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 4
                            } as %summedMovies

                            WHERE{
                              INCLUDE %summedMovies.
                              BIND(?actorsLabel as ?answer)
                              BIND('Sort these actors by the number of movies they appeared in' as ?question)
                            }
                            order by ?movieCount

                            ", 2, "Sort these actors by the number of movies they appeared in" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("d0fcf5ac-3215-4355-9090-b6a49cf66cc3"), new Guid("2a388146-e32c-4a08-a246-472eff12849a"), new Guid("ac3e0a15-376e-4dbc-a8f8-6df4c9fe39e7"), 2, @"
                           SELECT DISTINCT ?question ?answer
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel  ?movieLabel ?awardLabel
                            WHERE{
                              ?actor wdt:P31 wd:Q5;
                                     wdt:P106 wd:Q33999;
                                     wdt:P166 ?award.
                              ?award wdt:P31+ wd:Q19020.
                              ?actor p:P166 ?statement.
                              ?statement pq:P1686 ?movie.
                              ?statement pq:P805+ ?awardCeremony.
                              ?awardCeremony wdt:P31+ wd:Q16913666.
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?movie rdfs:label ?movieLabel.
                                                     ?actor rdfs:label ?actorLabel.
                                                     ?award rdfs:label ?awardLabel}
                            }
                            ORDER BY ?actor ?actorLabel
                            } as %allWinners
                                       
                            WITH{
                              SELECT ?actor ?actorLabel ?question
                              WHERE{
                                 INCLUDE %allWinners.
                                 BIND(CONCAT('Who won the ', CONCAT(STR(?awardLabel), CONCAT(' in the movie ', CONCAT(STR(?movieLabel), '?'))))  as ?question)
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 1
                            } as %selectedActor

                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel
                              WHERE{
                                INCLUDE %allWinners
                                Filter NOT EXISTS {INCLUDE %selectedActor}
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 3
                            } as %decoyActors

                            WHERE{
                                {INCLUDE %selectedActor}
                                UNION
                                {INCLUDE %decoyActors}
                                BIND(?actorLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ", 2, "Who won the Academy Award for for the movie {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3fb180e6-99ae-466b-89e9-16ac0101daed"), new Guid("2a388146-e32c-4a08-a246-472eff12849a"), new Guid("9417595a-641b-4ce9-9219-b9c14e65621e"), 2, @"
                            SELECT ?question ?answer
                            # series that have won an emmy
                            WITH{
                                SELECT Distinct (SAMPLE(GROUP_CONCAT(DISTINCT ?seriesLabel; SEPARATOR=', ')) as ?seriesLabel) ?characterLabel
                                WHERE{
                                  ?series wdt:P31 wd:Q5398426.
                                  ?series wdt:P166 ?award.
                                  ?award wdt:P31+/wdt:P279+ wd:Q123737.
                                  ?series p:P161/pq:P453 ?character.
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                       ?character rdfs:label ?characterLabel.
                                                       ?series rdfs:label ?seriesLabel
                                                         }
                                  FILTER NOT EXISTS{ ?series p:P161/pq:P453 wd:Q18086706}
                                }
                              group by ?characterLabel
                             } as %emmySeries

                            WITH{
                              SELECT DISTINCT ?characterLabel ?seriesLabel
                              WHERE{
                                INCLUDE %emmySeries
                              }
                              ORDER BY MD5(CONCAT(STR(?seriesLabel), STR(NOW())))
                              LIMIT 1
                            } as %selectedSeries

                            WITH{
                              SELECT DISTINCT ?seriesLabel
                              WHERE{
                                INCLUDE %emmySeries.
                                FILTER NOT EXISTS {INCLUDE %selectedSeries}
                              }
                              ORDER BY MD5(CONCAT(STR(?seriesLabel), STR(NOW())))
                              LIMIT 3
                            }  as %decoySeries

                            WHERE{
                              {INCLUDE %selectedSeries}
                              UNION
                              {INCLUDE %decoySeries}
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.}
                              BIND(?characterLabel as ?question)
                              BIND(?seriesLabel as ?answer)
                            }
                            order by DESC(?question)
                            ", 2, "Where is the character {0} from?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            SELECT DISTINCT ?question (?name as ?answer) ?image

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25336;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allMarsupials

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q7380;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allPrimates

                            WITH{
                             SELECT *
                              WHERE{
                                 {include %allMarsupials}
                                 UNION
                                {include %allPrimates}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                                ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND('Which animal is in the image?' as ?question)
                             } ORDER BY DESC(?image)
                            ", 2, "Which animal is in the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                           SELECT DISTINCT ?question ?item (?name as ?answer)
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10908.
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allAmphibia

                                WITH {
                                  SELECT DISTINCT ?item ?status ?name WHERE {
                                    {Include %allAmphibia}
                                    ?item wdt:P141 ?status.
                                    ?status wdt:P279 wd:Q515487.
                                  } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                                } as %endangered
                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allAmphibia}
                                    ?item wdt:P141 ?status.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.} 
                                    VALUES ?status {wd:Q211005}.
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WHERE {
                                  {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", 2, "Which animal is {0}" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                            SELECT DISTINCT ?question ?item (?name as ?answer)

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q5113.
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                            } as %allBirds

                            WITH {
                              SELECT DISTINCT ?item ?status ?name WHERE {
                                {Include %allBirds}
                                ?item wdt:P141 ?status.
                                ?status wdt:P279 wd:Q515487.
                              } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                            } as %endangered
                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allBirds}
                                ?item wdt:P141 ?status.
                                SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.} 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                            } as %noproblem

                            WHERE {
                              {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", 2, "Which animal is {0}" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("bce51234-32b7-4629-b65a-d23beb8b43c3"), 0, @"
                            # sort animals by bite force quotient?
                            SELECT DISTINCT ?question (?name as ?answer) ?biteForce

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?item); SEPARATOR=', ')) as ?item) ?biteForce (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', '))as ?name)
                                WHERE{
                                  ?item wdt:P31 wd:Q16521;
                                        wdt:P3485 ?biteForce.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   #?reference ?referenceType wd:Q577.
                                   ?item wdt:P1843 ?name.
       
                                   filter(lang(?name) = 'en').
                                }
                              GROUP BY ?biteForce
                              ORDER BY MD5(CONCAT(STR(?biteForce), STR(NOW())))
                            } as %allTaxons
        
                            WITH{
                              SELECT ?name ?biteForce
                              WHERE{
                               {Include %allTaxons}  
                              }
                              ORDER BY MD5(CONCAT(STR(?biteForce), STR(NOW())))
                              LIMIT 4
                            } as %selectedTaxons

                            WHERE {
                                {Include %selectedTaxons}  
  
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                              } 
                              BIND('order these animals by bite force quotient' as ?question)
                            } 
                            ORDER BY ASC(?biteForce)
                            ", 2, "Order these animals by bite force quotient (Ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"), new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"), new Guid("289b9977-07e8-4540-9f06-bfcd147b5063"), 2, @"SELECT ?question ?answer WHERE {
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
                        LIMIT 4", 2, "Which element has the chemical symbol {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"), new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"), new Guid("62f3426e-6c47-43de-b9fc-db3e8d988986"), 0, @"#sort chemical elements by number in period system
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
                            } ORDER BY ASC(?number)", 2, "Sort chemical elements by {0} (ascending)." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"), new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"), new Guid("ba587fa0-9601-4d99-a56b-7e92a5ccbe13"), 2, @"# Which chemical compound has the formula {0}?
                            SELECT DISTINCT ?chemicalCompound ?answer (?chemical_formula AS ?question) ?sitelinks WHERE {
                              ?chemicalCompound wdt:P31 wd:Q11173;
                                wdt:P274 ?chemical_formula;
                                wikibase:sitelinks ?sitelinks.
                              FILTER(?sitelinks >= 50 )
                              ?chemicalCompound rdfs:label ?answer.
                              FILTER((LANG(?answer)) = 'en')
                            }
                            ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW()))))
                            LIMIT 4", 2, "Which chemical compound has the formula {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"), new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"), new Guid("f5d0100f-a7bf-4d6d-9767-b5a4463daeb5"), 0, @"
                            # Sort these chemical elements by their melting point
                            # ?question what gets used as a placeholder value
                            # ?answer is one answer option
                            # ?value is ignored
                            SELECT ?question ?answer ?value
                        
                            # select all chemical elements with melting point
                            WITH {
                              SELECT DISTINCT ?question ?answer ?value WHERE {
                                # Select chemical elements
                                ?element wdt:P31 wd:Q11344;
                                         # select melting point and unit
                                         p:P2101/psv:P2101 [ 
                                           wikibase:quantityUnit ?unit;
                                           wikibase:quantityAmount ?value;
                                         ].
                                # use only degrees celsius
                                BIND(wd:Q25267 AS ?unit)
                                BIND('melting point' AS ?question)
                                ?element rdfs:label ?answer.
                                FILTER((LANG(?answer)) = 'en')
                              }
                              ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                              LIMIT 4
                            } AS %items
                        
                            WHERE {
                              INCLUDE %items
                            }
                        
                            # the final results must be sorted ascending
                            ORDER BY ?value", 2, "Sort these chemical elements by {0} (ascending)!" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("e6ec8ea0-39ee-476c-81f5-b17bd99e715f"), 2, @"SELECT DISTINCT ?answer ?question WHERE {
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
                            ?capital rdfs:label ?answer;
                            filter(lang(?answer) = 'en').
                            ?state rdfs:label ?question;
                            filter(lang(?question) = 'en').
                          }
                        } 
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
                        LIMIT 4", 2, "What is the name of the capital of {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("ffd0f0da-b31d-4c01-b946-8b81fa30b00e"), 2, @"SELECT ?continent ?answer ?question WHERE {
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
                            LIMIT 4", 2, "Which continent has {0} countries?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("e1a1fbf7-850f-4fdb-878c-9e8190b54d6b"), 2, @"SELECT ?answer ?question
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
                              }
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
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) # order by random
                              LIMIT 1
                            } AS %oneState
                            WITH {
                              SELECT ?state ?empty WHERE {
                                INCLUDE %states.
                                FILTER NOT EXISTS { INCLUDE %selectedContinent. }
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) # order by random
                              LIMIT 3
                            } AS %threeStates
                            WHERE {
                                { INCLUDE %oneState. } UNION
                                { INCLUDE %threeStates. }

                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?state  rdfs:label ?answer.
                                ?continent rdfs:label ?question.
                              }
                            }
                            ORDER BY DESC(?question)", 2, "Which country is a part of {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"), 2, @"SELECT DISTINCT ?question ?answer
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
                            order by DESC(?question)", 2, "Which country is not a basin country of the Baltic Sea?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"), 2, @"# Which country is no basin country of the Caribbean Sea?
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
                            order by DESC(?noSea)", 2, "Which country is not a basin country of the Caribbean Sea?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"), 2, @"# Which country is no basin country of the Mediterranean Sea?
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
                            order by DESC(?noSea)", 2, "Which country is not a basin country of the Mediterranean Sea?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("f88bb7ba-a1dc-45c1-8c6f-1c918bf87217"), 2, @"# What is the longest river in {continent}?
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
                            limit 4", 2, "What is the longest river in {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("15f9b57e-118a-4448-b24f-b66806197ff8"), 0, @"# sort countries by number of inhabitants (ascending)
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
                            } ORDER BY ?population", 2, "Sort countries by {0} (ascending)." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("f64d3784-3584-4f19-be07-aa44fd9a4086"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("848e9590-10f6-4d16-b2cd-ca282adaee99"), 1, @"
                            # Based on the example question 'Former capitals''

                            SELECT ?question ?answer(SAMPLE(?image) AS ?image)
                            WHERE
                            {
                              # Fetches capitals from current or former members of the U.N.
                              # Capital is always the most recent capital, states are always only existing states
                              ?country wdt:P463 wd:Q1065.
                              ?country p:P36 ?stat.
                              ?stat ps:P36 ?capital.


                              ?country rdfs:label ?countryLabel.
                              ?capital rdfs:label ?capitalLabel.


                              ?capital wdt:P18 ?image.

                              OPTIONAL {
                                ?country wdt:P582|wdt:P576 ?ended.
                              }
                              OPTIONAL {
                                ?capital wdt:P582|wdt:P576 ?ended.
                              }
                              OPTIONAL {
                                ?stat pq:P582 ?ended.
                              }


                              FILTER(!BOUND(?ended)).
                              FILTER(LANG(?countryLabel) = 'en').
                              FILTER(LANG(?capitalLabel) = 'en').

                              BIND(CONCAT(?capitalLabel, ', ', ?countryLabel) as ?answer).
                              BIND('Where are we?' AS ?question).
                            }
                            GROUP BY ?question ?answer
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) #order by random
                            LIMIT 4
                            ", 2, "Where are we?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"), new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"), new Guid("848e9590-10f6-4d16-b2cd-ca282adaee99"), 1, @"
                            #Which famous landmark is this: {image}
                            SELECT ?question ?answer ?image
                            WITH{
                            SELECT ?tes (CONCAT( ?ans, '(', ?country, ')' ) as ?answer) WHERE {
                              { SELECT DISTINCT(?answer as ?ans)(MAX(?image) as ?tes) ?country WHERE { 
                                ?landmark wdt:P31 / wdt:P279 * wd:Q2319498;
                                    wikibase:sitelinks ?sitelinks;
                                    wdt:P18 ?image;
                                    wdt:P17 ?cntr.
                                ?landmark wdt:P1435 ?type.
                                FILTER(?sitelinks >= 10)
                                    ?landmark rdfs:label ?answer

                                    filter(lang(?answer) = 'en').
                                    ?cntr rdfs:label ?country filter(lang(?country) = 'en').
                                }
                                GROUP BY ?answer ?country
                                ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              }
                            }
                            } as %allMonuments

                            WITH
                            {
                                SELECT ?tes ?answer
                              WHERE
                                {
                                    Include %allMonuments
                              }
                                Limit 1
                            } as %selectedMonument

                            WITH
                            {
                                SELECT ?tes ?answer
                            WHERE
                                {
                                 Include %allMonuments
                                 FILTER NOT EXISTS { INCLUDE %selectedMonument}
                                }
                                Limit 3
                            } as %decoyMonuments

                            WHERE
                            {
                              { INCLUDE %selectedMonument}
                                UNION
                                { INCLUDE %decoyMonuments}
                                Bind(?tes as ?image)
                                BIND('Which famous landmark is this' as ?question)
                            }
                            ORDER BY DESC(?question)
                            ", 2, "Which famous landmark is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), new Guid("184f3c3b-a831-4a0b-8c01-a846608f139b"), 0, @"
                             # US presidents by start of their presidency
                             # ?question contains question template value
                             # ?answer contains the label for the different answer options
                             # ?firstElectionPeriod is ignored by the server but used to sort the final result
                             SELECT ?question ?answer ?firstElectionPeriod
                         
                             WITH {
                                 # The MIN(...)-thing for ?startTime is a trick to convert a date like 01-12-2012 (in format DD-MM-YYYY) into an integer
                                 # that's formatted like YYYYMMDD and that we can easily sort :)
                                 SELECT ?person ?personLabel (MIN(YEAR(?startTime) * 100 * 100 + MONTH(?startTime) * 100 + DAY(?startTime)) AS ?firstElectionPeriod) WHERE {
                                   # start by looking at real humans only because we're not interested in Lex Luthor
                                   ?person wdt:P31 wd:Q5.
                                   ?person p:P39 ?usPresident.
                                   ?usPresident rdf:type wikibase:BestRank;
                                     ps:P39 wd:Q11696;
                                     pq:P582 ?endTime; # <- because this is a history question, we only look at presidencies that ended
                                     pq:P580 ?startTime.
                               
                                   # If we don't have an end time, we use the current time as a default value
                                   # BIND(IF(!(BOUND(?endTime)), NOW(), ?endTime) AS ?endTime)
                                   SERVICE wikibase:label {
                                     bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'.
                                     ?person rdfs:label ?personLabel.
                                   }
                                 }
                                 GROUP BY ?person ?personLabel
                                 # Shuffle the results
                                 ORDER BY MD5(CONCAT(STR(?person),  STR(NOW())))
                                 LIMIT 4
                             } AS %presidents
                         
                             WHERE {
                                 INCLUDE %presidents
                                 BIND(?personLabel AS ?answer)
                                 BIND('their first election period' AS ?question)
                             } 
                             ORDER BY ?firstElectionPeriod
                           ", 2, "Sort these US presidents by {0} (ascending)." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), new Guid("2af47804-eeaa-4bcd-98e3-f515aeaf30b5"), 0, @"# sort EU countries by the date they joined
                            SELECT ?date (SAMPLE(?answer) AS ?answer) (SAMPLE(?question) AS ?question) 
                            WITH {
                              SELECT DISTINCT (?memberOfEuSince as ?date) ?answer WHERE {
                                {SELECT ?item ?memberOfEuSince
                                              WHERE 
                                              {
                                                ?item p:P463 [ps:P463 wd:Q458;
                                                                      pq:P580 ?memberOfEuSince].
                                              }
                                }
                                SERVICE wikibase:label {
                                  bd:serviceParam wikibase:language 'en'.
                                  ?item  rdfs:label ?answer.
                                }
                              }
                            } AS %dates
                            WITH {
                              SELECT DISTINCT ?date WHERE {
                                INCLUDE %dates.
                              }
                              ORDER BY MD5(CONCAT(STR(?date), STR(NOW())))
                              LIMIT 4
                            } AS %fourDates
                            WHERE {
                              INCLUDE %fourDates.
                              INCLUDE %dates.
                              BIND('the date they joined the EU' as ?question).
                            }
                            GROUP BY ?date
                            ORDER BY ?date", 2, "Sort the countries by {0} (ascending)." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), new Guid("976e1e61-3b95-43bf-8e4c-1963b6795113"), 2, @"# German Chancellors
                            SELECT ?answer (CONCAT(STR(?startYear), ' to ', STR(?endYear)) AS ?question) WHERE {
                              ?person p:P39 ?Bundeskanzler.
                              ?Bundeskanzler ps:P39 wd:Q4970706;
                                             pq:P580 ?start;
                                             pq:P582 ?end. # <- note the mandatory end date

                              BIND(YEAR(?start) AS ?startYear)
                              BIND(YEAR(?end) AS ?endYear)

                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?person rdfs:label ?answer.
                              }
                            }
                            ORDER BY (MD5(CONCAT(STR(?person), STR(NOW()))))
                            LIMIT 4", 2, "Who was Federal Chancellor of Germany from {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), new Guid("fc74c29e-b4a9-428b-96c9-b41127869a31"), 2, @"# empires and colonies
                            SELECT DISTINCT ?empire (?empireLabel as ?question) ?colony (?colonyLabel as ?answer)
                            WITH {
                                SELECT DISTINCT ?empire ?empireLabel ?colony ?colonyLabel WHERE {
                                {
                                    SELECT ?empire ?empireLabel ?colony ?colonyLabel WHERE {
                                    ?empire (wdt:P31/(wdt:P279*)) wd:Q1790360.
                                    ?colony wdt:P361 ?empire;
                                            wdt:P31 wd:Q133156;
                                            wdt:P576 ?enddate.
                                    FILTER(?enddate >= '1790-01-01T00:00:00Z'^^xsd:dateTime)
                                    SERVICE wikibase:label {
                                        bd:serviceParam wikibase:language 'en'.
                                        ?empire rdfs:label ?empireLabel.
                                        ?colony rdfs:label ?colonyLabel.
                                    }
                                    }
                                }
                                UNION
                                {
                                    SELECT DISTINCT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                    ?colony (wdt:P31/(wdt:P279*)) wd:Q133156;
                                                                    wdt:P576 ?enddate;
                                                                    wdt:P17 ?empire.
                                    VALUES ?empire {
                                        wd:Q8680
                                    }
                                    SERVICE wikibase:label { 
                                        bd:serviceParam wikibase:language 'en'. 
                                        ?empire rdfs:label ?empireLabel.
                                        ?colony rdfs:label ?colonyLabel.
                                    }
                                    FILTER(?enddate >= '1790-01-01T00:00:00Z'^^xsd:dateTime)
                                    }
                                }
                                FILTER(!(CONTAINS(?colonyLabel, 'Q')))
                                }
                            } as %colonies

                            WITH {
                                SELECT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                INCLUDE %colonies.
                                {
                                    SELECT ?empire WHERE {
                                    INCLUDE %colonies.
                                    } GROUP BY ?empire 
                                    ORDER BY (MD5(CONCAT(STR(?empire), STR(NOW()))))
                                    LIMIT 1
                                }
                                }
                            } as %selectedEmpire

                            WITH {
                                SELECT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                INCLUDE %selectedEmpire.
                                } ORDER BY (MD5(CONCAT(STR(?colony), STR(NOW())))) LIMIT 1
                            } as %selectedColony

                            WITH {
                                SELECT ?colony ?colonyLabel ?empty ?emptyLabel WHERE {
                                INCLUDE %colonies.
                                MINUS {INCLUDE %selectedEmpire.}.
    
                                } ORDER BY (MD5(CONCAT(STR(?colony), STR(NOW())))) LIMIT 3
                            } as %threeOtherColonies

                            WHERE {
                                {INCLUDE %selectedColony.}
                                UNION
                                {INCLUDE %threeOtherColonies.}
                            } ORDER BY DESC(?empire)", 2, "Which colony belonged to the {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), new Guid("d375ff0f-cb79-4eac-84e6-c4bf65c2382a"), 2, @"# wars of the 20th century
                            SELECT (SAMPLE(?itemLabel) AS ?answer) (YEAR(MAX(?startdate)) AS ?question) WHERE {
                              {
                                SELECT ?item ?itemLabel ?startdate WHERE {
                                  ?item (wdt:P31/(wdt:P279*)) wd:Q198.
                                  ?item wdt:P580 ?startdate.
                                  FILTER(?startdate >= '1900-01-01T00:00:00Z'^^xsd:dateTime)
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                                }
                              }
                              FILTER(!(CONTAINS(?itemLabel, '1')))
                              FILTER(!(CONTAINS(?itemLabel, '2')))
                              FILTER(!(STRSTARTS(?itemLabel, 'Q')))
                            }
                            GROUP BY ?itemLabel
                            ORDER BY (MD5(CONCAT(STR(?item), STR(NOW()))))
                            LIMIT 4", 2, "Which of these wars started in {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), new Guid("26333055-8b72-4b65-b622-1bfac80e0adc"), 1, @"
                        # Which U.S. president's signature is this: {question}
                        SELECT  ?question ?answer ?image
                        WITH{
                          SELECT ?answer ?image
                        WHERE { 
	                        ?president wdt:P39 wd:Q11696.
                            ?president wdt:P109 ?image.
	                        OPTIONAL {
		                        ?president rdfs:label ?answer
		                        filter(lang(?answer) = 'en').
	                        }
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                        } as %allPresidents

                        WITH{
                          SELECT ?image ?answer
                          WHERE{
                             Include %allPresidents
                          }
                          Limit 1
                        } as %selectedPresident
                        WITH{
                          SELECT  ?answer
                          WHERE{
                             Include %allPresidents
                                     FILTER NOT EXISTS {INCLUDE %selectedPresident}
                          }
                          Limit 3
                        } as %decoyPresidents

                        WHERE {
                          {INCLUDE %selectedPresident}
                          UNION
                          {INCLUDE %decoyPresidents}
                          BIND('Whose presidents signature is this?' as ?question)
                        }
                        ORDER BY DESC(?image)
                        ", 2, "Which of presidents signature is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"), new Guid("1b9185c0-c46b-4abf-bf82-e464f5116c7d"), new Guid("6cfc621c-7a35-464a-80cd-3937a6d2af3d"), 0, @"# sort planets by average distance to sun
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
                            } ORDER BY ?avgDistanceToSun", 2, "Sort planets by {0} (ascending)." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"), new Guid("1b9185c0-c46b-4abf-bf82-e464f5116c7d"), new Guid("3de1256d-f8d2-4418-a932-d459d5ee44d6"), 0, @"# sort planets by radius
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
                            ORDER BY ?radius", 2, "Sort planets by {0} (ascending)." });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("14d93797-c61c-4415-b1ed-359d180237ff"), new Guid("1b9185c0-c46b-4abf-bf82-e464f5116c7d"), new Guid("98a751f6-eee0-4d79-9401-992417283aa9"), 2, @"#Which of these moons belongs to the planet {0}?
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
                            } ORDER BY DESC(?question)", 2, "Which of these moons belongs to {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("e9ab8641-23b7-428f-b8d6-d96ae7d17e6f"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"), 0, @"
                            # Sort animals by gestation time (days)? (onlycarnivores, artiodactyla and rodents)
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25329.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allArtiodactyla

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allArtiodactyla}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime
                        ", 2, "Sort these animals by gestation period (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("6ba60225-d7be-4c97-ad19-eaf895a14734"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"), 0, @"
                            # Sort animals by gestation time (days)? (only marsupials, artiodactyla, primates)
                            # Sort animals by gestation time (days)?
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 


                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25329.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allArtiodactyla

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Primates.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allPrimates

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allArtiodactyla}
                                  UNION
                                  {INCLUDE %allPrimates}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime
                        ", 2, "Sort these animals by gestation period (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("9fd3f504-eb96-42f3-91bc-606a17759e45"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"), 0, @"
                            # Sort animals by gestation time (days)? (only rodentia, carnivora, marsupial)
                            # Sort animals by gestation time (days)?
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25336.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allMarsupial

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allMarsupial}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime              
                        ", 2, "Sort these animals by gestation period (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("9d630c39-6606-49ff-8cef-b34695d8ed91"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"), 0, @"
                            # Sort animals by gestation time (days)? (only primates, rodents, carnivora)
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q7380.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allPrimates

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allPrimates}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime             
                        ", 2, "Sort these animals by gestation period (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"), new Guid("3d6c54d3-0fda-4923-a00e-e930640430b3"), new Guid("039acc70-30d3-40fe-a28a-0b44964d49e7"), 2, @"
                            SELECT  (SAMPLE(?question) AS ?question)(SAMPLE(?answer) AS ?answer) (SAMPLE(?teamLabel) AS ?team) ?soccerTeam 
                            WITH{
                              #get all teams with coaches
                              SELECT DISTINCT ?soccerTeam ?soccerTeamLabel ?coach ?coachLabel ?answer ?league ?teamLabel
                               WHERE{
                                 ?soccerTeam wdt:P31 wd:Q476028.
                                 ?soccerTeam wdt:P118 ?league.
                                 FILTER(?league IN (wd:Q82595, wd:Q35572))
                                 ?soccerTeam wdt:P286 ?coach.
                               SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language 'en'.
                                      ?coach rdfs:label ?answer.
                                      ?soccerTeam rdfs:label ?teamLabel.
                                     }
                               } 
                            } AS %allCoaches

                            WITH{
                              # get teams with coaches that have quit
                              SELECT ?soccerTeam ?coach ?endTime
                              WHERE{
                                 ?soccerTeam wdt:P31 wd:Q476028.
                                 ?soccerTeam wdt:P286 ?coach.
                                 ?coach p:P6087 [ps:P6087 ?soccerTream;
                                                          pq:P582 ?endTime]
                               } 
                            }as %endTimeCoaches   

                            #filter only teams with active coaches
                            WITH{
                              SELECT *
                              WHERE{
                              INCLUDE %allCoaches
                              FILTER NOT EXISTS { INCLUDE %endTimeCoaches. }
                              }
                            } as %activeCoaches

                            WITH{
                              #selectr a coach
                              SELECT *
                              WHERE{
                               INCLUDE %activeCoaches
                                        SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language 'en'.
                                      ?soccerTeam rdfs:label ?question.
                                     }
                              }
                              ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW())))) 
                              LIMIT 1
                            } as %selectedCoach

                            WITH{
                              #select decoy coaches that is not the selected coach
                              SELECT *
                              WHERE{
                                  INCLUDE %activeCoaches
                                  FILTER NOT EXISTS {INCLUDE %selectedCoach.}
                                }
                              ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW())))) 
                              LIMIT 3
                            } as %decoyCoaches

                            WHERE{
                              {INCLUDE %selectedCoach}
                              UNION
                              {INCLUDE %decoyCoaches}
                              BIND(?coachLabel as ?question)
                            }
                            GROUP BY ?soccerTeam
                            ORDER BY DESC(?question)
                            ", 2, "Who is the trainer of {0} ?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"), new Guid("3d6c54d3-0fda-4923-a00e-e930640430b3"), new Guid("0b1ff760-e02f-4ddc-8f32-5161931ebcbe"), 2, @"
                            SELECT (Sample(GROUP_CONCAT( DISTINCT ?question; SEPARATOR=', ')) AS ?question) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?answer); SEPARATOR=', ')) AS ?answer) 
                            ?playerCount
                            WITH {
                                 SELECT ?sport ?playerCount ?sportLabel ?answer
                                 WHERE {
	                                   ?sport wdt:P31 wd:Q31629.
                                       ?sport wdt:P1873 ?playerCount.
                                       SERVICE wikibase:label {
                                                                 bd:serviceParam wikibase:language 'en'.
                                                                 ?sport rdfs:label ?answer.
                                       }
                                 }
                                 ORDER BY MD5(CONCAT(STR(?playerCount), STR(NOW())))
                            } as %sports
                        
                            WITH{
                            SELECT DISTINCT ?playerCount 
                                    WHERE {
                                            INCLUDE %sports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?playerCount), STR(NOW())))
                                    LIMIT 4
                                    } AS %fourSports

                            WHERE {
                                   INCLUDE %fourSports.
                                   INCLUDE %sports.
                                           BIND('participating players' as ?question)
                                    }
                            GROUP BY ?playerCount
                            ORDER BY ?playerCount
                            ", 2, "Who is the trainer of {0} ?" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendId",
                table: "Friends",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_RecipientId",
                table: "GameRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_SenderId",
                table: "GameRequests",
                column: "SenderId");

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
                name: "IX_MiniGames_QuestionId",
                table: "MiniGames",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_TileId",
                table: "MiniGames",
                column: "TileId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRatings_QuestionId",
                table: "QuestionRatings",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CategoryId",
                table: "Questions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MinigameId",
                table: "Reports",
                column: "MinigameId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "GameRequests");

            migrationBuilder.DropTable(
                name: "GameUser");

            migrationBuilder.DropTable(
                name: "QuestionRatings");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

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
                name: "AspNetUsers");
        }
    }
}
