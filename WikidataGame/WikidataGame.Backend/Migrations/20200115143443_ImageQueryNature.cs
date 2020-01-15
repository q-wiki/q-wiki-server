using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class ImageQueryNature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                          {INCLUDE %selectedFish} UNION {INCLUDE %decoyFish}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                          {INCLUDE %selectedAmphibia} UNION {INCLUDE %decoyAmphibia}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                          {INCLUDE %selectedReptile} UNION {INCLUDE %decoyReptiles}       
                           BIND(?image as ?question)
                         } ORDER BY DESC(?question)
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                         } ORDER BY DESC(?question)
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                         } ORDER BY DESC(?question)
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("bae2897b-61c9-448e-bedf-8fc069dd62b0"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                         } ORDER BY DESC(?question)
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                        ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                            ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 1, @"
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
                            ", 2, "Which animal is on the image?" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"), new Guid("e9019ee1-0eed-492d-8aa7-feb1974fb265"), 2, @"
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
                            ", 2, "Which animal is {0}" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"), new Guid("ddd333f7-ef45-4e13-a2ca-fb4494dce324"), 1, @"
                            SELECT ?question ?answer ?painting
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
                                BIND(?image as ?question)
                            }
                              order by DESC(?question)
                            ", 2, "Which painting is this?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bae2897b-61c9-448e-bedf-8fc069dd62b0"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"));
        }
    }
}
