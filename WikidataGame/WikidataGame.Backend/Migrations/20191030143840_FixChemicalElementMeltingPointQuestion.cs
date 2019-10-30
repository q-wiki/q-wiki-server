using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixChemicalElementMeltingPointQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "bba18c92-47a6-4541-9305-d6453ad8477a",
                column: "SparqlQuery",
                value: @"
                        # Sort these chemical elements by their melting point
                        # ?question what gets used as a placeholder value
                        # ?answer is one answer option
                        # ?value is ignored
                        SELECT ?question ?answer ?value
                        
                        # select all chemical elements with melting point
                        WITH {
                          SELECT DISTINCT ?question ?answer ?value WHERE {
                            # Select chemical elements
                            ?element wdt:P31 wd:Q11344;
                                     # select melting point and unit
                                     p:P2101/psv:P2101 [ 
                                       wikibase:quantityUnit ?unit;
                                       wikibase:quantityAmount ?value;
                                     ].
                            # use only degrees celsius
                            BIND(wd:Q25267 AS ?unit)
                            BIND('melting point' AS ?question)
                            ?element rdfs:label ?answer.
                            FILTER((LANG(?answer)) = 'en')
                          }
                          ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                          LIMIT 4
                        } AS %items
                        
                        WHERE {
                          INCLUDE %items
                        }
                        
                        # the final results must be sorted ascending
                        ORDER BY ?value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "bba18c92-47a6-4541-9305-d6453ad8477a",
                column: "SparqlQuery",
                value: @"
                        # Sort these chemical elements by their melting point
                        # ?question what gets used as a placeholder value
                        # ?answer is one answer option
                        # ?value is ignored
                        SELECT ?question ?answer ?value
                        
                        # select all chemical elements with melting point
                        WITH {
                          SELECT DISTINCT ?question ?answer ?value WHERE {
                            # Select chemical elements
                            ?element wdt:P31 wd:Q11344;
                                     # select melting point and unit
                                     p:P2101/psv:P2101 [ 
                                       wikibase:quantityUnit ?unit;
                                       wikibase:quantityAmount ?value;
                                     ].
                            # use only degrees celsius
                            BIND(wd:Q25267 AS ?unit)
                            BIND('melting point' AS ?question)
                            ?element rdfs:label ?answer.
                            FILTER((LANG(?answer)) = 'en')
                          }
                          ORDER BY MD5(CONCAT(?answer, NOW()))
                          LIMIT 4
                        } AS %items
                        
                        WHERE {
                          INCLUDE %items
                        }
                        
                        # the final results must be sorted ascending
                        ORDER BY ?value");
        }
    }
}
