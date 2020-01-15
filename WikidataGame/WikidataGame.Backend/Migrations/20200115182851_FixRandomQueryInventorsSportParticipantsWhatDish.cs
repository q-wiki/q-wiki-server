using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixRandomQueryInventorsSportParticipantsWhatDish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15679f96-27bd-4e88-b367-4eb05e5f6d95"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question (?dishLabel as ?answer) ?image
                            WITH{
                              SELECT DISTINCT ?dish ?dishLabel (SAMPLE(?image)as ?image)
                              WHERE{
                                 ?dish wdt:P279 wd:Q746549.
                                 ?dish wdt:P18 ?image.
                                FILTER NOT EXISTS{?dish wdt:P31 wd:Q19861951}
                                 SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                        ?dish rdfs:label ?dishLabel}
                                 filter(lang(?dishLabel) = 'en').
                              }
                              group by ?dish ?dishLabel
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                            } as %allDishes

                            WITH{
                              SELECT ?dishLabel ?image ?dish
                              WHERE { 
                                INCLUDE %allDishes
                              }
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                              LIMIT 1
                            } as %selectedDish

                            WITH{
                              SELECT ?dishLabel ?dish
                              WHERE{
                                INCLUDE %allDishes
                                FILTER NOT EXISTS{INCLUDE %selectedDish}
                              }
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                              LIMIT 3
                            } as %decoyDishes

                            WHERE{
                              {INCLUDE %selectedDish}
                              UNION
                              {INCLUDE %decoyDishes}
                              BIND('What dish is this?' as ?question)
                            }
                            ORDER BY DESC(?image)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "SparqlQuery",
                value: @"
                        # Which U.S. president's signature is this: {question}
                        SELECT  ?question ?answer ?image
                        WITH{
                          SELECT ?answer ?image
                        WHERE { 
	                        ?president wdt:P39 wd:Q11696.
                            ?president wdt:P109 ?image.
	                        OPTIONAL {
		                        ?president rdfs:label ?answer
		                        filter(lang(?answer) = 'en').
	                        }
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                        } as %allPresidents

                        WITH{
                          SELECT ?image ?answer
                          WHERE{
                             Include %allPresidents
                          }
                          Limit 1
                        } as %selectedPresident
                        WITH{
                          SELECT  ?answer
                          WHERE{
                             Include %allPresidents
                                     FILTER NOT EXISTS {INCLUDE %selectedPresident}
                          }
                          Limit 3
                        } as %decoyPresidents

                        WHERE {
                          {INCLUDE %selectedPresident}
                          UNION
                          {INCLUDE %decoyPresidents}
                          BIND('Whose presidents signature is this?' as ?question)
                        }
                        ORDER BY DESC(?image)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"),
                column: "SparqlQuery",
                value: @"
                            #Which famous landmark is this: {image}
                            SELECT ?question ?answer ?image
                            WITH{
                            SELECT ?tes (CONCAT( ?ans, '(', ?country, ')' ) as ?answer) WHERE {
                              { SELECT DISTINCT(?answer as ?ans)(MAX(?image) as ?tes) ?country WHERE { 
                                ?landmark wdt:P31 / wdt:P279 * wd:Q2319498;
                                    wikibase:sitelinks ?sitelinks;
                                    wdt:P18 ?image;
                                    wdt:P17 ?cntr.
                                ?landmark wdt:P1435 ?type.
                                FILTER(?sitelinks >= 10)
                                    ?landmark rdfs:label ?answer

                                    filter(lang(?answer) = 'en').
                                    ?cntr rdfs:label ?country filter(lang(?country) = 'en').
                                }
                                GROUP BY ?answer ?country
                                ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              }
                            }
                            } as %allMonuments

                            WITH
                            {
                                SELECT ?tes ?answer
                              WHERE
                                {
                                    Include %allMonuments
                              }
                                Limit 1
                            } as %selectedMonument

                            WITH
                            {
                                SELECT ?tes ?answer
                            WHERE
                                {
                                 Include %allMonuments
                                 FILTER NOT EXISTS { INCLUDE %selectedMonument}
                                }
                                Limit 3
                            } as %decoyMonuments

                            WHERE
                            {
                              { INCLUDE %selectedMonument}
                                UNION
                                { INCLUDE %decoyMonuments}
                                Bind(?tes as ?image)
                                BIND('Which famous landmark is this' as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "SparqlQuery",
                value: @"
                            # sort animals by bite force quotient?
                            SELECT DISTINCT ?question (?name as ?answer) ?biteForce

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?item); SEPARATOR=', ')) as ?item) ?biteForce (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', '))as ?name)
                                WHERE{
                                  ?item wdt:P31 wd:Q16521;
                                        wdt:P3485 ?biteForce.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   #?reference ?referenceType wd:Q577.
                                   ?item wdt:P1843 ?name.
       
                                   filter(lang(?name) = 'en').
                                }
                              GROUP BY ?biteForce
                              ORDER BY MD5(CONCAT(STR(?biteForce), STR(NOW())))
                            } as %allTaxons
        
                            WITH{
                              SELECT ?name ?biteForce
                              WHERE{
                               {Include %allTaxons}  
                              }
                              ORDER BY MD5(CONCAT(STR(?biteForce), STR(NOW())))
                              LIMIT 4
                            } as %selectedTaxons

                            WHERE {
                                {Include %selectedTaxons}  
  
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                              } 
                              BIND('order these animals by bite force quotient' as ?question)
                            } 
                            ORDER BY ASC(?biteForce)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question ?answer ?painting ?image
                            WITH{
                            SELECT DISTINCT ?creator ?painting ?image ?paintingLabel
                              WHERE 
                              { 
                                ?painting wdt:P1343 wd:Q66362718.
                                ?painting wdt:P18 ?image.
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                       ?painting rdfs:label ?paintingLabel}
                                filter(lang(?paintingLabel) = 'en').
                              }   
                               ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW())))) 
                               LIMIT 4
                            } as %allPaintings

                            WITH{
                                SELECT DISTINCT ?painting ?paintingLabel ?image
                                     WHERE{
                                          INCLUDE %allPaintings.
                                        }
                               LIMIT 1
                            } as %selectedPainting

                            WITH{
                              SELECT DISTINCT ?painting ?paintingLabel
                                     WHERE{
                                          INCLUDE %allPaintings
                                          FILTER NOT EXISTS{INCLUDE %selectedPainting}
                                        }
                               LIMIT 3
                            } as %decoyPainting

                            WHERE{
                                {INCLUDE %selectedPainting.}
                                UNION
                                {INCLUDE %decoyPainting}

                                BIND(?paintingLabel as ?answer)
                                BIND('What is the name of the painting?' as ?question)
                            }
                              order by DESC(?image)
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
                keyValue: new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"),
                column: "SparqlQuery",
                value: @"
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
                           ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "SparqlQuery",
                value: @"
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
                            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15679f96-27bd-4e88-b367-4eb05e5f6d95"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question (?dishLabel as ?answer) ?image
                            WITH{
                              SELECT DISTINCT ?dish ?dishLabel (SAMPLE(?image)as ?image)
                              WHERE{
                                 ?dish wdt:P279 wd:Q746549.
                                 ?dish wdt:P18 ?image.
                                FILTER NOT EXISTS{?dish wdt:P31 wd:Q19861951}
                                 SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                        ?dish rdfs:label ?dishLabel}
                                 filter(lang(?dishLabel) = 'en').
                              }
                              group by ?dish ?dishLabel
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                            } as %allDishes

                            WITH{
                              SELECT ?dishLabel ?image ?dish
                              WHERE { 
                                INCLUDE %allDishes
                              }
                              LIMIT 1
                            } as %selectedDish

                            WITH{
                              SELECT ?dishLabel ?dish
                              WHERE{
                                INCLUDE %allDishes
                                FILTER NOT EXISTS{INCLUDE %selectedDish}
                              }
                              LIMIT 3
                            } as %decoyDishes

                            WHERE{
                              {INCLUDE %selectedDish}
                              UNION
                              {INCLUDE %decoyDishes}
                              BIND('What dish is this?' as ?question)
                            }
                            ORDER BY DESC(?image)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "SparqlQuery",
                value: @"
                        # Which U.S. president's signature is this: {question}
                        SELECT  ?question ?answer ?image
                        WITH{
                          SELECT ?answer ?image
                        WHERE { 
	                        ?president wdt:P39 wd:Q11696.
                            ?president wdt:P109 ?image.
	                        OPTIONAL {
		                        ?president rdfs:label ?answer
		                        filter(lang(?answer) = 'en').
	                        }
                          }
                          ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                        } as %allPresidents

                        WITH{
                          SELECT ?image ?answer
                          WHERE{
                             Include %allPresidents
                          }
                          Limit 1
                        } as %selectedPresident
                        WITH{
                          SELECT  ?answer
                          WHERE{
                             Include %allPresidents
                                     FILTER NOT EXISTS {INCLUDE %selectedPresident}
                          }
                          Limit 3
                        } as %decoyPresidents

                        WHERE {
                          {INCLUDE %selectedPresident}
                          UNION
                          {INCLUDE %decoyPresidents}
                          BIND('Whose pressidents signature is this?' as ?question)
                        }
                        ORDER BY DESC(?image)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"),
                column: "SparqlQuery",
                value: @"
                            #Which famous landmark is this: {image}
                            SELECT ?question ?answer ?image
                            WITH{
                            SELECT ?tes (CONCAT( ?ans, '(', ?country, ')' ) as ?answer) WHERE {
                              { SELECT DISTINCT(?answer as ? ans)(MAX(?image) as ?tes) ?country WHERE { 
                                ?landmark wdt:P31 / wdt:P279 * wd:Q2319498;
                                    wikibase: sitelinks ?sitelinks;
                                    wdt: P18? image;
                                    wdt: P17? cntr.
                                ?landmark wdt:P1435? type.
                                FILTER(?sitelinks >= 10)
                                    ?landmark rdfs:label? answer

                                    filter(lang(?answer) = 'en').
                                    ?cntr rdfs:label? country filter(lang(?country) = 'en').
                                }
                                GROUP BY ?answer? country
                                ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              }
                            }
                            } as %allMonuments

                            WITH
                            {
                                SELECT ?tes ?answer
                              WHERE
                                {
                                    Include %allMonuments
                              }
                                Limit 1
                            } as %selectedMonument

                            WITH
                            {
                                SELECT ?tes ?answer
                            WHERE
                                {
                                 Include %allMonuments
                                 FILTER NOT EXISTS { INCLUDE %selectedMonument}
                                }
                                Limit 3
                            } as %decoyMonuments

                            WHERE
                            {
                              { INCLUDE %selectedMonument}
                                UNION
                                { INCLUDE %decoyMonuments}
                                Bind(?tes as ?image)
                                BIND('Which famous landmark is this' as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "SparqlQuery",
                value: @"
                            # sort animals by bite force quotient?
                            SELECT DISTINCT ?question (?name as ?answer) ?biteForce

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?empty (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?item); SEPARATOR=', ')) as ?item) ?biteForce (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', '))as?name)
                                WHERE{
                                  ?item wdt:P31 wd:Q16521;
                                        wdt:P3485 ?biteForce.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   #?reference ?referenceType wd:Q577.
                                   ?item wdt:P1843 ?name.
       
                                   filter(lang(?name) = 'en').
                                }
                              GROUP BY ?biteForce ?empty
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                            } as %allTaxons
        
                            WITH{
                              SELECT ?name ?biteForce ?empty
                              WHERE{
                               {Include %allTaxons}  
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              LIMIT 4
                            } as %selectedTaxons

                            WHERE {
                                {Include %selectedTaxons}  
  
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                              } 
                              BIND('order these animals by bite force quotient' as ?question)
                            } 
                            ORDER BY ASC(?biteForce)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question ?answer ?painting ?image
                            WITH{
                            SELECT DISTINCT ?creator ?painting ?image ?paintingLabel
                              WHERE 
                              { 
                                ?painting wdt:P1343 wd:Q66362718.
                                ?painting wdt:P18 ?image.
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                       ?painting rdfs:label ?paintingLabel}
                                filter(lang(?paintingLabel) = 'en').
                              }   
                               ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW())))) 
                               LIMIT 4
                            } as %allPaintings

                            WITH{
                                SELECT DISTINCT ?painting ?paintingLabel ?image
                                     WHERE{
                                          INCLUDE %allPaintings.
                                        }
                               LIMIT 1
                            } as %selectedPainting

                            WITH{
                              SELECT DISTINCT ?painting ?paintingLabel
                                     WHERE{
                                          INCLUDE %allPaintings
                                          FILTER NOT EXISTS{INCLUDE %selectedPainting}
                                        }
                               LIMIT 3
                            } as %decoyPainting

                            WHERE{
                                {INCLUDE %selectedPainting.}
                                UNION
                                {INCLUDE %decoyPainting}

                                BIND(?paintingLabel as ?answer)
                                BIND('What's the name of the painting?' as ?question)
                            }
                              order by DESC(?image)
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
                keyValue: new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"),
                column: "SparqlQuery",
                value: @"
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
                           ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "SparqlQuery",
                value: @"
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
                                 ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                            } as %sports
                        
                            WITH{
                            SELECT DISTINCT ?playerCount 
                                    WHERE {
                                            INCLUDE %sports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                                    LIMIT 4
                                    } AS %fourSports

                            WHERE {
                                   INCLUDE %fourSports.
                                   INCLUDE %sports.
                                           BIND('participating players' as ?question)
                                    }
                            GROUP BY ?playerCount
                            ORDER BY ?playerCount
                            ");
        }
    }
}
