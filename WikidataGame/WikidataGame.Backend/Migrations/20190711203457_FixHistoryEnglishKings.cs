using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixHistoryEnglishKings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "0d218830-55d2-4d66-8d8f-d402514e9202",
                column: "SparqlQuery",
                value: @"# wars of the 20th century
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
                        LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "9a70639b-3447-475a-905a-e866a0c98a1c",
                column: "SparqlQuery",
                value: @"SELECT ?answer ?question
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
                        ORDER BY DESC(?question)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d9011896-04e5-4d32-8d3a-02a6d2b0bdb6",
                column: "SparqlQuery",
                value: @"#English kings until 1707
                        SELECT DISTINCT ?question ?answer ?reignstart ?reignend WHERE {
                          {
                            SELECT DISTINCT ?human ?name (MIN(?reignstart) as ?reignstart) (MIN(?reignend) as ?reignend) WHERE {
                              ?human wdt:P31 wd:Q5;
                                p:P39 ?memberOfStatement.
                              ?memberOfStatement rdf:type wikibase:BestRank;
                                ps:P39 wd:Q18810062;
                                pq:P580 ?reignstart;
                                pq:P582 ?reignend.
                              FILTER(?reignstart >= '1066-12-31T00:00:00Z'^^xsd:dateTime)
                              MINUS { ?human wdt:P97 wd:Q719039. }
                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?human rdfs:label ?name.
                              }
                            } GROUP BY ?human ?name
                            ORDER BY (MD5(CONCAT(STR(?name), STR(NOW()))))
                            LIMIT 4
                          }
                          BIND(?name AS ?answer)
                          BIND('the beginning of their reigning period' AS ?question)
                        }
                        ORDER BY (?reignstart)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "0d218830-55d2-4d66-8d8f-d402514e9202",
                column: "SparqlQuery",
                value: @"# wars of the 20th century
                        SELECT (SAMPLE(?itemLabel) as ?answer)  (YEAR(SAMPLE(?startdate)) as ?question) 
                        WHERE {
                          {
                            SELECT DISTINCT ?item ?itemLabel ?startdate ?enddate (CONCAT(STR(YEAR(?startdate)), ' - ', STR(YEAR(?enddate))) AS ?time)  WHERE {
                              ?item (wdt:P31/(wdt:P279*)) wd:Q198;
        
                                p:P582 ?memberOfStatementEnd.
                                      ?memberOfStatementEnd a wikibase:BestRank; ps:P582 ?enddate.                     
    
                              ?item p:P580 ?memberOfStatementStart.
                             ?memberOfStatementStart a wikibase:BestRank; ps:P580 ?startdate.
                              FILTER(?startdate >= '1900-01-01T00:00:00Z'^^xsd:dateTime)
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                            } 
                          }
                          FILTER(!(CONTAINS(?itemLabel, '1')))
                          FILTER(!(CONTAINS(?itemLabel, '2')))
                          FILTER(!(STRSTARTS(?itemLabel, 'Q')))
                        } GROUP BY ?item ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                        LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "9a70639b-3447-475a-905a-e866a0c98a1c",
                column: "SparqlQuery",
                value: @"SELECT ?answer ?question
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
                          } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) # order by random
                          LIMIT 1
                        } AS %oneState
                        WITH {
                          SELECT ?state ?empty WHERE {
                            INCLUDE %states.
                            FILTER NOT EXISTS { INCLUDE %selectedContinent. }
                          } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) # order by random
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
                        ORDER BY DESC(?question)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d9011896-04e5-4d32-8d3a-02a6d2b0bdb6",
                column: "SparqlQuery",
                value: @"#English kings until 1707
                        SELECT DISTINCT ?question ?answer WHERE {
                          {SELECT DISTINCT ?human ?name ?reignstart ?reignend WHERE {
                            ?human wdt:P31 wd:Q5.      #find humans
                            ?human p:P39 ?memberOfStatement.
                            ?memberOfStatement a wikibase:BestRank;
                                                 ps:P39 wd:Q18810062. # position

                            ?memberOfStatement pq:P580 ?reignstart;
                                               pq:P582 ?reignend. 
                            FILTER (?reignstart >= '1066-12-31T00:00:00Z'^^xsd:dateTime) . #start with William the Conquerer
                            MINUS {?human wdt:P97 wd:Q719039.}

                            SERVICE wikibase:label {
                              bd:serviceParam wikibase:language 'en'.
                              ?human  rdfs:label ?name.
                            }
                          } ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                          LIMIT 4}
                                BIND (?name as ?answer).
                                BIND ('the beginning of their reigning period' as ?question).
                        } ORDER BY ?reignstart");
        }
    }
}
