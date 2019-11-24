using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class RemoveUKThroneSuccessionQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d9011896-04e5-4d32-8d3a-02a6d2b0bdb6",
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                         # US presidents by start of their presidency
                         # ?question contains question template value
                         # ?answer contains the label for the different answer options
                         # ?firstElectionPeriod is ignored by the server but used to sort the final result
                         SELECT ?question ?answer ?firstElectionPeriod
                         
                         WITH {
                             # The MIN(...)-thing for ?startTime is a trick to convert a date like 01-12-2012 (in format DD-MM-YYYY) into an integer
                             # that's formatted like YYYYMMDD and that we can easily sort :)
                             SELECT ?person ?personLabel (MIN(YEAR(?startTime) * 100 * 100 + MONTH(?startTime) * 100 + DAY(?startTime)) AS ?firstElectionPeriod) WHERE {
                               # start by looking at real humans only because we're not interested in Lex Luthor
                               ?person wdt:P31 wd:Q5.
                               ?person p:P39 ?usPresident.
                               ?usPresident rdf:type wikibase:BestRank;
                                 ps:P39 wd:Q11696;
                                 pq:P582 ?endTime; # <- because this is a history question, we only look at presidencies that ended
                                 pq:P580 ?startTime.
                               
                               # If we don't have an end time, we use the current time as a default value
                               # BIND(IF(!(BOUND(?endTime)), NOW(), ?endTime) AS ?endTime)
                               SERVICE wikibase:label {
                                 bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'.
                                 ?person rdfs:label ?personLabel.
                               }
                             }
                             GROUP BY ?person ?personLabel
                             # Shuffle the results
                             ORDER BY MD5(CONCAT(STR(?person),  STR(NOW())))
                             LIMIT 4
                         } AS %presidents
                         
                         WHERE {
                             INCLUDE %presidents
                             BIND(?personLabel AS ?answer)
                             BIND('their first election period' AS ?question)
                         }
                         
                         ORDER BY ?firstElectionPeriod
                       ", "Sort these US presidents by {0} (ascending)." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d9011896-04e5-4d32-8d3a-02a6d2b0bdb6",
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"#English kings until 1707
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
                        ORDER BY (?reignstart)", "Sort these English kings by {0} (ascending)." });
        }
    }
}
