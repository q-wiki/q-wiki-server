using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AddHistoryCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "f9c52d1a-9315-423d-a818-94c1769fffe5", "History" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "14d93797-c61c-4415-b1ed-359d180237ff",
                column: "TaskDescription",
                value: "Which of these moons belongs to {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "2ed01768-9ab6-4895-8cbf-09dbc6f957e0",
                column: "TaskDescription",
                value: "Sort planets by {0} (ascending).");

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
                column: "TaskDescription",
                value: "Which country is not a basin country of the Caribbean Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "9a70639b-3447-475a-905a-e866a0c98a1c",
                column: "TaskDescription",
                value: "Which country is a part of {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a4a7289a-3053-4ad7-9c60-c75a18305243",
                column: "TaskDescription",
                value: "Sort planets by {0} (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a6a470de-9efb-4fde-9388-6eb20f2ff1f4",
                column: "TaskDescription",
                value: "Which country is not a basin country of the Mediterranean Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "bc7a22ee-4985-44c3-9388-5c7dd6b8762e",
                column: "TaskDescription",
                value: "Sort countries by {0} (ascending).");

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[,]
                {
                    { "d9011896-04e5-4d32-8d3a-02a6d2b0bdb6", "f9c52d1a-9315-423d-a818-94c1769fffe5", 0, @"#English kings until 1707
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
                                        } ORDER BY ?reignstart", "Sort these English kings by {0} (ascending)." },
                    { "909182d1-4ae6-46ea-bd9b-8c4323ea53fa", "f9c52d1a-9315-423d-a818-94c1769fffe5", 0, @"# sort EU countries by the date they joined
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
                                        ORDER BY ?date", "Sort the countries by {0} (ascending)." },
                    { "86b64102-8074-4c4e-8f3e-71a0e52bb261", "f9c52d1a-9315-423d-a818-94c1769fffe5", 2, @"# German Chancellors
                                        SELECT ?answer (CONCAT(STR(?startYear), ' - ', STR(?endYear)) AS ?question) WHERE {
                                          ?person p:P39 ?Bundeskanzler.
                                          ?Bundeskanzler ps:P39 wd:Q4970706;
                                            pq:P580 ?start.
                                          OPTIONAL { ?Bundeskanzler pq:P582 ?end. }
                                          BIND(YEAR(?start) AS ?startYear)
                                          BIND(IF(!(BOUND(?end)), 'today', YEAR(?end)) AS ?endYear)
                                          SERVICE wikibase:label {
                                            bd:serviceParam wikibase:language 'en'.
                                            ?person rdfs:label ?answer.
                                          }
                                        }
                                        ORDER BY (MD5(CONCAT(STR(?person), STR(NOW()))))
                                        LIMIT 4", "Who was Federal Chancellor of Germany in {0}?" },
                    { "d135088c-e062-4016-8eb4-1d68c72915ea", "f9c52d1a-9315-423d-a818-94c1769fffe5", 2, @"# empires and colonies
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
                                        } ORDER BY DESC(?empire)", "Which colony belonged to the {0}?" },
                    { "0d218830-55d2-4d66-8d8f-d402514e9202", "f9c52d1a-9315-423d-a818-94c1769fffe5", 2, @"# wars of the 20th century
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
                                        LIMIT 4", "Which of these wars started in {0}?" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "0d218830-55d2-4d66-8d8f-d402514e9202");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "86b64102-8074-4c4e-8f3e-71a0e52bb261");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "909182d1-4ae6-46ea-bd9b-8c4323ea53fa");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d135088c-e062-4016-8eb4-1d68c72915ea");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d9011896-04e5-4d32-8d3a-02a6d2b0bdb6");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "f9c52d1a-9315-423d-a818-94c1769fffe5");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "14d93797-c61c-4415-b1ed-359d180237ff",
                column: "TaskDescription",
                value: "Which of these moons belongs to planet {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "2ed01768-9ab6-4895-8cbf-09dbc6f957e0",
                column: "TaskDescription",
                value: "Sort planets by {0} (ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "46679c4f-ef97-445d-9a70-d95a5337720f",
                column: "TaskDescription",
                value: "Which country is no basin country of the Baltic Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "4f6c477e-7025-44b4-a3b0-f3ebd8902902",
                column: "TaskDescription",
                value: "Which country is no basin country of the Caribbean Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "9a70639b-3447-475a-905a-e866a0c98a1c",
                column: "TaskDescription",
                value: "Which country is a part of continent {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a4a7289a-3053-4ad7-9c60-c75a18305243",
                column: "TaskDescription",
                value: "Sort planets by {0} (ascending)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a6a470de-9efb-4fde-9388-6eb20f2ff1f4",
                column: "TaskDescription",
                value: "Which country is no basin country of the Mediterranean Sea?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "bc7a22ee-4985-44c3-9388-5c7dd6b8762e",
                column: "TaskDescription",
                value: "Sort countries by {0} (ascending)");
        }
    }
}
