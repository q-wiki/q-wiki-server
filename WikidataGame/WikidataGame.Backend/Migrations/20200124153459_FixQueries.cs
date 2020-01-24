using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixQueries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "847fd22d-3644-4da9-8d8b-3ffc06b9a9e7");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                                SELECT DISTINCT ?question (?name as ?answer)

                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      {?item wdt:P171* wd:Q150760.}
                                      UNION
                                      {?item wdt:P171* wd:Q840552}
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                      ?item wdt:P141 ?status.
                                      FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                      }
                                    }
                                  GROUP BY ?item
                                  ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                  LIMIT 800
                                } as %allFishes

                                WITH {
                                  SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status) ?name WHERE {
                                    {Include %allFishes}
                                    ?item wdt:P141 ?s.
                                    ?s wdt:P279 wd:Q515487.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                      ?s  rdfs:label ?sLabel.
                                    } 
                                    ?s wdt:P279 wd:Q515487.
                                  } 
                                  group by ?name
                                } as %endangered

                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allFishes}
                                    BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %endangered.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                      } 
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WITH{
                                  SELECT *
                                  WHERE{
                                    include %endangered 
                                  }
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                                  LIMIT 1
                                } as %selectedSpecies

                                WHERE {
                                  {INCLUDE %selectedSpecies} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15679f96-27bd-4e88-b367-4eb05e5f6d95"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question (?dishLabel as ?answer)
                            WITH{
                              SELECT DISTINCT ?dish ?dishLabel (SAMPLE(?image)as ?image)
                              WHERE{
                                 ?dish wdt:P279 wd:Q746549.
                                 ?dish wdt:P18 ?image.
                                 FILTER NOT EXISTS{
                                   {?dish wdt:P31 wd:Q19861951}
                                   UNION
                                   {?dish wdt:P31 wd:Q8148}
                                 }
                                 FILTER NOT EXISTS{FILTER(?dish = wd:Q748611)}
                                 SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                        ?dish rdfs:label ?dL}
                                 filter(lang(?dL) = 'en').
                                 BIND(CONCAT(UCASE(SUBSTR(?dL, 1, 1)), SUBSTR(?dL, 2)) as ?dishLabel)
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
                              BIND(?image as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"),
                column: "SparqlQuery",
                value: @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #marsupials
                                  {?item wdt:P171* wd:Q25336;}
                                  UNION
                                  #primates
                                  {?item wdt:P171* wd:Q7380;}
                                  ?item wdt:P18 ?image;
                                        wdt:P1843 ?name;
                                  filter(lang(?name) = 'en').
                                  FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                  }
                                }
                              GROUP BY ?item
                            } as %allAnimals

                            WITH{
                                SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                WHERE{
                                  {include %allAnimals}
                                  ?item wdt:P18 ?img.
                                }
                              GROUP BY ?name
                            } as %allImages

                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allImages}
                              }  
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allImages}
                                 BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %selectedAnimal.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                              } 
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                              {INCLUDE %selectedAnimal} UNION {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #artiadactyl
                                  {?item wdt:P171* wd:Q25329.}
                                  UNION
                                  #rodents
                                  {?item wdt:P171* wd:Q10850.}
                                  UNION
                                  {?item wdt:p171* wd:Q25306}
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 800
                            } as %allAnimals

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status)  ?name
                              WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                 bd:serviceParam wikibase:language 'en'.
                                  ?s  rdfs:label ?sLabel.
                                } 
                                ?s wdt:P279 wd:Q515487.
                              } 
                              GROUP BY ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?status.
                                BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %endangered.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 3
                            } as %noproblem

                            WITH{
                              SELECT *
                              WHERE{
                                include %endangered 
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} 
                               UNION 
                              {INCLUDE %noproblem}
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                column: "MiniGameType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3609a5f7-c90a-4ecf-a713-0224fa8a4215"),
                column: "TaskDescription",
                value: "Where is the dish {0} from?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"),
                column: "SparqlQuery",
                value: @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #rodents
                                  {?item wdt:P171* wd:Q10850;}
                                  UNION
                                  #artiodactyla
                                  {?item wdt:P171* wd:Q25329;}
                                  ?item wdt:P18 ?image;
                                        wdt:P1843 ?name;
                                  filter(lang(?name) = 'en').
                                  FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                  }
                                }
                              GROUP BY ?item
                            } as %allAnimals

                            WITH{
                                SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                WHERE{
                                  {include %allAnimals}
                                  ?item wdt:P18 ?img.
                                }
                              GROUP BY ?name
                            } as %allImages

                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allImages}
                              }  
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allImages}
                                 BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %selectedAnimal.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                              } 
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                              {INCLUDE %selectedAnimal} UNION {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"),
                column: "SparqlQuery",
                value: @"
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q122422.
                                      ?item wdt:P18 ?image;
                                            wdt:P1843 ?name;
                                      filter(lang(?name) = 'en').
                                      FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                      }
                                    }
                                  GROUP BY ?item
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                                  LIMIT 400
                                } as %allReptiles

                                WITH{
                                    SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                    WHERE{
                                      {include %allReptiles}
                                      ?item wdt:P18 ?img.
                                    }
                                  GROUP BY ?name
                                } as %allImages

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allImages}
                                  }  
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedReptile

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allImages}
                                     BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %selectedReptile.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                    } 
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 3
                                } as %decoyReptiles

                                WHERE {
                                  {INCLUDE %selectedReptile} UNION {INCLUDE %decoyReptiles}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #rodents
                                  {?item wdt:P171* wd:Q10850.}
                                  UNION
                                  #carnivores
                                  {?item wdt:P171* wd:Q25306.}
                                  UNION
                                  #marsupials
                                  {?item wdt:p171* wd:Q25336}
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              #LIMIT 800
                            } as %allAnimals

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status)  ?name
                              WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                 bd:serviceParam wikibase:language 'en'.
                                  ?s  rdfs:label ?sLabel.
                                } 
                                ?s wdt:P279 wd:Q515487.
                              } 
                              GROUP BY ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?status.
                                BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %endangered.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 3
                            } as %noproblem

                            WITH{
                              SELECT *
                              WHERE{
                                include %endangered 
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} 
                               UNION 
                              {INCLUDE %noproblem}
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "TaskDescription",
                value: "Sort these softdrinks by release.");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"),
                column: "SparqlQuery",
                value: @"
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      {?item wdt:P171* wd:Q150760.}
                                      UNION
                                      {?item wdt:P171* wd:Q840552}
                                      ?item wdt:P18 ?image;
                                            wdt:P1843 ?name;
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                  ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                  LIMIT 400         
                                } as %allFishes

                                WITH{
                                    SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                    WHERE{
                                      {include %allFishes}
                                      ?item wdt:P18 ?img.
                                    }
                                  GROUP BY ?name
                                } as %allImages

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allImages}
                                  }  
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedFish

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allImages}
                                     BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %selectedFish.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                    } 
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 3
                                } as %decoyFishes

                                WHERE {
                                  {INCLUDE %selectedFish} UNION {INCLUDE %decoyFishes}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("50120520-4441-48c1-b387-1c923a038194"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                              SELECT DISTINCT ?question (?name as ?answer)

                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q122422.
                                      ?item wdt:P1843 ?name.
                                      ?item wdt:P141 ?status.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                  LIMIT 200
                                } as %allReptiles

                                WITH {
                                  SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status) ?name WHERE {
                                    {Include %allReptiles}
                                    ?item wdt:P141 ?s.
                                    ?s wdt:P279 wd:Q515487.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                      ?s  rdfs:label ?sLabel.
                                    } 
                                    ?s wdt:P279 wd:Q515487.
                                  } 
                                   group by ?name
                                } as %endangered

                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allReptiles}
                                    ?item wdt:P141 ?status.
                                    BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %endangered.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                      } 
                                      VALUES ?status {wd:Q211005}.
                                    }
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                                  LIMIT 3
                                } as %noProblem

                                WITH{
                                  SELECT *
                                  WHERE{
                                    include %endangered 
                                  }
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                                  LIMIT 1
                                } as %selectedSpecies

                                WHERE {
                                  {INCLUDE %selectedSpecies} UNION {INCLUDE %noProblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"),
                column: "SparqlQuery",
                value: @"
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      #artiodactyla
                                      {?item wdt:P171* wd:Q25329;}
                                      UNION
                                      #primates
                                      {?item wdt:P171* wd:Q7380;}
                                      UNION
                                      #carnivores
                                      {?item wdt:P171* wd:Q25306;}
                                      ?item wdt:P18 ?image;
                                            wdt:P1843 ?name;
                                      filter(lang(?name) = 'en').
                                      FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                      }
                                    }
                                  GROUP BY ?item
                                  ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                  LIMIT 400        
                                } as %allAnimals

                                WITH{
                                    SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                    WHERE{
                                      {include %allAnimals}
                                      ?item wdt:P18 ?img.
                                    }
                                  GROUP BY ?name
                                } as %allImages

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allImages}
                                  }  
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allImages}
                                     BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %selectedAnimal.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                    } 
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                  {INCLUDE %selectedAnimal} UNION {INCLUDE %decoyAnimals}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                           SELECT DISTINCT ?question (?name as ?answer)

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  {?item wdt:P171* wd:Q21736.}
                                  UNION
                                  {?item wdt:P171* wd:Q853058.}
                                  UNION
                                  {?item wdt:P171* wd:Q31431}
      
                                  ?item wdt:P1843 ?name.
                                  ?item wdt:P141 ?status.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                            } as %allBirds

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status) ?name WHERE {
                                {Include %allBirds}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                  ?s  rdfs:label ?sLabel.
                                } 
                                ?s wdt:P279 wd:Q515487.
                              } 
                              group by ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allBirds}
                                ?item wdt:P141 ?status.
                                #filter common names across multiple species that might be endangered
                                BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                      include %endangered.
                                      BIND(lcase(?name) as ?caseName)}
                                    }
                                }
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              LIMIT 3
                            } as %noproblem

                            WITH{
                              select * 
                              where{
                                 include %endangered
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} UNION {INCLUDE %noproblem}
                              BIND(?status as ?question)
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #artiodactyla
                                  {?item wdt:P171* wd:Q25329.}
                                  UNION
                                  #carnivores
                                  {?item wdt:P171* wd:Q25306.}
                                  UNION
                                  #primates
                                  {?item wdt:p171* wd:Q7380}
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              #LIMIT 800
                            } as %allAnimals

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status)  ?name
                              WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                 bd:serviceParam wikibase:language 'en'.
                                  ?s  rdfs:label ?sLabel.
                                } 
                                ?s wdt:P279 wd:Q515487.
                              } 
                              GROUP BY ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?status.
                                BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %endangered.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 3
                            } as %noproblem

                            WITH{
                              SELECT *
                              WHERE{
                                include %endangered 
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} 
                               UNION 
                              {INCLUDE %noproblem}
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"),
                column: "SparqlQuery",
                value: @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #marsupials
                                  {?item wdt:P171* wd:Q25336;}
                                  UNION
                                  #carnivores
                                  {?item wdt:P171* wd:Q1194816;}
                                  ?item wdt:P18 ?image;
                                        wdt:P1843 ?name;
                                  filter(lang(?name) = 'en').
                                  FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                  }
                                }
                              GROUP BY ?item
                            } as %allAnimals

                            WITH{
                                SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                WHERE{
                                  {include %allAnimals}
                                  ?item wdt:P18 ?img.
                                }
                              GROUP BY ?name
                            } as %allImages

                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allImages}
                              }  
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allImages}
                                 BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %selectedAnimal.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                              } 
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                              {INCLUDE %selectedAnimal} UNION {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"),
                column: "SparqlQuery",
                value: @"SELECT ?answer ?question
                            WITH {
                              SELECT DISTINCT ?state ?continent ?stateLabel ?continentLabel WHERE {
                                ?state wdt:P31/wdt:P279* wd:Q3624078;
                                     p:P463 ?memberOfStatement.
                                ?memberOfStatement a wikibase:BestRank;
                                                   ps:P463 wd:Q1065.
                                MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                MINUS { ?state wdt:P576|wdt:P582 ?end. }
                                ?state p:P30 ?continentStatement.
                                ?continentStatement a wikibase:BestRank;
                                                  ps:P30 ?continent.
                                VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                              }
                            } AS %states
                            WITH {
                              SELECT ?state ?continent WHERE {
                                INCLUDE %states.
                                {
                                  SELECT DISTINCT ?continent WHERE {
                                    VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                                  } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW())))
                                  LIMIT 1
                                }
                              }
                            } AS %selectedContinent
                            WITH {
                              SELECT DISTINCT ?state ?continent WHERE {
                                INCLUDE %selectedContinent.
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) # order by random
                              LIMIT 1
                            } AS %oneState
                            WITH {
                              SELECT DISTINCT ?state ?empty WHERE {
                                INCLUDE %states.
                                FILTER NOT EXISTS { INCLUDE %selectedContinent. }
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) # order by random
                              LIMIT 3
                            } AS %threeStates
                            WHERE {
                                { INCLUDE %oneState. } UNION
                                { INCLUDE %threeStates. }

                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?state  rdfs:label ?answer.
                                ?continent rdfs:label ?question.
                              }
                            }
                            ORDER BY DESC(?question)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"),
                column: "SparqlQuery",
                value: @"
                                # which of these species is {endangered || heavily endangered} ?
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10908;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 400
                                } as %allAmphibia

                                WITH{
                                    SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                    WHERE{
                                      {include %allAmphibia}
                                      ?item wdt:P18 ?img.
                                    }
                                  GROUP BY ?name
                                } as %allImages

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allImages}
                                  }  
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedAmphibia

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allImages}
                                     BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %selectedAmphibia.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                    } 
                                    FILTER NOT EXISTS{Include %selectedAmphibia}
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 3
                                } as %decoyAmphibia

                                WHERE {
                                  {INCLUDE %selectedAmphibia} UNION {INCLUDE %decoyAmphibia}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                                ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "TaskDescription",
                value: "Who is the coach of {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ac86282f-1fa1-48ba-a088-f4c202ea0177"),
                column: "TaskDescription",
                value: "Where is the cocktail {0} from?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #artiadactyl
                                  {?item wdt:P171* wd:Q25329.}
                                  UNION
                                  #primates
                                  {?item wdt:P171* wd:Q7380.}
                                  UNION
                                  #marsupials
                                  {?item wdt:p171* wd:Q25336}
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              #LIMIT 800
                            } as %allAnimals

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status)  ?name
                              WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                 bd:serviceParam wikibase:language 'en'.
                                  ?s  rdfs:label ?sLabel.
                                } 
                                ?s wdt:P279 wd:Q515487.
                              } 
                              GROUP BY ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?status.
                                BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %endangered.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 3
                            } as %noproblem

                            WITH{
                              SELECT *
                              WHERE{
                                include %endangered 
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} 
                               UNION 
                              {INCLUDE %noproblem}
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"), 2, @"
                            SELECT DISTINCT ?question (?name as ?answer)

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(STR(?name)); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q10908.
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                              LIMIT 800
                            } as %allAmphibia

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status) ?name WHERE {
                                {Include %allAmphibia}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?s  rdfs:label ?sLabel.
                                } 
                                ?status wdt:P279 wd:Q515487.
                              }
                              group by ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAmphibia}
                                BIND(lcase(?name) as ?caseName)
                                #filter common names across multiple species that might be endangered
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                      include %endangered.
                                      BIND(lcase(?name) as ?caseName)}
                                    }
                                }
                                ?item wdt:P141 ?status.
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              LIMIT 3
                            } as %noProblem

                            WITH{
                              SELECT *
                              WHERE{
                                include %endangered.
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} UNION {INCLUDE %noProblem}
                              BIND(?status as ?question)
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

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
                                      filter(lang(?itemLabel) = 'en').
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
                column: "SparqlQuery",
                value: @"
                            # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #bats
                                  {?item wdt:P171* wd:Q28425;}
                                  UNION
                                  #rodents
                                  {?item wdt:P171* wd:Q10850;}
                                  ?item wdt:P18 ?image;
                                        wdt:P1843 ?name;
                                  filter(lang(?name) = 'en').
                                  FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                  }
                                }
                              GROUP BY ?item
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              LIMIT 300
                            } as %allAnimals

                            WITH{
                                SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                WHERE{
                                  {include %allAnimals}
                                  ?item wdt:P18 ?img.
                                }
                              GROUP BY ?name
                            } as %allImages

                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allImages}
                              }  
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allImages}
                                 BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %selectedAnimal.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                              } 
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                              {INCLUDE %selectedAnimal} UNION {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"),
                column: "SparqlQuery",
                value: @"
                                # which of these species is {endangered || heavily endangered} ?
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      #marsupials
                                      {?item wdt:P171* wd:Q25336;}
                                      UNION
                                      #rodents
                                      {?item wdt:P171* wd:Q10850;}
                                      ?item wdt:P18 ?image;
                                            wdt:P1843 ?name;
                                      filter(lang(?name) = 'en').
                                      FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                      }
                                    }
                                  GROUP BY ?item
                                } as %allAnimals

                                WITH{
                                    SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                    WHERE{
                                      {include %allAnimals}
                                      ?item wdt:P18 ?img.
                                    }
                                  GROUP BY ?name
                                } as %allImages

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allImages}
                                  }  
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allImages}
                                     BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %selectedAnimal.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                    } 
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                  {INCLUDE %selectedAnimal} UNION {INCLUDE %decoyAnimals}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d0fcf5ac-3215-4355-9090-b6a49cf66cc3"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel ?award ?awardLabel ?movie
                            WHERE{
                              ?actor wdt:P31 wd:Q5;
                                     wdt:P106 wd:Q33999.
                              ?actor p:P166 ?statement.
                              ?statement ps:P166 ?award.
                              ?statement pq:P1686 ?movie.
                              ?award wdt:P31+ wd:Q19020.
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?actor rdfs:label ?actorLabel.
                                                     ?award rdfs:label ?awardLabel}
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 50
                            } as %allWinners
                                       
                            WITH{
                              SELECT ?actor ?actorLabel ?question
                              WHERE{
                                 INCLUDE %allWinners.
                                 SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?movie rdfs:label ?movieLabel.}
                                 BIND(CONCAT('Who won the ', CONCAT(STR(?awardLabel), CONCAT(' for the movie ', CONCAT(STR(?movieLabel), '?'))))  as ?question)
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 1
                            } as %selectedActor

                            WITH{
                              SELECT ?gender
                              WHERE{
                                 INCLUDE %selectedActor
                                 {?actor wdt:P21 ?gender}
                              }
                            } as %selectedGender

                            WITH{
                              SELECT ?gender
                              WHERE{
                                 INCLUDE %allWinners
                                 ?actor wdt:P21 ?gender
                                 Filter NOT EXISTS {
                                  include %selectedGender
                                 }
                              }
                              LIMIT 1
                            } as %filteredGenders

                            WITH{
                              SELECT DISTINCT ?actorLabel
                              WHERE{
                                INCLUDE %allWinners
                                Filter NOT EXISTS {
                                  include %selectedActor
                                }     
                                ?actor wdt:P21 ?gender
                                Filter NOT EXISTS {
                                  include %filteredGenders
                                }
                              }
                              ORDER BY MD5(CONCAT(STR(?actorLabel), STR(NOW())))
                              LIMIT 3
                            } as %decoyActors

                            WHERE{
                                {INCLUDE %selectedActor}
                                UNION
                                {INCLUDE %decoyActors}
                                BIND(?actorLabel as ?answer)
                            }
                            ORDER BY DESC(?question)

                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                           # which of these species is {endangered || heavily endangered} ?
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  #artiodactyla
                                  {?item wdt:P171* wd:Q25329.}
                                  UNION
                                  #carnivores
                                  {?item wdt:P171* wd:Q25306.}
                                  UNION
                                  #primates
                                  {?item wdt:p171* wd:Q7380}
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                              ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                              #LIMIT 800
                            } as %allAnimals

                            WITH {
                              SELECT DISTINCT (GROUP_CONCAT(DISTINCT SAMPLE(?sLabel); SEPARATOR=', ') as ?status)  ?name
                              WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?s.
                                ?s wdt:P279 wd:Q515487.
                                SERVICE wikibase:label { 
                                 bd:serviceParam wikibase:language 'en'.
                                  ?s  rdfs:label ?sLabel.
                                } 
                                ?s wdt:P279 wd:Q515487.
                              } 
                              GROUP BY ?name
                            } as %endangered

                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allAnimals}
                                ?item wdt:P141 ?status.
                                BIND(lcase(?name) as ?caseName)
                                FILTER NOT EXISTS{
                                    {
                                      select ?caseName
                                      where{
                                        include %endangered.
                                        BIND(lcase(?name) as ?caseName)
                                      }
                                    }
                                } 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 3
                            } as %noproblem

                            WITH{
                              SELECT *
                              WHERE{
                                include %endangered 
                              }
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) 
                              LIMIT 1
                            } as %selectedSpecies

                            WHERE {
                              {INCLUDE %selectedSpecies} 
                               UNION 
                              {INCLUDE %noproblem}
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", "Which species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"),
                column: "SparqlQuery",
                value: @"
                                # which of these species is {endangered || heavily endangered} ?
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                       {?item wdt:P171* wd:Q21736.}
                                      UNION
                                      {?item wdt:P171* wd:Q853058.}
                                      UNION
                                      {?item wdt:P171* wd:Q31431}

                                      ?item wdt:P18 ?image;
                                            wdt:P1843 ?name;
                                      filter(lang(?name) = 'en').
                                      FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allBirds

                                WITH{
                                    SELECT (GROUP_CONCAT(DISTINCT SAMPLE(?img); SEPARATOR=', ') as ?image) ?name
                                    WHERE{
                                      {include %allBirds}
                                      ?item wdt:P18 ?img.
                                    }
                                  GROUP BY ?name
                                } as %allImages

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allImages}
                                  }  
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedBird

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allImages}
                                     BIND(lcase(?name) as ?caseName)
                                    FILTER NOT EXISTS{
                                        {
                                          select ?caseName
                                          where{
                                            include %selectedBird.
                                            BIND(lcase(?name) as ?caseName)
                                          }
                                        }
                                    } 
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 3
                                } as %decoyBirds

                                WHERE {
                                  {INCLUDE %selectedBird} UNION {INCLUDE %decoyBirds}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "dce9d630-8fce-4b75-b559-2fe21364709c");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                                # This query includes: primates + artiodactyla + rodentia
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q7380;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allPrimates


                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q25329;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allArtiodactyla

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10850;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allRodents

                                WITH{
                                 SELECT *
                                  WHERE{
                                     {include %allPrimates}
                                     UNION
                                    {include %allRodents}
                                    UNION
                                    {include %allArtiodactyla}
                                  }
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                } as %allAnimals


                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allAnimals}
                                  }  
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allAnimals}
                                    FILTER NOT EXISTS{Include %selectedAnimal}
                                  } 
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                   {INCLUDE %selectedAnimal} 
                                   UNION 
                                   {INCLUDE %decoyAnimals}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                            ", "Which animal is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15679f96-27bd-4e88-b367-4eb05e5f6d95"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question (?dishLabel as ?answer)
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
                              BIND(?image as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25336;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allMarsupials

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q7380;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allPrimates

                            WITH{
                             SELECT *
                              WHERE{
                                 {include %allMarsupials}
                                 UNION
                                {include %allPrimates}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                                ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            SELECT DISTINCT ?question ?item (?name as ?answer)

                            WITH{
                                SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q5113.
                                  ?item wdt:P1843 ?name.
                                  filter(lang(?name) = 'en').
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                }
                              GROUP BY ?item
                            } as %allBirds

                            WITH {
                              SELECT DISTINCT ?item ?status ?name WHERE {
                                {Include %allBirds}
                                ?item wdt:P141 ?status.
                                ?status wdt:P279 wd:Q515487.
                              } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                            } as %endangered
                            WITH {
                              SELECT DISTINCT ?empty ?name WHERE {
                                {Include %allBirds}
                                ?item wdt:P141 ?status.
                                SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.} 
                                VALUES ?status {wd:Q211005}.
                              } 
                              ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                            } as %noproblem

                            WHERE {
                              {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?item  rdfs:label ?itemLabel.
                                ?status rdfs:label ?question.
                              } 
                             } ORDER BY DESC(?question)
                            ", "Which animal is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                column: "MiniGameType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3609a5f7-c90a-4ecf-a713-0224fa8a4215"),
                column: "TaskDescription",
                value: "Where is {0} from?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q10850;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allRodents

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25329;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allArtiodactyla

                            WITH{
                             SELECT *
                              WHERE{
                                 {include %allRodents}
                                 UNION
                                {include %allArtiodactyla}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                                ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"),
                column: "SparqlQuery",
                value: @"
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q122422;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allReptiles

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allReptiles}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedReptile

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allReptiles}
                            FILTER NOT EXISTS{Include %selectedReptile}
                          } 
                          LIMIT 3
                        } as %decoyReptiles

                        WHERE {
                          {
                           INCLUDE %selectedReptile} UNION {INCLUDE %decoyReptiles}       
                           BIND(?image as ?question)
                         }
                        ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                           SELECT DISTINCT ?question ?item (?name as ?answer)

                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q122422.
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allReptiles

                                WITH {
                                  SELECT DISTINCT ?item ?status ?name WHERE {
                                    {Include %allReptiles}
                                    ?item wdt:P141 ?status.
                                    ?status wdt:P279 wd:Q515487.
                                  } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                                } as %endangered
                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allReptiles}
                                    ?item wdt:P141 ?status.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.} 
                                    VALUES ?status {wd:Q211005}.
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WHERE {
                                  {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", "Which animal is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "TaskDescription",
                value: "Sort these softdrinks by inception.");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"),
                column: "SparqlQuery",
                value: @"
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q10908;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allAmphibia

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAmphibia}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAmphibia

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAmphibia}
                            FILTER NOT EXISTS{Include %selectedAmphibia}
                          } 
                          LIMIT 3
                        } as %decoyAmphibia

                        WHERE {
                          {
                           INCLUDE %selectedAmphibia} UNION {INCLUDE %decoyAmphibia}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("50120520-4441-48c1-b387-1c923a038194"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                              # This query includes: carnivore, artiodactyla, primates
                              SELECT DISTINCT ?question (?name as ?answer)
                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10850;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allRodentia

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q7380;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allPrimates

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q25329;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allArtiodactyla

                                WITH{
                                 SELECT *
                                  WHERE{
                                    {include %allRodentia}
                                    UNION
                                    {include %allPrimates}
                                    UNION 
                                    {include %allArtiodactyla}
                                  }
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  Limit 4
                                } as %allAnimals

                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allAnimals}
                                  }  
                                    ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allAnimals}
                                    FILTER NOT EXISTS{Include %selectedAnimal}
                                  } 
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                   {INCLUDE %selectedAnimal} 
                                   UNION 
                                   {INCLUDE %decoyAnimals}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                            ", "Which animal is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"),
                column: "SparqlQuery",
                value: @"
                        # which of these 
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25329;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allArtiodactyla

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q7380;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allPrimates


                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25306;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allCarnivora


                        WITH{
                         SELECT *
                          WHERE{
                             {include %allArtiodactyla}
                             UNION
                            {include %allPrimates}
                             UNION
                            {include %allCarnivora}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          #LIMIT 4
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND(?image as ?question)
                         }
                        ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            # This query includes: artiodactyla, primates, marsupials
                            SELECT DISTINCT ?question (?name as ?answer)
                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25336;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allMarsupials

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q7380;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allPrimates

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25329;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allArtiodactyla

                            WITH{
                             SELECT *
                              WHERE{
                                {include %allMarsupials}
                                UNION
                                {include %allPrimates}
                                UNION 
                                {include %allArtiodactyla}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                                ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                            ", "Which animal is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                                # This query includes: primates + artiodactyla + rodentia
                                SELECT DISTINCT ?question (?name as ?answer)

                                #seperated animals in variables befor unionizing for performance/quicker response
                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q7380;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allPrimates


                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q25329;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allArtiodactyla

                                WITH{
                                    SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10850;
                                            wdt:P1843 ?name;
                                            wdt:P141 wd:Q211005;
                                            wdt:P18 ?image.
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                      filter(lang(?name) = 'en').
                                    }
                                  GROUP BY ?item
                                } as %allRodents

                                WITH{
                                 SELECT *
                                  WHERE{
                                     {include %allPrimates}
                                     UNION
                                    {include %allRodents}
                                    UNION
                                    {include %allArtiodactyla}
                                  }
                                  ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                                } as %allAnimals


                                WITH {
                                  SELECT DISTINCT ?image ?name WHERE {
                                    {Include %allAnimals}
                                  }  
                                  LIMIT 1
                                } as %selectedAnimal

                                WITH {
                                  SELECT DISTINCT ?name WHERE {
                                    {Include %allAnimals}
                                    FILTER NOT EXISTS{Include %selectedAnimal}
                                  } 
                                  LIMIT 3
                                } as %decoyAnimals

                                WHERE {
                                   {INCLUDE %selectedAnimal} 
                                   UNION 
                                   {INCLUDE %decoyAnimals}       
                                   BIND(?image as ?question)
                                 } ORDER BY DESC(?question)
                            ", "Which animal is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"),
                column: "SparqlQuery",
                value: @"
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25336;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allMarsupials

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25306;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allCarnivora

                        WITH{
                         SELECT *
                          WHERE{
                            {include %allMarsupials}
                             UNION
                            {include %allCarnivora}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"),
                column: "SparqlQuery",
                value: @"SELECT ?answer ?question
                            WITH {
                              SELECT DISTINCT ?state ?continent ?stateLabel ?continentLabel WHERE {
                                ?state wdt:P31/wdt:P279* wd:Q3624078;
                                     p:P463 ?memberOfStatement.
                                ?memberOfStatement a wikibase:BestRank;
                                                   ps:P463 wd:Q1065.
                                MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                MINUS { ?state wdt:P576|wdt:P582 ?end. }
                                ?state p:P30 ?continentStatement.
                                ?continentStatement a wikibase:BestRank;
                                                  ps:P30 ?continent.
                                VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                              }
                            } AS %states
                            WITH {
                              SELECT ?state ?continent WHERE {
                                INCLUDE %states.
                                {
                                  SELECT DISTINCT ?continent WHERE {
                                    VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                                  } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW())))
                                  LIMIT 1
                                }
                              }
                            } AS %selectedContinent
                            WITH {
                              SELECT DISTINCT ?state ?continent WHERE {
                                INCLUDE %selectedContinent.
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) # order by random
                              LIMIT 1
                            } AS %oneState
                            WITH {
                              SELECT ?state ?empty WHERE {
                                INCLUDE %states.
                                FILTER NOT EXISTS { INCLUDE %selectedContinent. }
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) # order by random
                              LIMIT 3
                            } AS %threeStates
                            WHERE {
                                { INCLUDE %oneState. } UNION
                                { INCLUDE %threeStates. }

                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?state  rdfs:label ?answer.
                                ?continent rdfs:label ?question.
                              }
                            }
                            ORDER BY DESC(?question)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"),
                column: "SparqlQuery",
                value: @"
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q127282;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 4
                        } as %allFishes

                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allFishes}
                          }  
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedFish

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allFishes}
                            FILTER NOT EXISTS{Include %selectedFish}
                          } 
                          LIMIT 3
                        } as %decoyFish

                        WHERE {
                          {
                           INCLUDE %selectedFish} UNION {INCLUDE %decoyFish}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "TaskDescription",
                value: "Who is the trainer of {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ac86282f-1fa1-48ba-a088-f4c202ea0177"),
                column: "TaskDescription",
                value: "Where is {0} from?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                           SELECT DISTINCT ?question ?item (?name as ?answer)
                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q10908.
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allAmphibia

                                WITH {
                                  SELECT DISTINCT ?item ?status ?name WHERE {
                                    {Include %allAmphibia}
                                    ?item wdt:P141 ?status.
                                    ?status wdt:P279 wd:Q515487.
                                  } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                                } as %endangered
                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allAmphibia}
                                    ?item wdt:P141 ?status.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.} 
                                    VALUES ?status {wd:Q211005}.
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WHERE {
                                  {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", "Which animal is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            # This query includes: rodentia, carnivora, marsupial
                            SELECT DISTINCT ?question (?name as ?answer)

                            #seperated animals in variables befor unionizing for performance/quicker response
                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q10850;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allRodentia

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25306;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allCarnivora

                            WITH{
                                SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                                WHERE{
                                  ?item wdt:P171* wd:Q25336;
                                        wdt:P1843 ?name;
                                        wdt:P141 wd:Q211005;
                                        wdt:P18 ?image.
                                   FILTER NOT EXISTS{
                                     ?item wdt:P141 wd:Q237350.
                                     ?item wdt:P171* wd:Q23038290
                                   }
                                  filter(lang(?name) = 'en').
                                }
                              GROUP BY ?item
                            } as %allMarsupial

                            WITH{
                             SELECT *
                              WHERE{
                                 {include %allRodentia}
                                 UNION
                                {include %allCarnivora}
                                UNION
                                {include %allMarsupial}
                              }
                              ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                            } as %allAnimals


                            WITH {
                              SELECT DISTINCT ?image ?name WHERE {
                                {Include %allAnimals}
                              }  
                              LIMIT 1
                            } as %selectedAnimal

                            WITH {
                              SELECT DISTINCT ?name WHERE {
                                {Include %allAnimals}
                                FILTER NOT EXISTS{Include %selectedAnimal}
                              } 
                              LIMIT 3
                            } as %decoyAnimals

                            WHERE {
                               {INCLUDE %selectedAnimal} 
                               UNION 
                               {INCLUDE %decoyAnimals}       
                               BIND(?image as ?question)
                             } ORDER BY DESC(?question)
                            ", "Which animal is this?" });

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
                column: "SparqlQuery",
                value: @"
                       # which of these 
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q28425;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allBats

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q10850;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allRodentias


                        WITH{
                         SELECT *
                          WHERE{
                             {include %allBats}
                             UNION
                            {include %allRodentias}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          #LIMIT 4
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND(?image as ?question)
                         }
                        ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"),
                column: "SparqlQuery",
                value: @"
                        # which of these 
                        SELECT DISTINCT ?question (?name as ?answer)

                        #seperated animals in variables befor unionizing for performance/quicker response
                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q25336;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allMarsupials

                        WITH{
                            SELECT DISTINCT (SAMPLE(?image) as ?image) ?item (SAMPLE(GROUP_CONCAT(DISTINCT Sample(?name); SEPARATOR=', ')) as ?name)
                            WHERE{
                              ?item wdt:P171* wd:Q10850;
                                    wdt:P1843 ?name;
                                    wdt:P141 wd:Q211005;
                                    wdt:P18 ?image.
                               FILTER NOT EXISTS{
                                 ?item wdt:P141 wd:Q237350.
                                 ?item wdt:P171* wd:Q23038290
                               }
                              filter(lang(?name) = 'en').
                            }
                          GROUP BY ?item
                        } as %allRodentias


                        WITH{
                         SELECT *
                          WHERE{
                             {include %allMarsupials}
                             UNION
                            {include %allRodentias}
                          }
                          ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          #LIMIT 4
                        } as %allAnimals


                        WITH {
                          SELECT DISTINCT ?image ?name WHERE {
                            {Include %allAnimals}
                          }  
                            ORDER BY MD5(CONCAT(STR(?image), STR(NOW())))
                          LIMIT 1
                        } as %selectedAnimal

                        WITH {
                          SELECT DISTINCT ?name WHERE {
                            {Include %allAnimals}
                            FILTER NOT EXISTS{Include %selectedAnimal}
                          } 
                          LIMIT 3
                        } as %decoyAnimals

                        WHERE {
                           {INCLUDE %selectedAnimal} 
                           UNION 
                           {INCLUDE %decoyAnimals}       
                           BIND(?image as ?question)
                         }
                        ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d0fcf5ac-3215-4355-9090-b6a49cf66cc3"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer ?awardLabel ?movieLabel
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel ?movieLabel ?awardLabel
                            WHERE{
                              ?actor wdt:P31 wd:Q5;
                                     wdt:P106 wd:Q33999;
                                     wdt:P166 ?award.
                              ?award wdt:P31+ wd:Q19020.
                              ?actor p:P166 ?statement.
                              ?statement pq:P1686 ?movie.
                              ?statement pq:P805+ ?awardCeremony.
                              ?awardCeremony wdt:P31+ wd:Q16913666.
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?movie rdfs:label ?movieLabel.
                                                     ?actor rdfs:label ?actorLabel.
                                                     ?award rdfs:label ?awardLabel}
                            }
                            ORDER BY ?actor ?actorLabel
                            } as %allWinners
                                       
                            WITH{
                              SELECT ?actor ?actorLabel ?question ?movieLabel ?awardLabel
                              WHERE{
                                 INCLUDE %allWinners.
                                 BIND(CONCAT('Who won the ', CONCAT(STR(?awardLabel), CONCAT(' for ', CONCAT(STR(?movieLabel), '?'))))  as ?question)
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 1
                            } as %selectedActor

                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel
                              WHERE{
                                INCLUDE %allWinners
                                Filter NOT EXISTS {INCLUDE %selectedActor}
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 3
                            } as %decoyActors

                            WHERE{
                                {INCLUDE %selectedActor}
                                UNION
                                {INCLUDE %decoyActors}
                                BIND(?actorLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                           SELECT DISTINCT ?question ?item (?name as ?answer)

                                WITH{
                                    SELECT DISTINCT ?item (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?name); SEPARATOR=', ')) as ?name)
                                    WHERE{
                                      ?item wdt:P171* wd:Q127282.
                                      ?item wdt:P1843 ?name.
                                      filter(lang(?name) = 'en').
                                       FILTER NOT EXISTS{
                                         ?item wdt:P141 wd:Q237350.
                                         ?item wdt:P171* wd:Q23038290
                                       }
                                    }
                                  GROUP BY ?item
                                } as %allFishes

                                WITH {
                                  SELECT DISTINCT ?item ?status ?name WHERE {
                                    {Include %allFishes}
                                    ?item wdt:P141 ?status.
                                    ?status wdt:P279 wd:Q515487.
                                  } ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 1
                                } as %endangered
                                WITH {
                                  SELECT DISTINCT ?empty ?name WHERE {
                                    {Include %allFishes}
                                    ?item wdt:P141 ?status.
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.} 
                                    VALUES ?status {wd:Q211005}.
                                  } 
                                  ORDER BY MD5(CONCAT(STR(?name), STR(NOW()))) LIMIT 3
                                } as %noproblem

                                WHERE {
                                  {INCLUDE %endangered} UNION {INCLUDE %noproblem}
          
                                  SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?itemLabel.
                                    ?status rdfs:label ?question.
                                  } 
                                 } ORDER BY DESC(?question)
                            ", "Which animal is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"),
                column: "SparqlQuery",
                value: @"
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
                        ");
        }
    }
}
