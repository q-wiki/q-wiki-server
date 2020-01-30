using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixEndangeredSpeciesInventorDuplicateOutput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: new Guid("14d93797-c61c-4415-b1ed-359d180237ff"),
                column: "SparqlQuery",
                value: @"#Which of these moons belongs to the planet {0}?
                            SELECT ?question ?answer
                            WITH{
                              Select distinct ?planet ?planetLabel ?image
                              WHERE{
                                #get all planets that have a moon
                                  ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                  ?moon wdt:P31/wdt:P279* wd:Q2537;
                                                          wdt:P397 ?planet.
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                         ?planet rdfs:label ?planetLabel}
                              }
                            } as %allPlanets

                            WITH{
                                SELECT ?planetLabel ?planet
                                WHERE{
                                  INCLUDE %allPlanets
                                }
                              ORDER BY MD5(CONCAT(STR(?planet), STR(NOW()))) 
                              LIMIT 1
                            } as %selectedPlanet

                            #get moon of one selected planet
                            WITH{
                              SELECT ?moon ?moonLabel ?planet ?planetLabel
                              WHERE{
                                include %selectedPlanet
                                ?moon wdt:P31/wdt:P279* wd:Q2537;
                                                          wdt:P397 ?planet.
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                         ?planet rdfs:label ?planetLabel.
                                                          ?moon rdfs:label ?moonLabel}
                                 FILTER(!CONTAINS(?moonLabel, '/'))
                              }
                              ORDER BY MD5(CONCAT(STR(?planet), STR(NOW()))) 
                              limit 1
                            } as %selectedMoon

                            WITH {
                               SELECT DISTINCT ?moonLabel 
                               WHERE{
                                  INCLUDE %allPlanets
                                  FILTER NOT EXISTS{INCLUDE %selectedPlanet}
                                  ?moon wdt:P31/wdt:P279* wd:Q2537;
                                                          wdt:P397 ?planet.
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                          ?moon rdfs:label ?moonLabel}
                                  FILTER(!CONTAINS(?moonLabel, '/'))
                                }
                              ORDER BY MD5(CONCAT(STR(?moonLabel), STR(NOW()))) 
                              LIMIT 3
                            } as %decoyMoons

                            WHERE{
                              {INCLUDE %selectedMoon}
                              UNION
                              {INCLUDE %decoyMoons}
                              Bind(?planetLabel as ?question)
                              Bind(?moonLabel as ?answer)
                            }

                            ORDER BY DESC(?question)
                            ");

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
                              ORDER BY MD5(CONCAT(STR(?dish), STR(NOW())))
                              LIMIT 4
                            } as %selectedDish

                            WHERE{
                              INCLUDE %selectedDish
                              BIND(?image as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "TaskDescription",
                value: "Which one of these species is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                                SELECT ?question ?answer
                                WITH{
                                SELECT DISTINCT ?creator
                                  WHERE 
                                  { 
                                    ?painting wdt:P1343 wd:Q66362718;
                                            wdt:P170 ?creator.
                                  }
                                   ORDER BY (MD5(CONCAT(STR(?creator), STR(NOW()))))                    
                                   LIMIT 20
                                  } as %selectedArtists

                                WITH{
                                    SELECT DISTINCT ?creator ?creatorLabel  (MIN(?inception) AS ?inception1) 
                                            WHERE{
                                                  INCLUDE %selectedArtists.
                                                   ?painting wdt:P170 ?creator.
                                                   ?painting wdt:P571 ?inception.
                                                   Filter(datatype(YEAR(?inception))!='') .
                                                   Filter(?creator != wd:Q4233718) 
                                                   SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                          ?creator rdfs:label ?creatorLabel}
                                            SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                          ?creator rdfs:label ?creatorLabel}
                                            }
                                  group by ?creator ?creatorLabel
                                } as %firstPainting

                                WITH{
                                   select distinct (GROUP_CONCAT ( distinct sample(?creatorLabel); separator=', ') as ?painter) ?year
                                   WHERE{
                                     include %firstPainting
                                     bind(year(?inception1) as ?year)
                                   }
                                  group by ?year
                                  ORDER BY (MD5(CONCAT(STR(?year), STR(NOW()))))                    
                                  LIMIT 4
                                } as %groupedPainter

                                WHERE{
                                    INCLUDE %groupedPainter.
                                    SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                           ?painting rdfs:label ?paintingLabel}
                                    BIND(?painter as ?answer)
                                    BIND('order the artists by the inception of their first painting' as ?question)
                                }
                                  order by ?year
                                ", "Sort artist by release of first painting (ascending)." });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            SELECT DISTINCT ?question ?answer ?awardCount
                            WITH{
                                                           # select the 500 movies with the highest box office revenue
                            SELECT DISTINCT ?movie WHERE {
                              ?movie wdt:P31 wd:Q11424.
                              ?movie p:P2142/psv:P2142 [wikibase:quantityAmount ?boxOffice; wikibase:quantityUnit ?currency].
                              # only look at us dollars
                              FILTER(?currency = wd:Q4917)
                            }
                            ORDER BY DESC(?boxOffice)
                            LIMIT 500
                            } as %topGrossingMovies


                            WITH{
                            SELECT DISTINCT ?actor
                            WHERE{
                               {INCLUDE %topGrossingMovies}
                              # get all actors that played in those movies
                              ?movie wdt:P161 ?actor.
                              ?actor wdt:P166 ?award.
                              }
                              ORDER BY MD5(CONCAT(STR(?award), STR(NOW())))
                              limit 150
                            } as %allWinners

                            WITH{
                            SELECT DISTINCT ?actorLabel (count(?award) as ?awardCount)
                              WHERE {
                                     {
                                        SELECT ?actor
                                        WHERE {include %allWinners}
                                     }
                                     ?actor wdt:P166 ?award.
                                     ?award wdt:P31*/wdt:P279* wd:Q4220917
                                     #?baseAward.
                                     #VALUES ?baseAward {wd:Q4220917 wd:Q1407225}
                                     SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                                                     ?actor rdfs:label ?actorLabel
                                                                                    }
                              }
                              group by ?actorLabel
                            } as %countedMovies

                            WITH{
                               select (Group_Concat(distinct  sample(?actorLabel); separator=', ') as ?actors) ?awardCount
                               where {
                                    include %countedMovies
                               }
                               group by ?awardCount
                               ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                               limit 4
                            } as %summedActors

                            WHERE{
                                {INCLUDE %summedActors}
                                BIND(?actors as ?answer)
                                BIND('Sort actors by oscars received.' as ?question)
                            }
                            ORDER BY asc(?awardCount)
                            ", "Sort these actors by the number of movie awards they have received (ascending)." });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("39508588-107a-4220-a748-a72eaad711db"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question ?answer
                            WITH{
                              Select distinct ?planet ?planetLabel ?image
                              WHERE{
                                  ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                  ?planet wdt:P18 ?image
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                         ?planet rdfs:label ?planetLabel}
                              }
                            } as %allPlanets

                            WITH{
                                SELECT ?planetLabel ?image
                                WHERE{
                                  INCLUDE %allPlanets
                                }
                              LIMIT 1
                            } as %selectedPlanet

                            WITH {
                               SELECT ?planetLabel  
                               WHERE{
                                  INCLUDE %allPlanets
                                  FILTER NOT EXISTS{INCLUDE %selectedPlanet}
                                }
                              LIMIT 3
                            } as %decoyPlanets

                            WHERE{
                              {INCLUDE %selectedPlanet}
                              UNION
                              {INCLUDE %decoyPlanets}
                              Bind(?image as ?question)
                              Bind(?planetLabel as ?answer)
                            }

                            ORDER BY DESC(?question)
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
                                ?status rdfs:label ?statusLabel.
                              } 
                               BIND(REPLACE(str(?statusLabel), 'species', '') AS ?question)
                             } ORDER BY DESC(?question)
                            ", "Which one of these species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "TaskDescription",
                value: "Sort these softdrinks by release (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
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
                                ?status rdfs:label ?statusLabel.
                              } 
                               BIND(REPLACE(str(?statusLabel), 'species', '') AS ?question)
                             } ORDER BY DESC(?question)
                            ", "Which one of these species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "TaskDescription",
                value: "Order these animals by bite strenght relatively to their size (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"),
                column: "SparqlQuery",
                value: @"# Which chemical compound has the formula {0}?
                            SELECT DISTINCT ?chemicalCompound ?answer (sample(?chemical_formula) AS ?question) ?sitelinks WHERE {
                              ?chemicalCompound wdt:P31 wd:Q11173;
                                wdt:P274 ?chemical_formula;
                                wikibase:sitelinks ?sitelinks.
                              FILTER(?sitelinks >= 50 )
                              ?chemicalCompound rdfs:label ?answer.
                              FILTER((LANG(?answer)) = 'en')
                            }
                            group by ?chemicalCompound ?answer ?sitelinks
                            ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW()))))
                            LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                column: "SparqlQuery",
                value: @"# Which country does not border the Mediterranean Sea?
                            SELECT DISTINCT ?question ?answer
                            WITH {
                              SELECT DISTINCT (?state as ?country) WHERE {
                                ?state wdt:P31/wdt:P279 wd:Q3624078;
                                       p:P463 ?memberOfStatement.
                                ?memberOfStatement a wikibase:BestRank;
                                                     ps:P463 wd:Q1065.
                                MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                MINUS { ?state wdt:P576|wdt:P582 ?end. }
                              }
                            } AS %states

                            WITH { 
                              SELECT DISTINCT ?country WHERE {
                                BIND(wd:Q4918 AS ?sea).
                                ?sea wdt:P205 ?country.
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 3 # random three
                            } as %threeBasins

                            WITH {
                              SELECT DISTINCT ?country ?noSea WHERE {
                                BIND(wd:Q4918 AS ?noSea).
                                INCLUDE %states.
                                ?country wdt:P361 ?region.
                                VALUES ?region { wd:Q7204 wd:Q984212 wd:Q27449 wd:Q263686 wd:Q50807777 wd:Q27468 wd:Q27381 }.
                                FILTER NOT EXISTS {?country wdt:P31 wd:Q51576574.}
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 1 # random one
                            } AS %oneOther

                            WHERE {
                              { INCLUDE %oneOther. } UNION
                              { INCLUDE %threeBasins. }
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?country rdfs:label ?answer.
                                ?noSea rdfs:label ?question.
                              }
                            }
                            order by DESC(?noSea)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "TaskDescription",
                value: "Who is the current coach of {0}?");

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
                            ?status rdfs:label ?statusLabel.
                          } 
                           BIND(REPLACE(str(?statusLabel), 'species', '') AS ?question)
                         } ORDER BY DESC(?question)
                            ", "Which one of these species is {0}?" });

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
                                 } as %allInventors

                                WITH{
                                 SELECT (group_concat(distinct sample(?inventorLabel); separator=', ') as ?inventors) ?itemLabel
                                  WHERE 
                                    { 
                                     INCLUDE %allInventors
                                    }
                                   group by ?itemLabel
                                } as %groupedInventors

                               WITH{
                                 SELECT ?inventors ?itemLabel
                                  WHERE 
                                    { 
                                     INCLUDE %groupedInventors
                                    }
                                   ORDER BY (MD5(CONCAT(STR(?inventorLabel), STR(NOW())))) 
                                   LIMIT 1
                                } as %selectedInventor

                                WITH{
                                 SELECT Distinct ?inventors
                                  WHERE 
                                    { 
                                     INCLUDE %groupedInventors.
                                     FILTER NOT EXISTS {INCLUDE %selectedInventor}
                                    }
                                   ORDER BY (MD5(CONCAT(STR(?inventorLabel), STR(NOW())))) 
                                   LIMIT 3
                                } as %decoyInventors

                                WHERE{
                                  {INCLUDE %selectedInventor}
                                  UNION
                                  {INCLUDE %decoyInventors}
                                  BIND(?inventors as ?answer)
                                  Bind(?itemLabel as ?question)
                                }

                                Order by DESC(?question)
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
                                ?status rdfs:label ?statusLabel.
                              } 
                               BIND(REPLACE(str(?statusLabel), 'species', '') AS ?question)
                             } ORDER BY DESC(?question)
                            ", "Which one of these species is {0}?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by maximum amount ofparticipating players (ascending).");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "c4a7b1e6-8067-4fe9-a8f4-ed76b64e02f2");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("14d93797-c61c-4415-b1ed-359d180237ff"),
                column: "SparqlQuery",
                value: @"#Which of these moons belongs to the planet {0}?
                            SELECT ?question ?answer 
                            WITH {
                              # subquery 1: get all moons of planets of our solar system
                              SELECT ?moon ?parent ?question ?answer WHERE {
                              {
                                SELECT ?moon ?moonLabel ?parent WHERE {
                                  ?moon wdt:P31/wdt:P279* wd:Q2537;
                                        wdt:P397 ?parent.
                                  ?parent wdt:P361+ wd:Q544.
                                  BIND (?parent as ?planet).
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. }
                                }
                              }
                              FILTER(!CONTAINS(?moonLabel, '/'))
                            } ORDER BY MD5(CONCAT(STR(?moonLabel), STR(NOW()))) # order by random
                            } as %moons

                            WITH {
                              # subquery 2:
                              # get one random planet
                              # get all moons out of list 1 which belong to that planet
                              SELECT ?moon ?parent WHERE {
                                INCLUDE %moons.
                                {
                                  SELECT DISTINCT ?parent WHERE {
                                    {
                                      SELECT ?moon ?moonLabel ?parentLabel ?parent WHERE {
                                        ?moon wdt:P31/wdt:P279* wd:Q2537;
                                              wdt:P397 ?parent.
                                        ?parent wdt:P361+ wd:Q544.
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. }
                                      }
                                    }
                                    FILTER(!CONTAINS(?moonLabel, '/'))
                                  } 
                                  GROUP BY ?parent
                                }
                              } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) # order by random
                                LIMIT 1
                            } AS %selectedPlanet

                            WITH {
                              # subquery 3: get one moon out of list 2 (= correct answer)
                              SELECT DISTINCT ?moon ?parent WHERE {
                                INCLUDE %selectedPlanet.
                              } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) 
                              LIMIT 1
  
                            } AS %oneMoon

                            WITH {
                            # subquery 4 get three false answers (question/parent must be empty here!)
                              SELECT DISTINCT ?moon ?empty WHERE {
                                INCLUDE %moons.
                                FILTER NOT EXISTS { INCLUDE %selectedPlanet. }
                              } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) 
                              LIMIT 3
                            } AS %threeMoons

                            WITH {
                              # another subquery because of dubios server errors
                              SELECT * WHERE {

                                 {INCLUDE %threeMoons } UNION {INCLUDE %oneMoon}
                              }
                            } AS %final WHERE {
                              INCLUDE %final.
  
                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?parent  rdfs:label ?question.
                                ?moon rdfs:label ?answer.
                              }
                            } ORDER BY DESC(?question)");

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
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "TaskDescription",
                value: "Which species is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                               SELECT ?question ?answer
                                WITH{
                                SELECT DISTINCT ?creator
                                  WHERE 
                                  { 
                                    ?painting wdt:P1343 wd:Q66362718;
                                            wdt:P170 ?creator.
                                  }
                                   ORDER BY (MD5(CONCAT(STR(?creator), STR(NOW()))))                    
                                   LIMIT 16
                                  } as %selectedArtists

                                WITH{
                                    SELECT DISTINCT ?creator ?creatorLabel (SAMPLE(?inception) as ?firstPaintingInception) (SAMPLE(?painting) as ?firstPainting) (GROUP_CONCAT(DISTINCT ?paintingLabel; SEPARATOR=', ') AS ?paintingNames)
                                            WHERE{
                                                  INCLUDE %selectedArtists.
                                                  {SELECT *
                                                    where{ 
                                                   ?creator wdt:P106 wd:Q1028181.
                                                   ?painting wdt:P170 ?creator.
                                                   ?painting wdt:P571 ?inception.
                                                   Filter(datatype(YEAR(?inception))!='') .  
                                                   SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                          ?creator rdfs:label ?creatorLabel}
                                                  }
                                              order by ?inception
                                            }}
                                  group by ?creator ?creatorLabel
                                  ORDER BY (MD5(CONCAT(STR(?creator), STR(NOW()))))  
                                  LIMIT 4
                                } as %firstPainting

                                    WHERE{
                                    INCLUDE %firstPainting.
                                    SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                           ?creator rdfs:label ?answer.
                                                           ?painting rdfs:label ?paintingLabel}
                                    BIND(?creatorLabel as ?answer)
                                    BIND('order the artists by the inception of their first painting' as ?question)
                                }
                                  order by ?firstPaintingInception
                                ", "Sort artist by release of first painting." });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"
                            SELECT DISTINCT ?question ?answer ?awardCount
                                WITH{
                                                               # select the 500 movies with the highest box office revenue
                                SELECT DISTINCT ?movie WHERE {
                                  ?movie wdt:P31 wd:Q11424.
                                  ?movie p:P2142/psv:P2142 [wikibase:quantityAmount ?boxOffice; wikibase:quantityUnit ?currency].
                                  # only look at us dollars
                                  FILTER(?currency = wd:Q4917)
                                }
                                ORDER BY DESC(?boxOffice)
                                LIMIT 500
                                } as %topGrossingMovies

                                WITH{
                                SELECT DISTINCT ?actor ?award
                                WHERE{
                                   {INCLUDE %topGrossingMovies}
                                  # get all actors that played in those movies
                                  ?movie wdt:P161 ?actor.
                                  ?actor wdt:P166 ?award.
                                  }
                                  ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                                  limit 500
                                } as %allWinners

                                WITH{
                                SELECT DISTINCT ?actorLabel (count(?award) as ?awardCount)
                                  WHERE {
                                         {INCLUDE %allWinners.}
                                         ?award wdt:P31*/wdt:P279* wd:Q4220917
                                         #?baseAward. logic if we want to count movie and television awards
                                         #VALUES ?baseAward {wd:Q4220917 wd:Q1407225}
                                         SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                                                         ?actor rdfs:label ?actorLabel
                                                                                        }
                                  }
                                  group by ?actorLabel
                                } as %countedMovies

                                WITH{
                                   select (Group_Concat(distinct sample(?actorLabel); separator=', ') as ?actors) ?awardCount
                                   where {
                                        include %countedMovies
                                        filter(?awardCount > 1)
                                   }
                                   group by ?awardCount
                                   ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                                   limit 4
                                } as %summedActors

                                WHERE{
                                    {INCLUDE %summedActors}
                                    BIND(?actors as ?answer)
                                    BIND('Sort actors by oscars received.' as ?question)
                                }
                                ORDER BY asc(?awardCount)
                            ", "Sort these actors by the number of movie awards they have received." });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("39508588-107a-4220-a748-a72eaad711db"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question ?answer
                            WITH{
                              Select distinct ?planet ?planetLabel ?image
                              WHERE{
                                  ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                  ?planet wdt:P18 ?image
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                         ?planet rdfs:label ?planetLabel}
                              }
                            } as %allPlanets

                            WITH{
                                SELECT ?planetLabel ?image
                                WHERE{
                                  INCLUDE %allPlanets
                                }
                              LIMIT 1
                            } as %selectedPlanet

                            WITH {
                               SELECT ?planetLabel  
                               WHERE{
                                  INCLUDE %allPlanets
                                  FILTER NOT EXISTS{INCLUDE %selectedPlanet}
                                }
                              LIMIT 3
                            } as %decoyPlanets

                            WHERE{
                              {INCLUDE %selectedPlanet}
                              UNION
                              {INCLUDE %decoyPlanets}
                              Bind(?image as ?question)
                              Bind(?planetLabel as ?answer)
                            }

                            ORDER BY DESC(?question)
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
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
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
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "TaskDescription",
                value: "Order these animals by bite force quotient (ascending).");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"),
                column: "SparqlQuery",
                value: @"# Which chemical compound has the formula {0}?
                            SELECT DISTINCT ?chemicalCompound ?answer (?chemical_formula AS ?question) ?sitelinks WHERE {
                              ?chemicalCompound wdt:P31 wd:Q11173;
                                wdt:P274 ?chemical_formula;
                                wikibase:sitelinks ?sitelinks.
                              FILTER(?sitelinks >= 50 )
                              ?chemicalCompound rdfs:label ?answer.
                              FILTER((LANG(?answer)) = 'en')
                            }
                            ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW()))))
                            LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                column: "SparqlQuery",
                value: @"# Which country does not border the Mediterranean Sea?
                            SELECT DISTINCT ?question ?answer
                            WITH {
                              SELECT DISTINCT (?state as ?country) WHERE {
                                ?state wdt:P31/wdt:P279* wd:Q3624078;
                                       p:P463 ?memberOfStatement.
                                ?memberOfStatement a wikibase:BestRank;
                                                     ps:P463 wd:Q1065.
                                MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                MINUS { ?state wdt:P576|wdt:P582 ?end. }
                              }
                            } AS %states

                            WITH { 
                              SELECT DISTINCT ?country WHERE {
                                BIND(wd:Q4918 AS ?sea).
                                ?sea wdt:P205 ?country.
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 3 # random three
                            } as %threeBasins

                            WITH {
                              SELECT DISTINCT ?country ?noSea WHERE {
                                BIND(wd:Q4918 AS ?noSea).
                                INCLUDE %states.
                                ?country wdt:P361 ?region.
                                VALUES ?region { wd:Q7204 wd:Q984212 wd:Q27449 wd:Q263686 wd:Q50807777 wd:Q27468 wd:Q27381 }.
                                FILTER NOT EXISTS {?country wdt:P31 wd:Q51576574.}
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 1 # random one
                            } AS %oneOther

                            WHERE {
                              { INCLUDE %oneOther. } UNION
                              { INCLUDE %threeBasins. }
                              SERVICE wikibase:label { 
                                bd:serviceParam wikibase:language 'en'.
                                ?country rdfs:label ?answer.
                                ?noSea rdfs:label ?question.
                              }
                            }
                            order by DESC(?noSea)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "TaskDescription",
                value: "Who is the coach of {0}?");

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
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by participating players.");
        }
    }
}
