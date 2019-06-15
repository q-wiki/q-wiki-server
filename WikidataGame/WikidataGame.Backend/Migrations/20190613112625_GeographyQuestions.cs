using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class GeographyQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761",
                column: "SparqlQuery",
                value: @"SELECT ?answer ?question WHERE {  
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
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "aca0f5f7-b000-42fb-b713-f5fe43748761",
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"SELECT ?answer (COUNT(?item) AS ?question)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "29fed1d0-d306-4946-8109-63b8aaf0262e");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "46679c4f-ef97-445d-9a70-d95a5337720f");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "4f6c477e-7025-44b4-a3b0-f3ebd8902902");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "9a70639b-3447-475a-905a-e866a0c98a1c");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a6a470de-9efb-4fde-9388-6eb20f2ff1f4");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "f88a4dc0-8187-43c4-8775-593822bf4af1");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761",
                column: "SparqlQuery",
                value: @"SELECT ?answer ?question WHERE {  
                          ? item wdt:P31 wd:Q5119.
                          ? item wdt:P1376 ? land.
                          ? land wdt : P31 wd:Q6256.
                          OPTIONAL { 
                            ?item rdfs:label ? answer;
                                    filter(lang(?answer) = 'en')
                              ? land rdfs:label? question;
                                    filter(lang(?question) = 'en').
                          }
                            }
                        ORDER BY RAND() LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "aca0f5f7-b000-42fb-b713-f5fe43748761",
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"SELECT ?answer (COUNT(?item) AS ?question)
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
                        ORDER BY RAND()
                        LIMIT 4", "How many countries are on the continent {0}?" });
        }
    }
}
