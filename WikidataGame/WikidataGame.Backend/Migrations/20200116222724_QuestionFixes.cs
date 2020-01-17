using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class QuestionFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "TaskDescription",
                value: "Which president has this signature?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "TaskDescription",
                value: "Which country does not border the Baltic Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "TaskDescription",
                value: "Sort these softdrinks by inception.");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"# Which country does not border the Caribbean Sea?
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
                            order by DESC(?noSea)", "Which country does not border the Caribbean Sea?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6ba60225-d7be-4c97-ad19-eaf895a14734"),
                column: "TaskDescription",
                value: "Sort these animals by duration of pregnancy (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "TaskDescription",
                value: "Order these animals by bite force quotient (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9d630c39-6606-49ff-8cef-b34695d8ed91"),
                column: "TaskDescription",
                value: "Sort these animals by duration of pregnancy (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9fd3f504-eb96-42f3-91bc-606a17759e45"),
                column: "TaskDescription",
                value: "Sort these animals by duration of pregnancy (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"# Which country does not border the Mediterranean Sea?
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
                            order by DESC(?noSea)", "Which country does not border the Mediterranean Sea?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "TaskDescription",
                value: "Who is the trainer of {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "TaskDescription",
                value: "What is the name of this painting?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b95607f9-8cd6-48e8-bc99-c9c305e812be"),
                column: "MiniGameType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d0fcf5ac-3215-4355-9090-b6a49cf66cc3"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            SELECT DISTINCT ?question ?answer ?awardLabel ?movieLabel
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel ?movieLabel ?awardLabel
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
                              SELECT ?actor ?actorLabel ?question ?movieLabel ?awardLabel
                              WHERE{
                                 INCLUDE %allWinners.
                                 BIND(CONCAT('Who won the ', CONCAT(STR(?awardLabel), CONCAT(' for ', CONCAT(STR(?movieLabel), '?'))))  as ?question)
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
                            ", "{0}" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e9ab8641-23b7-428f-b8d6-d96ae7d17e6f"),
                column: "TaskDescription",
                value: "Sort these animals by duration of pregnancy (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Who is the trainer of {0}?");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "TaskDescription",
                value: "Which of presidents signature is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "TaskDescription",
                value: "Which country is not a basin country of the Baltic Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "TaskDescription",
                value: "Sort these softdrinks by inception");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"# Which country is no basin country of the Caribbean Sea?
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
                            order by DESC(?noSea)", "Which country is not a basin country of the Caribbean Sea?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6ba60225-d7be-4c97-ad19-eaf895a14734"),
                column: "TaskDescription",
                value: "Sort these animals by gestation period (ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "TaskDescription",
                value: "Order these animals by bite force quotient (Ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9d630c39-6606-49ff-8cef-b34695d8ed91"),
                column: "TaskDescription",
                value: "Sort these animals by gestation period (ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9fd3f504-eb96-42f3-91bc-606a17759e45"),
                column: "TaskDescription",
                value: "Sort these animals by gestation period (ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"# Which country is no basin country of the Mediterranean Sea?
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
                            order by DESC(?noSea)", "Which country is not a basin country of the Mediterranean Sea?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "TaskDescription",
                value: "Who is the trainer of {0} ?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "TaskDescription",
                value: "What is the name of the painting?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b95607f9-8cd6-48e8-bc99-c9c305e812be"),
                column: "MiniGameType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d0fcf5ac-3215-4355-9090-b6a49cf66cc3"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            SELECT DISTINCT ?question ?answer ?awardLabel ?movieLabel
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel ?movieLabel ?awardLabel
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
                              SELECT ?actor ?actorLabel ?question ?movieLabel ?awardLabel
                              WHERE{
                                 INCLUDE %allWinners.
                                 BIND(CONCAT('Who won the ', CONCAT(STR(?awardLabel), CONCAT(' for the movie ', CONCAT(STR(?movieLabel), '?'))))  as ?question)
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
                            ", "Who won the {3} for for the movie {4}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e9ab8641-23b7-428f-b8d6-d96ae7d17e6f"),
                column: "TaskDescription",
                value: "Sort these animals by gestation period (ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Who is the trainer of {0} ?");
        }
    }
}
