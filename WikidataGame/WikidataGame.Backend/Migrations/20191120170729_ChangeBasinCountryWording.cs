using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class ChangeBasinCountryWording : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "46679c4f-ef97-445d-9a70-d95a5337720f",
                column: "TaskDescription",
                value: "Which country does not border the Baltic Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "4f6c477e-7025-44b4-a3b0-f3ebd8902902",
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
                keyValue: "a6a470de-9efb-4fde-9388-6eb20f2ff1f4",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "46679c4f-ef97-445d-9a70-d95a5337720f",
                column: "TaskDescription",
                value: "Which country is not a basin country of the Baltic Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "4f6c477e-7025-44b4-a3b0-f3ebd8902902",
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
                keyValue: "a6a470de-9efb-4fde-9388-6eb20f2ff1f4",
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
        }
    }
}
