using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixQueriesAwardsParticipatingPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "3090246e-346f-45f3-b13c-9636768a87c7");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer ?awardCount
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
                            SELECT DISTINCT ?actor
                            WHERE{
                               {INCLUDE %topGrossingMovies}
                              # get all actors that played in those movies
                              ?movie wdt:P161 ?actor.
                              ?actor wdt:P166 ?award.
                              }
                              ORDER BY MD5(CONCAT(STR(?award), STR(NOW())))
                              limit 150
                            } as %allWinners

                            WITH{
                            SELECT DISTINCT ?actorLabel (count(?award) as ?awardCount)
                              WHERE {
                                     {
                                        SELECT ?actor
                                        WHERE {include %allWinners}
                                     }
                                     ?actor wdt:P166 ?award.
                                     ?award wdt:P31*/wdt:P279* wd:Q4220917
                                     #?baseAward.
                                     #VALUES ?baseAward {wd:Q4220917 wd:Q1407225}
                                     SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                                                     ?actor rdfs:label ?actorLabel
                                                                                    }
                              }
                              group by ?actorLabel
                            } as %countedMovies

                            WITH{
                               select (Group_Concat(distinct  sample(?actorLabel); separator=', ') as ?actors) ?awardCount
                               where {
                                    include %countedMovies
                               }
                               group by ?awardCount
                               ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                               limit 4
                            } as %summedActors

                            WHERE{
                                {INCLUDE %summedActors}
                                BIND(?actors as ?answer)
                                BIND('Sort actors by oscars received.' as ?question)
                            }
                            ORDER BY asc(?awardCount)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("39508588-107a-4220-a748-a72eaad711db"),
                column: "SparqlQuery",
                value: @"
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
                    ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by maximum amount ofparticipating players.(ascending)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "c4a7b1e6-8067-4fe9-a8f4-ed76b64e02f2");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer ?awardCount
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
                                SELECT DISTINCT ?actor ?award
                                WHERE{
                                   {INCLUDE %topGrossingMovies}
                                  # get all actors that played in those movies
                                  ?movie wdt:P161 ?actor.
                                  ?actor wdt:P166 ?award.
                                  }
                                  ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                                  limit 500
                                } as %allWinners

                                WITH{
                                SELECT DISTINCT ?actorLabel (count(?award) as ?awardCount)
                                  WHERE {
                                         {INCLUDE %allWinners.}
                                         ?award wdt:P31*/wdt:P279* wd:Q4220917
                                         #?baseAward. logic if we want to count movie and television awards
                                         #VALUES ?baseAward {wd:Q4220917 wd:Q1407225}
                                         SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                                                         ?actor rdfs:label ?actorLabel
                                                                                        }
                                  }
                                  group by ?actorLabel
                                } as %countedMovies

                                WITH{
                                   select (Group_Concat(distinct sample(?actorLabel); separator=', ') as ?actors) ?awardCount
                                   where {
                                        include %countedMovies
                                        filter(?awardCount > 1)
                                   }
                                   group by ?awardCount
                                   ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                                   limit 4
                                } as %summedActors

                                WHERE{
                                    {INCLUDE %summedActors}
                                    BIND(?actors as ?answer)
                                    BIND('Sort actors by oscars received.' as ?question)
                                }
                                ORDER BY asc(?awardCount)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("39508588-107a-4220-a748-a72eaad711db"),
                column: "SparqlQuery",
                value: @"
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
                    ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by participating players.");
        }
    }
}
