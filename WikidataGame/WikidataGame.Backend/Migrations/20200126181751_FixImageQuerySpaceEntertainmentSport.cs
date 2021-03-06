﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixImageQuerySpaceEntertainmentSport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ConcurrencyStamp",
                value: "be45143a-8aa2-4bdd-9f91-21ca550ba06f");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"),
                column: "SparqlQuery",
                value: @"# wars of the 20th century
                            SELECT (SAMPLE(?itemLabel) AS ?answer) (YEAR(MAX(?startdate)) AS ?question) WHERE {
                              {
                                SELECT ?item ?itemLabel ?startdate WHERE {
                                  ?item (wdt:P31/(wdt:P279*)) wd:Q198.
                                  ?item wdt:P580 ?startdate.
                                  FILTER(?startdate >= '1900-01-01T00:00:00Z'^^xsd:dateTime)
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                                }
                              }
                              FILTER(!(CONTAINS(?itemLabel, '1')))
                              FILTER(!(CONTAINS(?itemLabel, '2')))
                              FILTER(!(STRSTARTS(?itemLabel, 'Q')))
                            }
                            GROUP BY ?itemLabel
                            ORDER BY (MD5(CONCAT(STR(?item), STR(NOW()))))
                            LIMIT 4");

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
                keyValue: new Guid("1e5551e1-a6a3-42df-8447-b0ac0effc8e6"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel
                            WHERE{
                              ?actor wdt:P31 wd:Q5;
                                     wdt:P106 wd:Q33999;
                                     wdt:P166 ?award.
                              ?award wdt:P31+ wd:Q19020.
                              ?actor wdt:P18 ?image
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?movie rdfs:label ?movieLabel.
                                                     ?actor rdfs:label ?actorLabel.
                                                     ?award rdfs:label ?awardLabel}
                            }
                              ORDER BY ?actor ?actorLabel
                            } as %allWinners
                                       
                            WITH{
                              SELECT ?actor ?actorLabel ?image
                              WHERE{
                                 INCLUDE %allWinners.
                                ?actor wdt:P18 ?image
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
                              SELECT DISTINCT ?actor ?actorLabel
                              WHERE{
                                INCLUDE %allWinners
                                Filter NOT EXISTS {INCLUDE %selectedActor}
                                ?actor wdt:P21 ?gender
                                Filter NOT EXISTS {
                                  include %filteredGenders
                                }
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?actor rdfs:label ?actorLabel.}
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 3
                            } as %decoyActors

                            WHERE{
                                {INCLUDE %selectedActor}
                                UNION
                                {INCLUDE %decoyActors}
                                BIND(?image as ?question)
                                BIND(?actorLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"),
                column: "SparqlQuery",
                value: @"
                            # What is the longest river in {continent}?
                            SELECT DISTINCT ?answer ?question 
                            WITH {
                              SELECT DISTINCT ?continent WHERE {
                                VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15  } # ohne Ozeanien
                              } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) LIMIT 1
                            } as %continent

                            WHERE {
                              { 
                                SELECT DISTINCT ?river ?continent (avg(?length2) as ?length) WHERE {
                                  INCLUDE %continent.
                                  ?river wdt:P31/wdt:P279* wd:Q55659167;
                                         wdt:P2043 ?length2;
                                         wdt:P30 ?continent.
                                }
                                group by ?river ?continent
                              }
                              OPTIONAL {
                                ?continent rdfs:label ?question;
                                           filter(lang(?question) = 'en')
                                           ?river rdfs:label ?answer ;
                                           filter(lang(?answer) = 'en')
                              }
                            }
                            order by desc(?length)
                            limit 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"),
                column: "SparqlQuery",
                value: @"# sort planets by radius
                            SELECT ?answer ?question WHERE {
                              {SELECT ?planet ?answer ?radius WHERE {
                                ?planet wdt:P397 wd:Q525;
                                        p:P2120 [
                                          ps:P2120 ?radius;
                                                   pq:P518 wd:Q23538
                                        ].
                                SERVICE wikibase:label { 
                                  bd:serviceParam wikibase:language 'en'.
                                  ?planet  rdfs:label ?answer.}
                              } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4}
                              BIND ('radius' as ?question)
                            }
                            ORDER BY ?radius");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                column: "SparqlQuery",
                value: @"
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
                                ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3609a5f7-c90a-4ecf-a713-0224fa8a4215"),
                column: "SparqlQuery",
                value: @"
                             SELECT DISTINCT(?foodLabel as ?question) (?dishOfLabel as ?answer)
                             WITH{
                                  SELECT DISTINCT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?food); SEPARATOR=', ')) as ?food) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?foodLabel); SEPARATOR=', ')) as ?foodLabel)
                                    ?dishOf
                                  WHERE{
                                        ?food (wdt:P31|wdt:P279) wd:Q746549. 
                                        ?food (wdt:P495|wdt:P17) ?dishOf.
                                        MINUS { ?dishOf pq:P582 ?endTime. }
                                        MINUS { ?dishOf wdt:P576|wdt:P582 ?end. }
                                        ?food rdfs:label ?foodLabel
                                        filter langMatches(lang(?foodLabel), 'en')
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                                 }
                                 group by ?dishOf
                                 order by ?dishOf
                                 } as %foods

                                 WITH{
                                       SELECT DISTINCT ?food (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?foodLabel); SEPARATOR=', ')) as ?foodLabel)
                                       (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?dishOf); SEPARATOR=', ')) as ?dishOf) 
                                              (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?dishOfLabel); SEPARATOR=', ')) as ?dishOfLabel)
                                   WHERE{
                                        {include %foods}
                                        {?dishOf wdt:P31 wd:Q6256.}
                                        UNION
                                        {?dishOf wdt:P31 wd:Q3624078.}
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                               ?dishOf rdfs:label ?dishOfLabel}
                                      }
                                      GROUP BY ?food
                                 } as %countryfoods

                                 WITH{
                                      SELECT *
                                      WHERE{
                                       {include %countryfoods} 
                                      }
                                      ORDER BY (MD5(CONCAT(STR(?food), STR(NOW())))) 
                                      LIMIT 1
                                 } as %selectedFood

                                 WITH{
                                    SELECT ?food ?dishOfLabel
                                         WHERE{
                                              {include %countryfoods}
                                                FILTER NOT EXISTS {include %selectedFood}
                                         }
                                      ORDER BY (MD5(CONCAT(STR(?food), STR(NOW())))) 
                                      LIMIT 3
                                } as %decoyfoods

                                WHERE{
                                     {include %selectedFood}
                                       UNION
                                     {include %decoyfoods}
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                              ?dishOf rdfs:label ?dishOfLabel}
                                }
                                order by DESC(?question)
                                    ");

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
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "SparqlQuery",
                value: @"
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
                            ");

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
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "SparqlQuery",
                value: @"
                        # Which U.S. president's signature is this: {question}
                        SELECT  ?question ?answer
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
                          BIND(?image as ?question)
                        }
                        ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3fb180e6-99ae-466b-89e9-16ac0101daed"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question ?answer
                            # series that have won an emmy
                            WITH{
                                SELECT Distinct (SAMPLE(GROUP_CONCAT(DISTINCT ?seriesLabel; SEPARATOR=', ')) as ?seriesLabel) ?characterLabel
                                WHERE{
                                  ?series wdt:P31 wd:Q5398426.
                                  ?series wdt:P166 ?award.
                                  ?award wdt:P31+/wdt:P279+ wd:Q123737.
                                  ?series p:P161/pq:P453 ?character.
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                       ?character rdfs:label ?characterLabel.
                                                       ?series rdfs:label ?seriesLabel
                                                         }
                                  FILTER NOT EXISTS{ ?series p:P161/pq:P453 wd:Q18086706}
                                }
                              group by ?characterLabel
                             } as %emmySeries

                            WITH{
                              SELECT DISTINCT ?characterLabel ?seriesLabel ?empty
                              WHERE{
                                INCLUDE %emmySeries
                              }
                              ORDER BY MD5(CONCAT(STR(?seriesLabel), STR(NOW())))
                              LIMIT 1
                            } as %selectedSeries

                            WITH{
                              SELECT ?seriesLabel
                                  WHERE{
                                    INCLUDE %selectedSeries
                                  }
                               LIMIT 1
                            } as %selectedSeriesLabel

                            WITH{
                              SELECT DISTINCT ?seriesLabel
                              WHERE{
                                INCLUDE %emmySeries.
                                FILTER NOT EXISTS {INCLUDE %selectedSeriesLabel}
                              }
                              ORDER BY MD5(CONCAT(STR(?seriesLabel), STR(NOW())))
                              LIMIT 3
                            }  as %decoySeries

                            WHERE{
                              {INCLUDE %selectedSeries}
                              UNION
                              {INCLUDE %decoySeries}
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.}
                              BIND(?characterLabel as ?question)
                              BIND(?seriesLabel as ?answer)
                            }
                            order by DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"),
                column: "SparqlQuery",
                value: @"SELECT ?question ?answer WHERE {
                          ?element wdt:P31 wd:Q11344;
                                   wdt:P1086 ?number;
                                   wdt:P246 ?question.
                          FILTER(1 <= ?number &&
                                 ?number <= 118)
                          SERVICE wikibase:label {
                            bd:serviceParam wikibase:language 'en'.
                            ?element  rdfs:label ?answer.
                          }
                        }
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
                        LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "SparqlQuery",
                value: @"SELECT DISTINCT ?question ?answer
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
                                  SELECT DISTINCT ?country ?sea WHERE {
                                      BIND(wd:Q545 AS ?sea).
                                      ?sea wdt:P205 ?country.
                                    }
                                } as %basins
                            WITH { 
                                  SELECT DISTINCT ?country ?sea WHERE {
                                      INCLUDE %basins.
                                    } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 3
                                } as %threeBasins
                            WITH {
                              SELECT DISTINCT ?country ?sea ?noSea
                                WHERE {
                                  INCLUDE %states.
                                  ?country wdt:P30 wd:Q46.
                                  BIND(wd:Q545 as ?noSea).
                                FILTER NOT EXISTS { INCLUDE %basins.}
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 1
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
                            order by DESC(?question)");

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
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                column: "SparqlQuery",
                value: @"# Which country does not border the Caribbean Sea?
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
                                  SELECT DISTINCT ?country ?sea WHERE {
                                      BIND(wd:Q1247 AS ?sea).
                                      {
                                        ?sea wdt:P205 ?country.
                                      }
                                      UNION
                                      {
                                        INCLUDE %states.
                                        ?country wdt:P361 ?region.
                                        VALUES ?region {wd:Q664609 wd:Q166131 wd:Q778 wd:Q93259 wd:Q19386 wd:Q5317255}.
                                      }
                                    } ORDER BY MD5(CONCAT(STR(?country), STR(NOW())))
                                } as %basins
                            WITH { 
                                SELECT DISTINCT ?country ?sea
                                WHERE {
                                  INCLUDE %basins.
                                    } LIMIT 3
                                } as %threeBasins
                            WITH {
                              SELECT DISTINCT ?country ?noSea
                                WHERE {
                                  INCLUDE %states.
                                  ?country wdt:P361 ?region.
                                  BIND(wd:Q1247 as ?noSea).
                                  VALUES ?region {wd:Q12585 wd:Q653884}.
                                  FILTER NOT EXISTS {?country wdt:P31 wd:Q112099.}
                                  FILTER NOT EXISTS {?country wdt:P31 wd:Q13107770.}
                                  FILTER NOT EXISTS {?country wdt:P361 wd:Q27611.}
                                  FILTER NOT EXISTS {INCLUDE %basins.}
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW())))
                              LIMIT 1
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
                column: "SparqlQuery",
                value: @"
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
                            ");

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
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"),
                column: "SparqlQuery",
                value: @"# What's the chemical symbol for {element}?
                            SELECT ?question ?answer WHERE {
                              ?element wdt:P31 wd:Q11344;
                                       wdt:P1086 ?number;
                                       wdt:P246 ?answer.
                              FILTER(1 <= ?number &&
                                     ?number <= 118)
                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?element  rdfs:label ?question.
                              }
                            }
                            ORDER BY MD5(CONCAT(STR(?question), STR(NOW()))) # order by random
                            LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"),
                column: "SparqlQuery",
                value: @"
                            #Which famous landmark is this: {image}
                            SELECT ?question ?answer
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
                                BIND(?image as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

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
                keyValue: new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"),
                column: "SparqlQuery",
                value: @"# German Chancellors
                            SELECT ?answer (CONCAT(STR(?startYear), ' to ', STR(?endYear)) AS ?question) WHERE {
                              ?person p:P39 ?Bundeskanzler.
                              ?Bundeskanzler ps:P39 wd:Q4970706;
                                             pq:P580 ?start;
                                             pq:P582 ?end. # <- note the mandatory end date

                              BIND(YEAR(?start) AS ?startYear)
                              BIND(YEAR(?end) AS ?endYear)

                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?person rdfs:label ?answer.
                              }
                            }
                            ORDER BY (MD5(CONCAT(STR(?person), STR(NOW()))))
                            LIMIT 4");

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
                keyValue: new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"),
                column: "SparqlQuery",
                value: @"# sort EU countries by the date they joined
                            SELECT ?date (SAMPLE(?answer) AS ?answer) (SAMPLE(?question) AS ?question) 
                            WITH {
                              SELECT DISTINCT (?memberOfEuSince as ?date) ?answer WHERE {
                                {SELECT ?item ?memberOfEuSince
                                              WHERE 
                                              {
                                                ?item p:P463 [ps:P463 wd:Q458;
                                                                      pq:P580 ?memberOfEuSince].
                                              }
                                }
                                SERVICE wikibase:label {
                                  bd:serviceParam wikibase:language 'en'.
                                  ?item  rdfs:label ?answer.
                                }
                              }
                            } AS %dates
                            WITH {
                              SELECT DISTINCT ?date WHERE {
                                INCLUDE %dates.
                              }
                              ORDER BY MD5(CONCAT(STR(?date), STR(NOW())))
                              LIMIT 4
                            } AS %fourDates
                            WHERE {
                              INCLUDE %fourDates.
                              INCLUDE %dates.
                              BIND('the date they joined the EU' as ?question).
                            }
                            GROUP BY ?date
                            ORDER BY ?date");

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
                keyValue: new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"),
                column: "SparqlQuery",
                value: @"# sort planets by average distance to sun
                            # NOTE: there are only three planets with apoapsis and periapsis in AU; 4 planets in total
                            SELECT ?answer ?question WHERE 
                            {
                              { SELECT DISTINCT ?answer ?avgDistanceToSun WHERE 
                                {
                                    # fetch planets in our solar system
                                    ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                    ?planet p:P2243/psv:P2243 [wikibase:quantityAmount ?apoapsis; wikibase:quantityUnit ?apoapsisUnit].
                                    ?planet p:P2244/psv:P2244 [wikibase:quantityAmount ?periapsis; wikibase:quantityUnit ?periapsisUnit].

                                    # FILTER (?apoapsisUnit = wd:Q1811 && ?periapsisUnit = wd:Q1811)
                                    BIND ((?apoapsis + ?periapsis) / 2 as ?avgDistanceToSun)
                                    FILTER (?apoapsisUnit = wd:Q828224 && ?periapsisUnit = wd:Q828224)
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?planet  rdfs:label ?answer.} 
                                } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4
                              }
                              BIND('average distance to sun' as ?question)
                            } ORDER BY ?avgDistanceToSun");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"),
                column: "SparqlQuery",
                value: @"SELECT DISTINCT ?answer ?question WHERE {
                          ?state wdt:P31/wdt:P279* wd:Q3624078;
                                 p:P463 ?memberOfStatement.
                          ?memberOfStatement a wikibase:BestRank;
                                               ps:P463 wd:Q1065.
                          MINUS { ?memberOfStatement pq:P582 ?endTime. }
                          MINUS { ?state wdt:P576|wdt:P582 ?end. }
  
                          ?state p:P36 ?capitalStatement.
                          ?capitalStatement a wikibase:BestRank;
                                              ps:P36 ?capital.
                          MINUS { ?capitalStatement pq:P582 ?capitalEnd. } # exclude former capitals
                          MINUS { ?capitalStatement pq:P459 ?capitalType. } # exclude lands that have more than one capital
                          MINUS { ?capitalStatement pq:P642 ?capitalType2. } # exclude lands that have more than one capital II
                          #MINUS { ?capital wdt:P576|wdt:P582 ?end2. }  
  
                          OPTIONAL { 
                            ?capital rdfs:label ?answer;
                            filter(lang(?answer) = 'en').
                            ?state rdfs:label ?question;
                            filter(lang(?question) = 'en').
                          }
                        } 
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
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
                column: "SparqlQuery",
                value: @"
                            SELECT  (SAMPLE(?question) AS ?question)(SAMPLE(?answer) AS ?answer) (SAMPLE(?teamLabel) AS ?team) ?soccerTeam 
                            WITH{
                              #get all teams with coaches
                              SELECT DISTINCT ?soccerTeam ?soccerTeamLabel ?coach ?coachLabel ?answer ?league ?teamLabel
                               WHERE{
                                 ?soccerTeam wdt:P31 wd:Q476028.
                                 ?soccerTeam wdt:P118 ?league.
                                 FILTER(?league IN (wd:Q82595, wd:Q35572))
                                 ?soccerTeam wdt:P286 ?coach.
                               SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language 'en'.
                                      ?coach rdfs:label ?answer.
                                      ?soccerTeam rdfs:label ?teamLabel.
                                     }
                               } 
                            } AS %allCoaches

                            WITH{
                              # get teams with coaches that have quit
                              SELECT ?soccerTeam ?coach ?endTime
                              WHERE{
                                 ?soccerTeam wdt:P31 wd:Q476028.
                                 ?soccerTeam wdt:P286 ?coach.
                                 ?coach p:P6087 [ps:P6087 ?soccerTream;
                                                          pq:P582 ?endTime]
                               } 
                            }as %endTimeCoaches   

                            #filter only teams with active coaches
                            WITH{
                              SELECT *
                              WHERE{
                              INCLUDE %allCoaches
                              FILTER NOT EXISTS { INCLUDE %endTimeCoaches. }
                              }
                            } as %activeCoaches

                            WITH{
                              #selectr a coach
                              SELECT *
                              WHERE{
                               INCLUDE %activeCoaches
                                        SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language 'en'.
                                      ?soccerTeam rdfs:label ?question.
                                     }
                              }
                              ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW())))) 
                              LIMIT 1
                            } as %selectedCoach

                            WITH{
                              #select decoy coaches that is not the selected coach
                              SELECT *
                              WHERE{
                                  INCLUDE %activeCoaches
                                  FILTER NOT EXISTS {INCLUDE %selectedCoach.}
                                }
                              ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW())))) 
                              LIMIT 3
                            } as %decoyCoaches

                            WHERE{
                              {INCLUDE %selectedCoach}
                              UNION
                              {INCLUDE %decoyCoaches}
                              BIND(?coachLabel as ?question)
                            }
                            GROUP BY ?soccerTeam
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ac86282f-1fa1-48ba-a088-f4c202ea0177"),
                column: "SparqlQuery",
                value: @"
                                    SELECT ?question ?answer 
                                    WITH{
                                       SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                       WHERE {
                                        ?item wdt:P31 wd:Q134768. 
                                        ?item wdt:P495 ?origin.
                                        ?origin wdt:P30 ?originContinent.
                                       }
                                        ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                    } AS %cocktails

                                    WITH{
                                       SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                       WHERE {
                                        ?item wdt:P31 wd:Q2536409. 
                                        ?item wdt:P495 ?origin.
                                        ?origin wdt:P30 ?originContinent.
                                        }
                                        ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                    } AS %ibaCocktails
     
                                    WITH{
                                      SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                      WHERE
                                      {
                                       {INCLUDE %cocktails}
                                       UNION
                                       {INCLUDE %ibaCocktails}
                                      }
                                       ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                       LIMIT 1
                                    } AS %selectedCountry

                                    WITH{
                                       SELECT ?originContinent
                                       WHERE{
                                         ?originContinent wdt:P31 wd:Q5107.
                                         INCLUDE %selectedCountry
                                       }
                                    } as %selectedContinent

                                    WITH{
                                      SELECT DISTINCT ?empty ?origin ?originLabel ?answer
                                      WHERE{
                                         ?origin wdt:P31 wd:Q3624078.
                                         FILTER NOT EXISTS { INCLUDE %selectedCountry. }
                                         {
                                           ?originContinent wdt:P31 wd:Q5107.
                                           INCLUDE %selectedContinent
                                         }
                                         ?origin wdt:P30 ?originContinent.
                                       }
                                      ORDER BY (MD5(CONCAT(STR(?origin), STR(NOW())))) 
                                      LIMIT 3
                                    } AS %decoyCountries

                                    WHERE{
                                      {INCLUDE %selectedCountry}
                                      UNION
                                      {INCLUDE %decoyCountries}
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                 ?item rdfs:label ?question.
                                                                 ?origin rdfs:label ?answer.
                                                               }
                                    }
                                    ORDER BY DESC(?question)
                                       ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"),
                column: "SparqlQuery",
                value: @"SELECT ?continent ?answer ?question WHERE {
                            { SELECT ?continent ?answer (COUNT(?item) AS ?question) WHERE {
                                ?item wdt:P31 wd:Q6256.
                                ?item wdt:P30 ?continent.
                                ?continent wdt:P31 wd:Q5107.
                                MINUS {VALUES ?continent {wd:Q51}}. # w/o Antarctica
                                OPTIONAL {?continent rdfs:label ?answer ;
                                                    filter(lang(?answer) = 'en')
                                        }
                                } GROUP BY ?continent ?answer}
                            }
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                            LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "SparqlQuery",
                value: @"
                                SELECT ?question ?answer
                                WITH{
                                SELECT DISTINCT ?painting ?image ?paintingLabel
                                       WHERE {
                                         ?painting wdt:P1343 wd:Q66362718.
                                         ?painting wdt:P18 ?image.
                                         SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                 ?painting rdfs:label ?paintingLabel}
                                         filter(lang(?paintingLabel) = 'en').
                                         BIND(REPLACE(?paintingLabel,'-',' ') AS ?paintingLabel) .
                                       }
                                } as %allPaintings

                                WITH{
                                  SELECT DISTINCT ?painting ?paintingLabel ?image
                                         WHERE{
                                           INCLUDE %allPaintings.
                                         }
                                  ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW()))))
                                  LIMIT 1
                                } as %selectedPainting

                                WITH{
                                  select ?paintingLabel
                                  where{
                                     INCLUDE %selectedPainting.
                                  }
                                } as %selectedName

                                WITH{
                                  SELECT DISTINCT ?paintingLabel
                                         WHERE{
                                           INCLUDE %allPaintings.
                                           FILTER NOT EXISTS{
                                             INCLUDE %selectedName
                                           }
                                         }
                                  ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW()))))
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"),
                column: "SparqlQuery",
                value: @"
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
                keyValue: new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"),
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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"),
                column: "SparqlQuery",
                value: @"# sort countries by number of inhabitants (ascending)
                            SELECT (?stateLabel AS ?answer) ?question
                            WITH {
                              # subquery: get 4 random countries with their average number of inhabitants
                              SELECT DISTINCT ?state ?stateLabel (ROUND(AVG(?population) / 1000) * 1000 AS ?population) {

                                {
                                  # subquery: list of all countries in the world
                                  SELECT DISTINCT ?state ?stateLabel ?population ?dateOfCensus WHERE {
                                    ?state wdt:P31/wdt:P279* wd:Q3624078;
                                           p:P463 ?memberOfStatement;
                                           p:P1082 [
                                             ps:P1082 ?population;
                                                      pq:P585 ?dateOfCensus
                                           ].
                                    ?memberOfStatement a wikibase:BestRank;
                                                         ps:P463 wd:Q1065.
                                    MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                    MINUS { ?state wdt:P576|wdt:P582 ?end. }
                                    ?state p:P30 ?continentStatement.
                                    ?continentStatement a wikibase:BestRank;
                                                          ps:P30 ?continent.
                                    VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                                    MINUS { ?continentStatement pq:P582 ?endTime. }
                                    SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'.
                                    }
                                    FILTER(YEAR(?dateOfCensus) > YEAR(NOW()) - 5)
                                  }
                                }
                              } GROUP BY ?state ?stateLabel
                            } AS %allStates

                            WITH {
                              SELECT DISTINCT ?state ?stateLabel ?population WHERE {
                                INCLUDE %allStates.
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) LIMIT 4
                            } AS %states

                            WHERE {
                              # fill the question (hard-coded) and sort by population (= correct sort order needed for sorting game)
                              INCLUDE %states.
                              BIND('number of inhabitants' AS ?question).
                            } ORDER BY ?population");

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
                keyValue: new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"),
                column: "SparqlQuery",
                value: @"# empires and colonies
                            SELECT DISTINCT ?empire (?empireLabel as ?question) ?colony (?colonyLabel as ?answer)
                            WITH {
                                SELECT DISTINCT ?empire ?empireLabel ?colony ?colonyLabel WHERE {
                                {
                                    SELECT ?empire ?empireLabel ?colony ?colonyLabel WHERE {
                                    ?empire (wdt:P31/(wdt:P279*)) wd:Q1790360.
                                    ?colony wdt:P361 ?empire;
                                            wdt:P31 wd:Q133156;
                                            wdt:P576 ?enddate.
                                    FILTER(?enddate >= '1790-01-01T00:00:00Z'^^xsd:dateTime)
                                    SERVICE wikibase:label {
                                        bd:serviceParam wikibase:language 'en'.
                                        ?empire rdfs:label ?empireLabel.
                                        ?colony rdfs:label ?colonyLabel.
                                    }
                                    }
                                }
                                UNION
                                {
                                    SELECT DISTINCT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                    ?colony (wdt:P31/(wdt:P279*)) wd:Q133156;
                                                                    wdt:P576 ?enddate;
                                                                    wdt:P17 ?empire.
                                    VALUES ?empire {
                                        wd:Q8680
                                    }
                                    SERVICE wikibase:label { 
                                        bd:serviceParam wikibase:language 'en'. 
                                        ?empire rdfs:label ?empireLabel.
                                        ?colony rdfs:label ?colonyLabel.
                                    }
                                    FILTER(?enddate >= '1790-01-01T00:00:00Z'^^xsd:dateTime)
                                    }
                                }
                                FILTER(!(CONTAINS(?colonyLabel, 'Q')))
                                }
                            } as %colonies

                            WITH {
                                SELECT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                INCLUDE %colonies.
                                {
                                    SELECT ?empire WHERE {
                                    INCLUDE %colonies.
                                    } GROUP BY ?empire 
                                    ORDER BY (MD5(CONCAT(STR(?empire), STR(NOW()))))
                                    LIMIT 1
                                }
                                }
                            } as %selectedEmpire

                            WITH {
                                SELECT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                INCLUDE %selectedEmpire.
                                } ORDER BY (MD5(CONCAT(STR(?colony), STR(NOW())))) LIMIT 1
                            } as %selectedColony

                            WITH {
                                SELECT ?colony ?colonyLabel ?empty ?emptyLabel WHERE {
                                INCLUDE %colonies.
                                MINUS {INCLUDE %selectedEmpire.}.
    
                                } ORDER BY (MD5(CONCAT(STR(?colony), STR(NOW())))) LIMIT 3
                            } as %threeOtherColonies

                            WHERE {
                                {INCLUDE %selectedColony.}
                                UNION
                                {INCLUDE %threeOtherColonies.}
                            } ORDER BY DESC(?empire)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "SparqlQuery",
                value: @"
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
                keyValue: new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"),
                column: "SparqlQuery",
                value: @"#sort chemical elements by number in period system
                            SELECT ?question ?answer WHERE {
                              BIND ('number in period system' as ?question).
                              {
                                SELECT ?item ?element ?number ?symbol WHERE {
                                  ?item wdt:P31 wd:Q11344;
                                        wdt:P1086 ?number;
                                        wdt:P246 ?symbol.
                                  FILTER(1 <= ?number &&
                                         ?number <= 118)
                                  SERVICE wikibase:label {
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?element.
                                  }
                                }
                                ORDER BY MD5(CONCAT(STR(?element), STR(NOW()))) # order by random
                                LIMIT 4
                              }
                              BIND (?element as ?answer).
                            } ORDER BY ASC(?number)");

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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("f64d3784-3584-4f19-be07-aa44fd9a4086"),
                column: "SparqlQuery",
                value: @"
                            # Based on the example question 'Former capitals''

                            SELECT (SAMPLE(?image) AS ?question) ?answer
                            WHERE
                            {
                              # Fetches capitals from current or former members of the U.N.
                              # Capital is always the most recent capital, states are always only existing states
                              ?country wdt:P463 wd:Q1065.
                              ?country p:P36 ?stat.
                              ?stat ps:P36 ?capital.


                              ?country rdfs:label ?countryLabel.
                              ?capital rdfs:label ?capitalLabel.


                              ?capital wdt:P18 ?image.

                              OPTIONAL {
                                ?country wdt:P582|wdt:P576 ?ended.
                              }
                              OPTIONAL {
                                ?capital wdt:P582|wdt:P576 ?ended.
                              }
                              OPTIONAL {
                                ?stat pq:P582 ?ended.
                              }


                              FILTER(!BOUND(?ended)).
                              FILTER(LANG(?countryLabel) = 'en').
                              FILTER(LANG(?capitalLabel) = 'en').

                              BIND(CONCAT(?capitalLabel, ', ', ?countryLabel) as ?answer).
                              BIND('Where are we?' AS ?question).
                            }
                            GROUP BY ?question ?answer
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) #order by random
                            LIMIT 4
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fa1e5667-cc80-4526-99bd-fd4fc539d4fd"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer
                            WITH {
                                 SELECT ?sport ?sportLabel ?image
                                 WHERE {
	                                   ?sport wdt:P31 wd:Q31629.
                                       ?sport wdt:P18 ?image
                                       SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                ?sport rdfs:label ?sL
                                                              }
                                 filter(lang(?sportLabel) = 'en').
                                 BIND(CONCAT(UCASE(SUBSTR(?sL, 1, 1)), SUBSTR(?sL, 2)) as ?sportLabel)
                                 }
                                 ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                            } as %allSports
                        
                            WITH{
                            SELECT DISTINCT ?sportLabel ?image
                                    WHERE {
                                      INCLUDE %allSports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                                    LIMIT 1
                            } AS %selectedSport

                            WITH{
                              SELECT DISTINCT ?sportLabel
                              WHERE{
                                INCLUDE %allSports.
                                FILTER NOT EXISTS {INCLUDE %selectedSport}
    
                              }
                              ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                              LIMIT 3
                            } as %decoySport

                            WHERE {
                                   {INCLUDE %selectedSport}
                                    union
                                   {INCLUDE %decoySport}
                                   BIND(?image as ?question)
                                   BIND(?sportLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ");

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
                value: "40bbb96b-16ea-439c-9724-b52a068a0b5b");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"),
                column: "SparqlQuery",
                value: @"# wars of the 20th century
                            SELECT (SAMPLE(?itemLabel) AS ?answer) (YEAR(MAX(?startdate)) AS ?question) WHERE {
                              {
                                SELECT ?item ?itemLabel ?startdate WHERE {
                                  ?item (wdt:P31/(wdt:P279*)) wd:Q198.
                                  ?item wdt:P580 ?startdate.
                                  FILTER(?startdate >= '1900-01-01T00:00:00Z'^^xsd:dateTime)
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                                }
                              }
                              FILTER(!(CONTAINS(?itemLabel, '1')))
                              FILTER(!(CONTAINS(?itemLabel, '2')))
                              FILTER(!(STRSTARTS(?itemLabel, 'Q')))
                            }
                            GROUP BY ?itemLabel
                            ORDER BY (MD5(CONCAT(STR(?item), STR(NOW()))))
                            LIMIT 4");

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
                keyValue: new Guid("1e5551e1-a6a3-42df-8447-b0ac0effc8e6"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer
                            WITH{
                              SELECT DISTINCT ?actor ?actorLabel
                            WHERE{
                              ?actor wdt:P31 wd:Q5;
                                     wdt:P106 wd:Q33999;
                                     wdt:P166 ?award.
                              ?award wdt:P31+ wd:Q19020.
                              ?actor wdt:P18 ?image
                              SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?movie rdfs:label ?movieLabel.
                                                     ?actor rdfs:label ?actorLabel.
                                                     ?award rdfs:label ?awardLabel}
                            }
                              ORDER BY ?actor ?actorLabel
                            } as %allWinners
                                       
                            WITH{
                              SELECT ?actor ?actorLabel ?image
                              WHERE{
                                 INCLUDE %allWinners.
                                ?actor wdt:P18 ?image
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
                              SELECT DISTINCT ?actor ?actorLabel
                              WHERE{
                                INCLUDE %allWinners
                                Filter NOT EXISTS {INCLUDE %selectedActor}
                                ?actor wdt:P21 ?gender
                                Filter NOT EXISTS {
                                  include %filteredGenders
                                }
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?actor rdfs:label ?actorLabel.}
                              }
                              ORDER BY MD5(CONCAT(STR(?actor), STR(NOW())))
                              LIMIT 3
                            } as %decoyActors

                            WHERE{
                                {INCLUDE %selectedActor}
                                UNION
                                {INCLUDE %decoyActors}
                                BIND(?image as ?question)
                                BIND(?actorLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"),
                column: "SparqlQuery",
                value: @"
                            # What is the longest river in {continent}?
                            SELECT DISTINCT ?answer ?question 
                            WITH {
                              SELECT DISTINCT ?continent WHERE {
                                VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15  } # ohne Ozeanien
                              } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) LIMIT 1
                            } as %continent

                            WHERE {
                              { 
                                SELECT DISTINCT ?river ?continent (avg(?length2) as ?length) WHERE {
                                  INCLUDE %continent.
                                  ?river wdt:P31/wdt:P279* wd:Q55659167;
                                         wdt:P2043 ?length2;
                                         wdt:P30 ?continent.
                                }
                                group by ?river ?continent
                              }
                              OPTIONAL {
                                ?continent rdfs:label ?question;
                                           filter(lang(?question) = 'en')
                                           ?river rdfs:label ?answer ;
                                           filter(lang(?answer) = 'en')
                              }
                            }
                            order by desc(?length)
                            limit 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"),
                column: "SparqlQuery",
                value: @"# sort planets by radius
                            SELECT ?answer ?question WHERE {
                              {SELECT ?planet ?answer ?radius WHERE {
                                ?planet wdt:P397 wd:Q525;
                                        p:P2120 [
                                          ps:P2120 ?radius;
                                                   pq:P518 wd:Q23538
                                        ].
                                SERVICE wikibase:label { 
                                  bd:serviceParam wikibase:language 'en'.
                                  ?planet  rdfs:label ?answer.}
                              } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4}
                              BIND ('radius' as ?question)
                            }
                            ORDER BY ?radius");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                column: "SparqlQuery",
                value: @"
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
                                ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3609a5f7-c90a-4ecf-a713-0224fa8a4215"),
                column: "SparqlQuery",
                value: @"
                             SELECT DISTINCT(?foodLabel as ?question) (?dishOfLabel as ?answer)
                             WITH{
                                  SELECT DISTINCT (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?food); SEPARATOR=', ')) as ?food) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?foodLabel); SEPARATOR=', ')) as ?foodLabel)
                                    ?dishOf
                                  WHERE{
                                        ?food (wdt:P31|wdt:P279) wd:Q746549. 
                                        ?food (wdt:P495|wdt:P17) ?dishOf.
                                        MINUS { ?dishOf pq:P582 ?endTime. }
                                        MINUS { ?dishOf wdt:P576|wdt:P582 ?end. }
                                        ?food rdfs:label ?foodLabel
                                        filter langMatches(lang(?foodLabel), 'en')
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. }
                                 }
                                 group by ?dishOf
                                 order by ?dishOf
                                 } as %foods

                                 WITH{
                                       SELECT DISTINCT ?food (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?foodLabel); SEPARATOR=', ')) as ?foodLabel)
                                       (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?dishOf); SEPARATOR=', ')) as ?dishOf) 
                                              (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?dishOfLabel); SEPARATOR=', ')) as ?dishOfLabel)
                                   WHERE{
                                        {include %foods}
                                        {?dishOf wdt:P31 wd:Q6256.}
                                        UNION
                                        {?dishOf wdt:P31 wd:Q3624078.}
                                        SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                               ?dishOf rdfs:label ?dishOfLabel}
                                      }
                                      GROUP BY ?food
                                 } as %countryfoods

                                 WITH{
                                      SELECT *
                                      WHERE{
                                       {include %countryfoods} 
                                      }
                                      ORDER BY (MD5(CONCAT(STR(?food), STR(NOW())))) 
                                      LIMIT 1
                                 } as %selectedFood

                                 WITH{
                                    SELECT ?food ?dishOfLabel
                                         WHERE{
                                              {include %countryfoods}
                                                FILTER NOT EXISTS {include %selectedFood}
                                         }
                                      ORDER BY (MD5(CONCAT(STR(?food), STR(NOW())))) 
                                      LIMIT 3
                                } as %decoyfoods

                                WHERE{
                                     {include %selectedFood}
                                       UNION
                                     {include %decoyfoods}
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. 
                                                              ?dishOf rdfs:label ?dishOfLabel}
                                }
                                order by DESC(?question)
                                    ");

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
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "SparqlQuery",
                value: @"
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
                            ");

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
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "SparqlQuery",
                value: @"
                        # Which U.S. president's signature is this: {question}
                        SELECT  ?question ?answer
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
                          BIND(?image as ?question)
                        }
                        ORDER BY DESC(?question)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3fb180e6-99ae-466b-89e9-16ac0101daed"),
                column: "SparqlQuery",
                value: @"
                            SELECT ?question ?answer
                            # series that have won an emmy
                            WITH{
                                SELECT Distinct (SAMPLE(GROUP_CONCAT(DISTINCT ?seriesLabel; SEPARATOR=', ')) as ?seriesLabel) ?characterLabel
                                WHERE{
                                  ?series wdt:P31 wd:Q5398426.
                                  ?series wdt:P166 ?award.
                                  ?award wdt:P31+/wdt:P279+ wd:Q123737.
                                  ?series p:P161/pq:P453 ?character.
                                  SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                       ?character rdfs:label ?characterLabel.
                                                       ?series rdfs:label ?seriesLabel
                                                         }
                                  FILTER NOT EXISTS{ ?series p:P161/pq:P453 wd:Q18086706}
                                }
                              group by ?characterLabel
                             } as %emmySeries

                            WITH{
                              SELECT DISTINCT ?characterLabel ?seriesLabel ?empty
                              WHERE{
                                INCLUDE %emmySeries
                              }
                              ORDER BY MD5(CONCAT(STR(?seriesLabel), STR(NOW())))
                              LIMIT 1
                            } as %selectedSeries

                            WITH{
                              SELECT ?seriesLabel
                                  WHERE{
                                    INCLUDE %selectedSeries
                                  }
                               LIMIT 1
                            } as %selectedSeriesLabel

                            WITH{
                              SELECT DISTINCT ?seriesLabel
                              WHERE{
                                INCLUDE %emmySeries.
                                FILTER NOT EXISTS {INCLUDE %selectedSeriesLabel}
                              }
                              ORDER BY MD5(CONCAT(STR(?seriesLabel), STR(NOW())))
                              LIMIT 3
                            }  as %decoySeries

                            WHERE{
                              {INCLUDE %selectedSeries}
                              UNION
                              {INCLUDE %decoySeries}
                                SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.}
                              BIND(?characterLabel as ?question)
                              BIND(?seriesLabel as ?answer)
                            }
                            order by DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"),
                column: "SparqlQuery",
                value: @"SELECT ?question ?answer WHERE {
                          ?element wdt:P31 wd:Q11344;
                                   wdt:P1086 ?number;
                                   wdt:P246 ?question.
                          FILTER(1 <= ?number &&
                                 ?number <= 118)
                          SERVICE wikibase:label {
                            bd:serviceParam wikibase:language 'en'.
                            ?element  rdfs:label ?answer.
                          }
                        }
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
                        LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "SparqlQuery",
                value: @"SELECT DISTINCT ?question ?answer
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
                                  SELECT DISTINCT ?country ?sea WHERE {
                                      BIND(wd:Q545 AS ?sea).
                                      ?sea wdt:P205 ?country.
                                    }
                                } as %basins
                            WITH { 
                                  SELECT DISTINCT ?country ?sea WHERE {
                                      INCLUDE %basins.
                                    } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 3
                                } as %threeBasins
                            WITH {
                              SELECT DISTINCT ?country ?sea ?noSea
                                WHERE {
                                  INCLUDE %states.
                                  ?country wdt:P30 wd:Q46.
                                  BIND(wd:Q545 as ?noSea).
                                FILTER NOT EXISTS { INCLUDE %basins.}
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW()))) LIMIT 1
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
                            order by DESC(?question)");

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
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                column: "SparqlQuery",
                value: @"# Which country does not border the Caribbean Sea?
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
                                  SELECT DISTINCT ?country ?sea WHERE {
                                      BIND(wd:Q1247 AS ?sea).
                                      {
                                        ?sea wdt:P205 ?country.
                                      }
                                      UNION
                                      {
                                        INCLUDE %states.
                                        ?country wdt:P361 ?region.
                                        VALUES ?region {wd:Q664609 wd:Q166131 wd:Q778 wd:Q93259 wd:Q19386 wd:Q5317255}.
                                      }
                                    } ORDER BY MD5(CONCAT(STR(?country), STR(NOW())))
                                } as %basins
                            WITH { 
                                SELECT DISTINCT ?country ?sea
                                WHERE {
                                  INCLUDE %basins.
                                    } LIMIT 3
                                } as %threeBasins
                            WITH {
                              SELECT DISTINCT ?country ?noSea
                                WHERE {
                                  INCLUDE %states.
                                  ?country wdt:P361 ?region.
                                  BIND(wd:Q1247 as ?noSea).
                                  VALUES ?region {wd:Q12585 wd:Q653884}.
                                  FILTER NOT EXISTS {?country wdt:P31 wd:Q112099.}
                                  FILTER NOT EXISTS {?country wdt:P31 wd:Q13107770.}
                                  FILTER NOT EXISTS {?country wdt:P361 wd:Q27611.}
                                  FILTER NOT EXISTS {INCLUDE %basins.}
                              } ORDER BY MD5(CONCAT(STR(?country), STR(NOW())))
                              LIMIT 1
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
                column: "SparqlQuery",
                value: @"
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
                            ");

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
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"),
                column: "SparqlQuery",
                value: @"# What's the chemical symbol for {element}?
                            SELECT ?question ?answer WHERE {
                              ?element wdt:P31 wd:Q11344;
                                       wdt:P1086 ?number;
                                       wdt:P246 ?answer.
                              FILTER(1 <= ?number &&
                                     ?number <= 118)
                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?element  rdfs:label ?question.
                              }
                            }
                            ORDER BY MD5(CONCAT(STR(?question), STR(NOW()))) # order by random
                            LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"),
                column: "SparqlQuery",
                value: @"
                            #Which famous landmark is this: {image}
                            SELECT ?question ?answer
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
                                BIND(?image as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

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
                keyValue: new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"),
                column: "SparqlQuery",
                value: @"# German Chancellors
                            SELECT ?answer (CONCAT(STR(?startYear), ' to ', STR(?endYear)) AS ?question) WHERE {
                              ?person p:P39 ?Bundeskanzler.
                              ?Bundeskanzler ps:P39 wd:Q4970706;
                                             pq:P580 ?start;
                                             pq:P582 ?end. # <- note the mandatory end date

                              BIND(YEAR(?start) AS ?startYear)
                              BIND(YEAR(?end) AS ?endYear)

                              SERVICE wikibase:label {
                                bd:serviceParam wikibase:language 'en'.
                                ?person rdfs:label ?answer.
                              }
                            }
                            ORDER BY (MD5(CONCAT(STR(?person), STR(NOW()))))
                            LIMIT 4");

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
                keyValue: new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"),
                column: "SparqlQuery",
                value: @"# sort EU countries by the date they joined
                            SELECT ?date (SAMPLE(?answer) AS ?answer) (SAMPLE(?question) AS ?question) 
                            WITH {
                              SELECT DISTINCT (?memberOfEuSince as ?date) ?answer WHERE {
                                {SELECT ?item ?memberOfEuSince
                                              WHERE 
                                              {
                                                ?item p:P463 [ps:P463 wd:Q458;
                                                                      pq:P580 ?memberOfEuSince].
                                              }
                                }
                                SERVICE wikibase:label {
                                  bd:serviceParam wikibase:language 'en'.
                                  ?item  rdfs:label ?answer.
                                }
                              }
                            } AS %dates
                            WITH {
                              SELECT DISTINCT ?date WHERE {
                                INCLUDE %dates.
                              }
                              ORDER BY MD5(CONCAT(STR(?date), STR(NOW())))
                              LIMIT 4
                            } AS %fourDates
                            WHERE {
                              INCLUDE %fourDates.
                              INCLUDE %dates.
                              BIND('the date they joined the EU' as ?question).
                            }
                            GROUP BY ?date
                            ORDER BY ?date");

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
                keyValue: new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"),
                column: "SparqlQuery",
                value: @"# sort planets by average distance to sun
                            # NOTE: there are only three planets with apoapsis and periapsis in AU; 4 planets in total
                            SELECT ?answer ?question WHERE 
                            {
                              { SELECT DISTINCT ?answer ?avgDistanceToSun WHERE 
                                {
                                    # fetch planets in our solar system
                                    ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                    ?planet p:P2243/psv:P2243 [wikibase:quantityAmount ?apoapsis; wikibase:quantityUnit ?apoapsisUnit].
                                    ?planet p:P2244/psv:P2244 [wikibase:quantityAmount ?periapsis; wikibase:quantityUnit ?periapsisUnit].

                                    # FILTER (?apoapsisUnit = wd:Q1811 && ?periapsisUnit = wd:Q1811)
                                    BIND ((?apoapsis + ?periapsis) / 2 as ?avgDistanceToSun)
                                    FILTER (?apoapsisUnit = wd:Q828224 && ?periapsisUnit = wd:Q828224)
                                    SERVICE wikibase:label { 
                                    bd:serviceParam wikibase:language 'en'.
                                    ?planet  rdfs:label ?answer.} 
                                } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4
                              }
                              BIND('average distance to sun' as ?question)
                            } ORDER BY ?avgDistanceToSun");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"),
                column: "SparqlQuery",
                value: @"SELECT DISTINCT ?answer ?question WHERE {
                          ?state wdt:P31/wdt:P279* wd:Q3624078;
                                 p:P463 ?memberOfStatement.
                          ?memberOfStatement a wikibase:BestRank;
                                               ps:P463 wd:Q1065.
                          MINUS { ?memberOfStatement pq:P582 ?endTime. }
                          MINUS { ?state wdt:P576|wdt:P582 ?end. }
  
                          ?state p:P36 ?capitalStatement.
                          ?capitalStatement a wikibase:BestRank;
                                              ps:P36 ?capital.
                          MINUS { ?capitalStatement pq:P582 ?capitalEnd. } # exclude former capitals
                          MINUS { ?capitalStatement pq:P459 ?capitalType. } # exclude lands that have more than one capital
                          MINUS { ?capitalStatement pq:P642 ?capitalType2. } # exclude lands that have more than one capital II
                          #MINUS { ?capital wdt:P576|wdt:P582 ?end2. }  
  
                          OPTIONAL { 
                            ?capital rdfs:label ?answer;
                            filter(lang(?answer) = 'en').
                            ?state rdfs:label ?question;
                            filter(lang(?question) = 'en').
                          }
                        } 
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
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
                column: "SparqlQuery",
                value: @"
                            SELECT  (SAMPLE(?question) AS ?question)(SAMPLE(?answer) AS ?answer) (SAMPLE(?teamLabel) AS ?team) ?soccerTeam 
                            WITH{
                              #get all teams with coaches
                              SELECT DISTINCT ?soccerTeam ?soccerTeamLabel ?coach ?coachLabel ?answer ?league ?teamLabel
                               WHERE{
                                 ?soccerTeam wdt:P31 wd:Q476028.
                                 ?soccerTeam wdt:P118 ?league.
                                 FILTER(?league IN (wd:Q82595, wd:Q35572))
                                 ?soccerTeam wdt:P286 ?coach.
                               SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language 'en'.
                                      ?coach rdfs:label ?answer.
                                      ?soccerTeam rdfs:label ?teamLabel.
                                     }
                               } 
                            } AS %allCoaches

                            WITH{
                              # get teams with coaches that have quit
                              SELECT ?soccerTeam ?coach ?endTime
                              WHERE{
                                 ?soccerTeam wdt:P31 wd:Q476028.
                                 ?soccerTeam wdt:P286 ?coach.
                                 ?coach p:P6087 [ps:P6087 ?soccerTream;
                                                          pq:P582 ?endTime]
                               } 
                            }as %endTimeCoaches   

                            #filter only teams with active coaches
                            WITH{
                              SELECT *
                              WHERE{
                              INCLUDE %allCoaches
                              FILTER NOT EXISTS { INCLUDE %endTimeCoaches. }
                              }
                            } as %activeCoaches

                            WITH{
                              #selectr a coach
                              SELECT *
                              WHERE{
                               INCLUDE %activeCoaches
                                        SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language 'en'.
                                      ?soccerTeam rdfs:label ?question.
                                     }
                              }
                              ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW())))) 
                              LIMIT 1
                            } as %selectedCoach

                            WITH{
                              #select decoy coaches that is not the selected coach
                              SELECT *
                              WHERE{
                                  INCLUDE %activeCoaches
                                  FILTER NOT EXISTS {INCLUDE %selectedCoach.}
                                }
                              ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW())))) 
                              LIMIT 3
                            } as %decoyCoaches

                            WHERE{
                              {INCLUDE %selectedCoach}
                              UNION
                              {INCLUDE %decoyCoaches}
                              BIND(?coachLabel as ?question)
                            }
                            GROUP BY ?soccerTeam
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ac86282f-1fa1-48ba-a088-f4c202ea0177"),
                column: "SparqlQuery",
                value: @"
                                    SELECT ?question ?answer 
                                    WITH{
                                       SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                       WHERE {
                                        ?item wdt:P31 wd:Q134768. 
                                        ?item wdt:P495 ?origin.
                                        ?origin wdt:P30 ?originContinent.
                                       }
                                        ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                    } AS %cocktails

                                    WITH{
                                       SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                       WHERE {
                                        ?item wdt:P31 wd:Q2536409. 
                                        ?item wdt:P495 ?origin.
                                        ?origin wdt:P30 ?originContinent.
                                        }
                                        ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                    } AS %ibaCocktails
     
                                    WITH{
                                      SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                                      WHERE
                                      {
                                       {INCLUDE %cocktails}
                                       UNION
                                       {INCLUDE %ibaCocktails}
                                      }
                                       ORDER BY MD5(CONCAT(STR(?item), STR(NOW())))
                                       LIMIT 1
                                    } AS %selectedCountry

                                    WITH{
                                       SELECT ?originContinent
                                       WHERE{
                                         ?originContinent wdt:P31 wd:Q5107.
                                         INCLUDE %selectedCountry
                                       }
                                    } as %selectedContinent

                                    WITH{
                                      SELECT DISTINCT ?empty ?origin ?originLabel ?answer
                                      WHERE{
                                         ?origin wdt:P31 wd:Q3624078.
                                         FILTER NOT EXISTS { INCLUDE %selectedCountry. }
                                         {
                                           ?originContinent wdt:P31 wd:Q5107.
                                           INCLUDE %selectedContinent
                                         }
                                         ?origin wdt:P30 ?originContinent.
                                       }
                                      ORDER BY (MD5(CONCAT(STR(?origin), STR(NOW())))) 
                                      LIMIT 3
                                    } AS %decoyCountries

                                    WHERE{
                                      {INCLUDE %selectedCountry}
                                      UNION
                                      {INCLUDE %decoyCountries}
                                      SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                 ?item rdfs:label ?question.
                                                                 ?origin rdfs:label ?answer.
                                                               }
                                    }
                                    ORDER BY DESC(?question)
                                       ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"),
                column: "SparqlQuery",
                value: @"SELECT ?continent ?answer ?question WHERE {
                            { SELECT ?continent ?answer (COUNT(?item) AS ?question) WHERE {
                                ?item wdt:P31 wd:Q6256.
                                ?item wdt:P30 ?continent.
                                ?continent wdt:P31 wd:Q5107.
                                MINUS {VALUES ?continent {wd:Q51}}. # w/o Antarctica
                                OPTIONAL {?continent rdfs:label ?answer ;
                                                    filter(lang(?answer) = 'en')
                                        }
                                } GROUP BY ?continent ?answer}
                            }
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                            LIMIT 4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "SparqlQuery",
                value: @"
                                SELECT ?question ?answer
                                WITH{
                                SELECT DISTINCT ?painting ?image ?paintingLabel
                                       WHERE {
                                         ?painting wdt:P1343 wd:Q66362718.
                                         ?painting wdt:P18 ?image.
                                         SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                 ?painting rdfs:label ?paintingLabel}
                                         filter(lang(?paintingLabel) = 'en').
                                         BIND(REPLACE(?paintingLabel,'-',' ') AS ?paintingLabel) .
                                       }
                                } as %allPaintings

                                WITH{
                                  SELECT DISTINCT ?painting ?paintingLabel ?image
                                         WHERE{
                                           INCLUDE %allPaintings.
                                         }
                                  ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW()))))
                                  LIMIT 1
                                } as %selectedPainting

                                WITH{
                                  select ?paintingLabel
                                  where{
                                     INCLUDE %selectedPainting.
                                  }
                                } as %selectedName

                                WITH{
                                  SELECT DISTINCT ?paintingLabel
                                         WHERE{
                                           INCLUDE %allPaintings.
                                           FILTER NOT EXISTS{
                                             INCLUDE %selectedName
                                           }
                                         }
                                  ORDER BY (MD5(CONCAT(STR(?painting), STR(NOW()))))
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "SparqlQuery",
                value: @"
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
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"),
                column: "SparqlQuery",
                value: @"
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
                keyValue: new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"),
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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"),
                column: "SparqlQuery",
                value: @"# sort countries by number of inhabitants (ascending)
                            SELECT (?stateLabel AS ?answer) ?question
                            WITH {
                              # subquery: get 4 random countries with their average number of inhabitants
                              SELECT DISTINCT ?state ?stateLabel (ROUND(AVG(?population) / 1000) * 1000 AS ?population) {

                                {
                                  # subquery: list of all countries in the world
                                  SELECT DISTINCT ?state ?stateLabel ?population ?dateOfCensus WHERE {
                                    ?state wdt:P31/wdt:P279* wd:Q3624078;
                                           p:P463 ?memberOfStatement;
                                           p:P1082 [
                                             ps:P1082 ?population;
                                                      pq:P585 ?dateOfCensus
                                           ].
                                    ?memberOfStatement a wikibase:BestRank;
                                                         ps:P463 wd:Q1065.
                                    MINUS { ?memberOfStatement pq:P582 ?endTime. }
                                    MINUS { ?state wdt:P576|wdt:P582 ?end. }
                                    ?state p:P30 ?continentStatement.
                                    ?continentStatement a wikibase:BestRank;
                                                          ps:P30 ?continent.
                                    VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 } # ohne Ozeanien
                                    MINUS { ?continentStatement pq:P582 ?endTime. }
                                    SERVICE wikibase:label {
                                      bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'.
                                    }
                                    FILTER(YEAR(?dateOfCensus) > YEAR(NOW()) - 5)
                                  }
                                }
                              } GROUP BY ?state ?stateLabel
                            } AS %allStates

                            WITH {
                              SELECT DISTINCT ?state ?stateLabel ?population WHERE {
                                INCLUDE %allStates.
                              } ORDER BY MD5(CONCAT(STR(?state), STR(NOW()))) LIMIT 4
                            } AS %states

                            WHERE {
                              # fill the question (hard-coded) and sort by population (= correct sort order needed for sorting game)
                              INCLUDE %states.
                              BIND('number of inhabitants' AS ?question).
                            } ORDER BY ?population");

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
                keyValue: new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"),
                column: "SparqlQuery",
                value: @"# empires and colonies
                            SELECT DISTINCT ?empire (?empireLabel as ?question) ?colony (?colonyLabel as ?answer)
                            WITH {
                                SELECT DISTINCT ?empire ?empireLabel ?colony ?colonyLabel WHERE {
                                {
                                    SELECT ?empire ?empireLabel ?colony ?colonyLabel WHERE {
                                    ?empire (wdt:P31/(wdt:P279*)) wd:Q1790360.
                                    ?colony wdt:P361 ?empire;
                                            wdt:P31 wd:Q133156;
                                            wdt:P576 ?enddate.
                                    FILTER(?enddate >= '1790-01-01T00:00:00Z'^^xsd:dateTime)
                                    SERVICE wikibase:label {
                                        bd:serviceParam wikibase:language 'en'.
                                        ?empire rdfs:label ?empireLabel.
                                        ?colony rdfs:label ?colonyLabel.
                                    }
                                    }
                                }
                                UNION
                                {
                                    SELECT DISTINCT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                    ?colony (wdt:P31/(wdt:P279*)) wd:Q133156;
                                                                    wdt:P576 ?enddate;
                                                                    wdt:P17 ?empire.
                                    VALUES ?empire {
                                        wd:Q8680
                                    }
                                    SERVICE wikibase:label { 
                                        bd:serviceParam wikibase:language 'en'. 
                                        ?empire rdfs:label ?empireLabel.
                                        ?colony rdfs:label ?colonyLabel.
                                    }
                                    FILTER(?enddate >= '1790-01-01T00:00:00Z'^^xsd:dateTime)
                                    }
                                }
                                FILTER(!(CONTAINS(?colonyLabel, 'Q')))
                                }
                            } as %colonies

                            WITH {
                                SELECT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                INCLUDE %colonies.
                                {
                                    SELECT ?empire WHERE {
                                    INCLUDE %colonies.
                                    } GROUP BY ?empire 
                                    ORDER BY (MD5(CONCAT(STR(?empire), STR(NOW()))))
                                    LIMIT 1
                                }
                                }
                            } as %selectedEmpire

                            WITH {
                                SELECT ?colony ?colonyLabel ?empire ?empireLabel WHERE {
                                INCLUDE %selectedEmpire.
                                } ORDER BY (MD5(CONCAT(STR(?colony), STR(NOW())))) LIMIT 1
                            } as %selectedColony

                            WITH {
                                SELECT ?colony ?colonyLabel ?empty ?emptyLabel WHERE {
                                INCLUDE %colonies.
                                MINUS {INCLUDE %selectedEmpire.}.
    
                                } ORDER BY (MD5(CONCAT(STR(?colony), STR(NOW())))) LIMIT 3
                            } as %threeOtherColonies

                            WHERE {
                                {INCLUDE %selectedColony.}
                                UNION
                                {INCLUDE %threeOtherColonies.}
                            } ORDER BY DESC(?empire)");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "SparqlQuery",
                value: @"
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
                keyValue: new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"),
                column: "SparqlQuery",
                value: @"#sort chemical elements by number in period system
                            SELECT ?question ?answer WHERE {
                              BIND ('number in period system' as ?question).
                              {
                                SELECT ?item ?element ?number ?symbol WHERE {
                                  ?item wdt:P31 wd:Q11344;
                                        wdt:P1086 ?number;
                                        wdt:P246 ?symbol.
                                  FILTER(1 <= ?number &&
                                         ?number <= 118)
                                  SERVICE wikibase:label {
                                    bd:serviceParam wikibase:language 'en'.
                                    ?item  rdfs:label ?element.
                                  }
                                }
                                ORDER BY MD5(CONCAT(STR(?element), STR(NOW()))) # order by random
                                LIMIT 4
                              }
                              BIND (?element as ?answer).
                            } ORDER BY ASC(?number)");

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

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("f64d3784-3584-4f19-be07-aa44fd9a4086"),
                column: "SparqlQuery",
                value: @"
                            # Based on the example question 'Former capitals''

                            SELECT (SAMPLE(?image) AS ?question) ?answer
                            WHERE
                            {
                              # Fetches capitals from current or former members of the U.N.
                              # Capital is always the most recent capital, states are always only existing states
                              ?country wdt:P463 wd:Q1065.
                              ?country p:P36 ?stat.
                              ?stat ps:P36 ?capital.


                              ?country rdfs:label ?countryLabel.
                              ?capital rdfs:label ?capitalLabel.


                              ?capital wdt:P18 ?image.

                              OPTIONAL {
                                ?country wdt:P582|wdt:P576 ?ended.
                              }
                              OPTIONAL {
                                ?capital wdt:P582|wdt:P576 ?ended.
                              }
                              OPTIONAL {
                                ?stat pq:P582 ?ended.
                              }


                              FILTER(!BOUND(?ended)).
                              FILTER(LANG(?countryLabel) = 'en').
                              FILTER(LANG(?capitalLabel) = 'en').

                              BIND(CONCAT(?capitalLabel, ', ', ?countryLabel) as ?answer).
                              BIND('Where are we?' AS ?question).
                            }
                            GROUP BY ?question ?answer
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) #order by random
                            LIMIT 4
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fa1e5667-cc80-4526-99bd-fd4fc539d4fd"),
                column: "SparqlQuery",
                value: @"
                            SELECT DISTINCT ?question ?answer
                            WITH {
                                 SELECT ?sport ?sportLabel ?image
                                 WHERE {
	                                   ?sport wdt:P31 wd:Q31629.
                                       ?sport wdt:P18 ?image
                                       SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                                ?sport rdfs:label ?sL
                                                              }
                                 filter(lang(?sportLabel) = 'en').
                                 BIND(CONCAT(UCASE(SUBSTR(?sL, 1, 1)), SUBSTR(?sL, 2)) as ?sportLabel)
                                 }
                                 ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                            } as %allSports
                        
                            WITH{
                            SELECT DISTINCT ?sportLabel ?image
                                    WHERE {
                                      INCLUDE %allSports.
                                    }
                                    ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                                    LIMIT 1
                            } AS %selectedSport

                            WITH{
                              SELECT DISTINCT ?sportLabel
                              WHERE{
                                INCLUDE %allSports.
                                FILTER NOT EXISTS {INCLUDE %selectedSport}
    
                              }
                              ORDER BY MD5(CONCAT(STR(?sport), STR(NOW())))
                              LIMIT 3
                            } as %decoySport

                            WHERE {
                                   {INCLUDE %selectedSport}
                                    union
                                   {INCLUDE %decoySport}
                                   BIND(?image as ?question)
                                   BIND(?sportLabel as ?answer)
                            }
                            ORDER BY DESC(?question)
                            ");

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
    }
}
