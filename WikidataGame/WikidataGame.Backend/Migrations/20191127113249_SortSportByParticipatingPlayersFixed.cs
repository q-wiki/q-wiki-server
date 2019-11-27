﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class SortSportByParticipatingPlayersFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "183a8a5d-588b-4a8c-91af-8314c6f8a0a9",
                column: "SparqlQuery",
                value: @"# select the current coach of the team
                     SELECT (Sample(GROUP_CONCAT( DISTINCT ?question; SEPARATOR="","")) AS ?question) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?answer); SEPARATOR="","")) AS ?answer) ?playerCount
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
                        ORDER BY ?playerCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "183a8a5d-588b-4a8c-91af-8314c6f8a0a9",
                column: "SparqlQuery",
                value: @"# select the current coach of the team
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
                        ORDER BY DESC(?question)");
        }
    }
}
