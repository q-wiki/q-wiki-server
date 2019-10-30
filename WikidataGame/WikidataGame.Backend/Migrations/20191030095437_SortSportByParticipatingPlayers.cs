using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class SortSportByParticipatingPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "3d6c54d3-0fda-4923-a00e-e930640430b3", "Sports" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "2bcedb7d-7df7-4bb3-a15a-cd94f65d41bf", "3d6c54d3-0fda-4923-a00e-e930640430b3", 0, @"# Sort Sports by participating players
                     SELECT (SAMPLE(?answer) AS ?answer) (SAMPLE(?question) AS ?question) ?playerCount
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
                              ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                            } as %sports
                        WITH{
                            SELECT DISTINCT ?playerCount WHERE {
                                INCLUDE %sports.
                            }
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                            LIMIT 4
                        } AS %fourSports
                        WHERE {
                            INCLUDE %fourSports.
                            INCLUDE %sports.
                            BIND('participating players' as ?question)
                        }
                        GROUP BY ?playerCount
                        ORDER BY ?playerCount", "Sort Sports by {0} (scending)" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "2bcedb7d-7df7-4bb3-a15a-cd94f65d41bf");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "3d6c54d3-0fda-4923-a00e-e930640430b3");
        }
    }
}
