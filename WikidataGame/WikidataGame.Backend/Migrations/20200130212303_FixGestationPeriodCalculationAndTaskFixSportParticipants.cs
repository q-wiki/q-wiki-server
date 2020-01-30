using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixGestationPeriodCalculationAndTaskFixSportParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: new Guid("6ba60225-d7be-4c97-ad19-eaf895a14734"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (only marsupials, artiodactyla, primates)
                            # Sort animals by gestation time (days)?
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 


                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25329.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allArtiodactyla

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Primates.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allPrimates

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allArtiodactyla}
                                  UNION
                                  {INCLUDE %allPrimates}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*7) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9d630c39-6606-49ff-8cef-b34695d8ed91"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (only primates, rodents, carnivora)
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q7380.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allPrimates

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allPrimates}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*7) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime             
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9fd3f504-eb96-42f3-91bc-606a17759e45"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (only rodentia, carnivora, marsupial)
                            # Sort animals by gestation time (days)?
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25336.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allMarsupial

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allMarsupial}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*7) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime              
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e9ab8641-23b7-428f-b8d6-d96ae7d17e6f"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (onlycarnivores, artiodactyla and rodents)
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25329.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allArtiodactyla

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allArtiodactyla}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*7) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by maximum amount of participating players (ascending).");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "9d406ede-473a-4577-abf0-acfcf505f649");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6ba60225-d7be-4c97-ad19-eaf895a14734"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (only marsupials, artiodactyla, primates)
                            # Sort animals by gestation time (days)?
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 


                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25329.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allArtiodactyla

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Primates.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allPrimates

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allArtiodactyla}
                                  UNION
                                  {INCLUDE %allPrimates}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9d630c39-6606-49ff-8cef-b34695d8ed91"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (only primates, rodents, carnivora)
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q7380.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allPrimates

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allPrimates}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime             
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9fd3f504-eb96-42f3-91bc-606a17759e45"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (only rodentia, carnivora, marsupial)
                            # Sort animals by gestation time (days)?
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25336.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allMarsupial

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allMarsupial}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime              
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e9ab8641-23b7-428f-b8d6-d96ae7d17e6f"),
                column: "SparqlQuery",
                value: @"
                            # Sort animals by gestation time (days)? (onlycarnivores, artiodactyla and rodents)
                            SELECT DISTINCT ?question (?name as ?answer) ?gestationTime ?item

                            #seperated animals in variables befor unionizing for performance/quicker response
 
                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25306.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q25329.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allArtiodactyla

                            WITH{
                                SELECT DISTINCT ?item ?gestation ?gestationUnit ?name
                                WHERE{
                                  ?item wdt:P171* wd:Q10850.
                                  ?item p:P3063/psv:P3063 [wikibase:quantityAmount ?gestation; wikibase:quantityUnit ?gestationUnit].
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350;
                                           wdt:P171* wd:Q23038290.
                                   }
                                   ?item wdt:P1843 ?name.
                                   filter(lang(?name) = 'en').
                                }
                              order by ?item ?gestation ?gestationUnit
                            } as %allRodentia

                            WITH{
                                Select *
                                WHERE{
                                  {INCLUDE %allCarnivora}  
                                  UNION
                                  {INCLUDE %allArtiodactyla}
                                  UNION
                                  {INCLUDE %allRodentia}
                                }
                            } as %allMammal

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER(?gestationUnit = wd:Q5151) 
                                BIND((?gestation*30) as ?gestationTime)
                              }
                            } as %monthsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal. 
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER(?gestationUnit = wd:Q23387) 
                                BIND((?gestation*14) as ?gestationTime)
                              }
                            } as %weeksInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER(?gestationUnit = wd:Q577) 
                                BIND((?gestation*365) as ?gestationTime)
                              }
                            } as %yearsInDays

                            WITH{
                              SELECT ?item ?gestationTime ?name
                              WHERE{
                                include %allMammal.
                                FILTER NOT EXISTS {Include %monthsInDays}
                                FILTER NOT EXISTS {Include %weeksInDays}
                                FILTER NOT EXISTS {Include %yearsInDays}
                                FILTER(?gestationUnit = wd:Q573) 
                                BIND((?gestation) as ?gestationTime)
                              }
                            } as %allDays
                            WITH{
                              SELECT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name) 
                              (SAMPLE(GROUP_CONCAT(DISTINCT ?item; SEPARATOR=', ')) as ?item)  
                              ?gestationTime
                              WHERE {
                                {Include %monthsInDays}  
                                UNION
                                {Include %weeksInDays}  
                                UNION
                                {Include %yearsInDays}  
                                UNION
                                {Include %allDays.}  
                              } 
                              GROUP BY ?gestationTime
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 4
                            } as %selectedAnimals

                             WHERE {
                              Include %selectedAnimals.
                              BIND('Order these animals by gestation period' as ?question)
                             }
                            ORDER BY ?gestationTime
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by maximum amount ofparticipating players (ascending).");
        }
    }
}
