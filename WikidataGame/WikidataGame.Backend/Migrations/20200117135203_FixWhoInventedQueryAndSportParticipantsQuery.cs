using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixWhoInventedQueryAndSportParticipantsQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b95607f9-8cd6-48e8-bc99-c9c305e812be"),
                column: "SparqlQuery",
                value: @"
                                SELECT DISTINCT ?question ?answer 
                                WITH{
                                 SELECT DISTINCT ?inventorLabel (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?itemLabel); SEPARATOR=',')) AS ?itemLabel)
                                  WHERE 
                                    { 
                                      ?inventor wdt:P31 wd:Q5; wdt:P106 wd:Q205375.
                                      ?item wdt:P61 ?inventor.
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                        ?item rdfs:label ?itemLabel.
                                        ?inventor rdfs:label ?inventorLabel
                                      }
                                      filter(lang(?inventorLabel) = 'en').
                                    }
                                  group by ?inventorLabel
                                  ORDER BY (MD5(CONCAT(STR(?inventorLabel), STR(NOW())))) 
                                 } as %allInventors

                                WITH{
                                 SELECT ?inventorLabel ?itemLabel
                                  WHERE 
                                    { 
                                     INCLUDE %allInventors
                                    }
                                   ORDER BY (MD5(CONCAT(STR(?inventorLabel), STR(NOW())))) 
                                   LIMIT 1
                                } as %selectedInventor

                                WITH{
                                 SELECT Distinct ?inventorLabel
                                  WHERE 
                                    { 
                                     INCLUDE %allInventors.
                                     FILTER NOT EXISTS {INCLUDE %selectedInventor}
                                    }
                                   ORDER BY (MD5(CONCAT(STR(?inventorLabel), STR(NOW())))) 
                                   LIMIT 3
                                } as %decoyInventors

                                WHERE{
                                  {INCLUDE %selectedInventor}
                                  UNION
                                  {INCLUDE %decoyInventors}
                                  BIND(?inventorLabel as ?answer)
                                  Bind(?itemLabel as ?question)
                                }

                                Order by DESC(?question)
                                ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                columns: new[] { "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { 0, @"
                            SELECT (Sample(GROUP_CONCAT( DISTINCT ?question; SEPARATOR=', ')) AS ?question) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?answer); SEPARATOR=', ')) AS ?answer) 
                            ?playerCount
                            WITH {
                                 SELECT ?sport ?playerCount ?sportLabel ?answer
                                 WHERE {
	                                   ?sport wdt:P31 wd:Q31629.
                                       ?sport wdt:P1873 ?playerCount.
                                       SERVICE wikibase:label {
                                              bd:serviceParam wikibase:language 'en'.
                                              ?sport rdfs:label ?answer.
                                       }
                                 }
                                 ORDER BY MD5(CONCAT(STR(?playerCount), STR(NOW())))
                            } as %sports
                        
                            WITH{
                            SELECT DISTINCT ?playerCount 
                                    WHERE {
                                            INCLUDE %sports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?playerCount), STR(NOW())))
                                    LIMIT 4
                                    } AS %fourSports

                            WHERE {
                                   INCLUDE %fourSports.
                                   INCLUDE %sports.
                                   BIND('participating players' as ?question)
                                    }
                            GROUP BY ?playerCount
                            ORDER BY ?playerCount
                            ", "Sort sports by participating players?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b95607f9-8cd6-48e8-bc99-c9c305e812be"),
                column: "SparqlQuery",
                value: @"
                                SELECT DISTINCT ?question ?answer 

                                WITH{
                                  SELECT DISTINCT ?item ?itemLabel ?inventor ?inventorLabel
                                  WHERE 
                                    { 
                                      ?inventor wdt:P31 wd:Q5; wdt:P106 wd:Q205375.
                                      ?item wdt:P61 ?inventor.
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                        ?item rdfs:label ?question.
                                        ?inventor rdfs:label ?answer.
                                                         }
                                    }
                                  ORDER BY (MD5(CONCAT(STR(?inventor), STR(NOW())))) 
                                  LIMIT 1
                                 } as %selectedInventor

                                WITH{
                                    SELECT ?inventor ?inventorLabel
                                    WHERE 
                                    { 
                                      ?inventor (wdt:P31|wdt:P106) wd:Q205375.
                                      ?item wdt:P61 ?inventor.
                                      FILTER NOT EXISTS {INCLUDE %selectedInventor}
                                    } 
                                   ORDER BY (MD5(CONCAT(STR(?inventor), STR(NOW())))) 
                                  LIMIT 3
                                } as %decoyInventors

                                WHERE{
                                  {INCLUDE %selectedInventor}
                                  UNION
                                  {INCLUDE %decoyInventors}
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                        ?item rdfs:label ?question.
                                        ?inventor rdfs:label ?answer.
                                                         }
                                }

                                Order by DESC(?question)
                                ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                columns: new[] { "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { 2, @"
                            SELECT (Sample(GROUP_CONCAT( DISTINCT ?question; SEPARATOR=', ')) AS ?question) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?answer); SEPARATOR=', ')) AS ?answer) 
                            ?playerCount
                            WITH {
                                 SELECT ?sport ?playerCount ?sportLabel ?answer
                                 WHERE {
	                                   ?sport wdt:P31 wd:Q31629.
                                       ?sport wdt:P1873 ?playerCount.
                                       SERVICE wikibase:label {
                                                                 bd:serviceParam wikibase:language 'en'.
                                                                 ?sport rdfs:label ?answer.
                                       }
                                 }
                                 ORDER BY MD5(CONCAT(STR(?playerCount), STR(NOW())))
                            } as %sports
                        
                            WITH{
                            SELECT DISTINCT ?playerCount 
                                    WHERE {
                                            INCLUDE %sports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?playerCount), STR(NOW())))
                                    LIMIT 4
                                    } AS %fourSports

                            WHERE {
                                   INCLUDE %fourSports.
                                   INCLUDE %sports.
                                           BIND('participating players' as ?question)
                                    }
                            GROUP BY ?playerCount
                            ORDER BY ?playerCount
                            ", "Who is the trainer of {0}?" });
        }
    }
}
