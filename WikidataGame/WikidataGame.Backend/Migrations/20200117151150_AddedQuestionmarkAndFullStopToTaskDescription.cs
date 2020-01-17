using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AddedQuestionmarkAndFullStopToTaskDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "TaskDescription",
                value: "Sort these actors by the number of movies they appeared in.");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

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
                keyValue: new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

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
                keyValue: new Guid("bae2897b-61c9-448e-bedf-8fc069dd62b0"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

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
                            ", "Sort these sports by participating players." });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q5113;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allBirds

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allBirds}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedBird

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allBirds}
                            FILTER NOT EXISTS{Include %selectedBird}
                          } 
                          LIMIT 3
                        } as %decoyBirds

                        WHERE {
                           {INCLUDE %selectedBird} UNION {INCLUDE %decoyBirds}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)
                        ", "Which animal is this?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "TaskDescription",
                value: "Which animal is in the image");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "TaskDescription",
                value: "Which animal is {0}");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "TaskDescription",
                value: "Sort these actors by the number of movies they appeared in");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "TaskDescription",
                value: "Which animal is {0}");

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
                keyValue: new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"),
                column: "TaskDescription",
                value: "Which animal is is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                column: "TaskDescription",
                value: "Which animal is this");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "TaskDescription",
                value: "Which animal is {0}");

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
                keyValue: new Guid("bae2897b-61c9-448e-bedf-8fc069dd62b0"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "TaskDescription",
                value: "Which animal is {0}");

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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                        SELECT DISTINCT ?question (?name as ?answer) ?image

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q5113;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allBirds

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allBirds}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedBird

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allBirds}
                            FILTER NOT EXISTS{Include %selectedBird}
                          } 
                          LIMIT 3
                        } as %decoyBirds

                        WHERE {
                           {INCLUDE %selectedBird} UNION {INCLUDE %decoyBirds}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)
                        ", "Which animal is in the image?" });
        }
    }
}
