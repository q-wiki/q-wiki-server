using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixImageQuerySpaceEntertainmentSport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "01af50db-4955-4953-bab1-82599d69ead0");

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("39508588-107a-4220-a748-a72eaad711db"), new Guid("1b9185c0-c46b-4abf-bf82-e464f5116c7d"), new Guid("7446071a-c64f-4b5a-97f3-10170a0824ac"), 1, @"
                            SELECT ?question ?answer
                            WITH{
                              Select distinct ?planet ?planetLabel ?image
                              WHERE{
                                  ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                  ?planet wdt:P18 ?image
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                         ?planet rdfs:label ?planetLabel}
                              }
                            } as %allPlanets

                            WITH{
                                SELECT ?planetLabel ?image
                                WHERE{
                                  INCLUDE %allPlanets
                                }
                              LIMIT 1
                            } as %selectedPlanet

                            WITH {
                               SELECT ?planetLabel  
                               WHERE{
                                  INCLUDE %allPlanets
                                  FILTER NOT EXISTS{INCLUDE %selectedPlanet}
                                }
                              LIMIT 3
                            } as %decoyPlanets

                            WHERE{
                              {INCLUDE %selectedPlanet}
                              UNION
                              {INCLUDE %decoyPlanets}
                              Bind(?image as ?question)
                              Bind(?planetLabel as ?answer)
                            }

                            ORDER BY DESC(?question)
                    ", 2, "Which planet is this?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("1e5551e1-a6a3-42df-8447-b0ac0effc8e6"), new Guid("2a388146-e32c-4a08-a246-472eff12849a"), new Guid("37c3f290-ec79-4639-a8b8-c8a2ff2c2987"), 1, @"
                            SELECT DISTINCT ?question ?answer
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel
                            WHERE{
                              ?actor wdt:P31 wd:Q5;
                                     wdt:P106 wd:Q33999;
                                     wdt:P166 ?award.
                              ?award wdt:P31+ wd:Q19020.
                              ?actor wdt:P18 ?image
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?movie rdfs:label ?movieLabel.
                                                     ?actor rdfs:label ?actorLabel.
                                                     ?award rdfs:label ?awardLabel}
                            }
                              ORDER BY ?actor ?actorLabel
                            } as %allWinners
                                       
                            WITH{
                              SELECT ?actor ?actorLabel ?image
                              WHERE{
                                 INCLUDE %allWinners.
                                ?actor wdt:P18 ?image
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 1
                            } as %selectedActor

                            WITH{
                              SELECT ?gender
                              WHERE{
                                 INCLUDE %selectedActor
                                 {?actor wdt:P21 ?gender}
                              }
                            } as %selectedGender
                            WITH{
                              SELECT ?gender
                              WHERE{
                                 INCLUDE %allWinners
                                 ?actor wdt:P21 ?gender
                                 Filter NOT EXISTS {
                                  include %selectedGender
                                 }
                              }
                              LIMIT 1
                            } as %filteredGenders

                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel
                              WHERE{
                                INCLUDE %allWinners
                                Filter NOT EXISTS {INCLUDE %selectedActor}
                                ?actor wdt:P21 ?gender
                                Filter NOT EXISTS {
                                  include %filteredGenders
                                }
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?actor rdfs:label ?actorLabel.}
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 3
                            } as %decoyActors

                            WHERE{
                                {INCLUDE %selectedActor}
                                UNION
                                {INCLUDE %decoyActors}
                                BIND(?image as ?question)
                                BIND(?actorLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ", 2, "Who is this actor?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "GroupId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("fa1e5667-cc80-4526-99bd-fd4fc539d4fd"), new Guid("3d6c54d3-0fda-4923-a00e-e930640430b3"), new Guid("28f59994-6a6b-4c9a-bc7a-a5c245c11ddb"), 1, @"
                            SELECT DISTINCT ?question ?answer
                            WITH {
                                 SELECT ?sport ?sportLabel ?image
                                 WHERE {
	                                   ?sport wdt:P31 wd:Q31629.
                                       ?sport wdt:P18 ?image
                                       SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                ?sport rdfs:label ?sL
                                                              }
                                 filter(lang(?sportLabel) = 'en').
                                 BIND(CONCAT(UCASE(SUBSTR(?sL, 1, 1)), SUBSTR(?sL, 2)) as ?sportLabel)
                                 }
                                 ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                            } as %allSports
                        
                            WITH{
                            SELECT DISTINCT ?sportLabel ?image
                                    WHERE {
                                      INCLUDE %allSports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                                    LIMIT 1
                            } AS %selectedSport

                            WITH{
                              SELECT DISTINCT ?sportLabel
                              WHERE{
                                INCLUDE %allSports.
                                FILTER NOT EXISTS {INCLUDE %selectedSport}
    
                              }
                              ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                              LIMIT 3
                            } as %decoySport

                            WHERE {
                                   {INCLUDE %selectedSport}
                                    union
                                   {INCLUDE %decoySport}
                                   BIND(?image as ?question)
                                   BIND(?sportLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ", 2, "Which sport is this?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1e5551e1-a6a3-42df-8447-b0ac0effc8e6"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("39508588-107a-4220-a748-a72eaad711db"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fa1e5667-cc80-4526-99bd-fd4fc539d4fd"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "847fd22d-3644-4da9-8d8b-3ffc06b9a9e7");
        }
    }
}
