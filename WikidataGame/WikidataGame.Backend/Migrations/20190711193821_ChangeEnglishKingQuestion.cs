using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class ChangeEnglishKingQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
