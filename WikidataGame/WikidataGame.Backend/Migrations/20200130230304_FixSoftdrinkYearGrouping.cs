using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixSoftdrinkYearGrouping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "a04e009d-0b01-480e-b575-d15311ac6aa0");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "SparqlQuery",
                value: @"
                            #Structure is important to get only one drink of a inception year and avoid duplicates
                            SELECT DISTINCT ?question  ?answer ?year
                            WITH{
                                    SELECT (Sample(GROUP_CONCAT(DISTINCT sample(?softDrink); SEPARATOR=', ')) AS ?softDrink) (Sample(GROUP_CONCAT( DISTINCT sample(?softDrinkLabel); SEPARATOR=', ')) AS ?softDrinkLabel) ?year
                                    WHERE {
                                         ?softDrink (wd:wd31|wdt:P279)* wd:Q147538.
                                         ?softDrink wdt:P571 ?inception.
                                         Filter(?softDrink != wd:Q180289)
                                         ?softDrink rdfs:label ?softDrinkLabel
                                                                   #makes sure to get only known drinks in germany by checking if item has a german label
                                         filter langMatches(lang(?softDrinkLabel), 'de')
                                         SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                                 ?softDrinkLabel rdfs:label ?answer.
                                                                               }
                                         BIND(Year(?inception) as ?year)
                                    }
                                    group by ?inception
                                    ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))                
                            } as %allSoftDrinks

                            WITH{
                              SELECT ?softDrinkLabel ?year ?empty
                              WHERE{
                                INCLUDE %allSoftDrinks.
                              }
                              ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))
                              LIMIT 4
                            } as %selectedSoftDrink

                            WHERE{
                              INCLUDE %selectedSoftDrink
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                      ?softDrinkLabel rdfs:label ?answer.
                                                     }
                              BIND('Order Softdrinks by inception' as ?question)       
                            }
                            order by ?year
                            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "bf3f3567-ea72-487c-adcc-c9761eb3bc6c");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "SparqlQuery",
                value: @"
                            #Structure is important to get only one drink of a inception year and avoid duplicates
                            SELECT DISTINCT ?question  ?answer ?year
                            WITH{
                                    SELECT (Sample(GROUP_CONCAT(DISTINCT sample(?softDrink); SEPARATOR=', ')) AS ?softDrink) (Sample(GROUP_CONCAT( DISTINCT sample(?softDrinkLabel); SEPARATOR=', ')) AS ?softDrinkLabel) (year(?inception) as ?year) 
                                    WHERE {
                                         ?softDrink (wd:wd31|wdt:P279)* wd:Q147538.
                                         ?softDrink wdt:P571 ?inception.
                                         Filter(?softDrink != wd:Q180289)
                                         ?softDrink rdfs:label ?softDrinkLabel
                                                                   #makes sure to get only known drinks in germany by checking if item has a german label
                                         filter langMatches(lang(?softDrinkLabel), 'de')
                                         SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                                 ?softDrinkLabel rdfs:label ?answer.
                                                                               }
                                         BIND(Year(?inception) as ?year)
                                    }
                                    group by ?inception
                                    ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))                
                            } as %allSoftDrinks

                            WITH{
                              SELECT ?softDrinkLabel ?year ?empty
                              WHERE{
                                INCLUDE %allSoftDrinks.
                              }
                              ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))
                              LIMIT 4
                            } as %selectedSoftDrink

                            WHERE{
                              INCLUDE %selectedSoftDrink
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                      ?softDrinkLabel rdfs:label ?answer.
                                                     }
                              BIND('Order Softdrinks by inception' as ?question)       
                            }
                            order by ?year
                            ");
        }
    }
}
