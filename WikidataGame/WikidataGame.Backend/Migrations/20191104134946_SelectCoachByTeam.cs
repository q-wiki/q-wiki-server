using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class SelectCoachByTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "2bcedb7d-7df7-4bb3-a15a-cd94f65d41bf",
                column: "TaskDescription",
                value: "Sort Sports by {0} (ascending)");

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "183a8a5d-588b-4a8c-91af-8314c6f8a0a9", "3d6c54d3-0fda-4923-a00e-e930640430b3", 2, @"# select the current coach of the team
                     SELECT (SAMPLE(?answer) AS ?answer) (SAMPLE(?question) AS ?question)(SAMPLE(?teamLabel) AS ?team) ?soccerTeam 
                        WITH{
                          #get all teams with coaches
                          SELECT DISTINCT ?soccerTeam ?soccerTeamLabel ?coach ?coachLabel ?answer ?league ?teamLabel
                           WHERE{
                             ?soccerTeam wdt:P31 wd:Q476028.
                             ?soccerTeam wdt:P118 ?league.
                             #team in bundesliga or uefa
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
                        ORDER BY DESC(?question)", "Select the current coach of {0}" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "183a8a5d-588b-4a8c-91af-8314c6f8a0a9");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "2bcedb7d-7df7-4bb3-a15a-cd94f65d41bf",
                column: "TaskDescription",
                value: "Sort Sports by {0} (scending)");
        }
    }
}
