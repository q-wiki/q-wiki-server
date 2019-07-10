using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AddNewQuestions20190711 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "8273acfe-c278-4cd4-92f5-07dd73a22577", "6c22af9b-2f45-413b-995d-7ee6c61674e5", 2, @"# Which chemical compound has the formula {0}?
                        SELECT DISTINCT ?chemicalCompound ?answer (?chemical_formula AS ?question) ?sitelinks WHERE {
                          ?chemicalCompound wdt:P31 wd:Q11173;
                            wdt:P274 ?chemical_formula;
                            wikibase:sitelinks ?sitelinks.
                          FILTER(?sitelinks >= 50 )
                          ?chemicalCompound rdfs:label ?answer.
                          FILTER((LANG(?answer)) = 'en')
                        }
                        ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW()))))
                        LIMIT 4", "Which chemical compound has the formula {0}?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "bba18c92-47a6-4541-9305-d6453ad8477a", "6c22af9b-2f45-413b-995d-7ee6c61674e5", 0, @"# Sort chemical compounds by melting point
                        SELECT ?answer ?question ?melting WHERE {
                          {
                            SELECT DISTINCT ?answer (AVG(?melting) as ?melting) ?unitLabel WHERE {
                              ?chemicalCompound wdt:P31 wd:Q11173;
                                wikibase:sitelinks ?sitelinks;
                                p:P2101/psv:P2101 [ 
                                  wikibase:quantityUnit ?unit;
                                  wikibase:quantityAmount ?melting;
                                ]
                              FILTER(?sitelinks >= 50 )
                              BIND(wd:Q25267 AS ?unit)
                              ?chemicalCompound rdfs:label ?answer.
                              FILTER((LANG(?answer)) = 'en')
                              ?unit rdfs:label ?unitLabel.
                              FILTER((LANG(?unitLabel)) = 'en')
                            }GROUP BY ?answer ?unitLabel
                            ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW()))))
                            LIMIT 4
                          }
                          BIND('melting point' AS ?question)
                        }
                        ORDER BY (?melting)", "Sort chemical compounds by {0} (ascending)." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "8273acfe-c278-4cd4-92f5-07dd73a22577");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "bba18c92-47a6-4541-9305-d6453ad8477a");
        }
    }
}
