﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Helpers
{
    public static class DatabaseSeeds
    {
        public static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                //new Category
                //{
                //    Id = "e9019ee1-0eed-492d-8aa7-feb1974fb265",
                //    Title = "Nature"
                //},
                //new Category
                //{
                //    Id = "ddd333f7-ef45-4e13-a2ca-fb4494dce324",
                //    Title = "Culture"
                //},
                new Category
                {
                    Id = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    Title = "Geography"
                }
                , new Category
                {
                    Id = "1b9185c0-c46b-4abf-bf82-e464f5116c7d",
                    Title = "Space"
                },
                //new Category
                //{
                //    Id = "6c22af9b-2f45-413b-995d-7ee6c61674e5",
                //    Title = "Chemistry"
                //},
                //new Category
                //{
                //    Id = "f9c52d1a-9315-423d-a818-94c1769fffe5",
                //    Title = "History"
                //},
                //new Category
                //{
                //    Id = "4941c348-b4c4-43b5-b3d4-85794c68eec4",
                //    Title = "Celebrities"
                //},
                //new Category
                //{
                //    Id = "2a388146-e32c-4a08-a246-472eff12849a",
                //    Title = "Entertainment"
                //},
                //new Category
                //{
                //    Id = "7f2baca7-cdf4-4e24-855b-c868d9030ba4",
                //    Title = "Politics"
                //},
                //new Category
                //{
                //    Id = "3d6c54d3-0fda-4923-a00e-e930640430b3",
                //    Title = "Sports"
                //}
                );
        }

        public static void SeedQuestions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = "a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "What is the name of the capital of {0}?",
                    SparqlQuery = @"SELECT ?answer ?question WHERE {  
                          ?item wdt:P31 wd:Q5119.
                          ?item wdt:P1376 ?land.
                          ?land wdt:P31 wd:Q6256.
                          OPTIONAL { 
                            ?item rdfs:label ?answer;
                                    filter(lang(?answer) = 'en')
                              ?land rdfs:label ?question;
                                    filter(lang(?question) = 'en').
                          }
                            }
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4"
                },
                new Question
                {
                    Id = "aca0f5f7-b000-42fb-b713-f5fe43748761",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which continent has {0} countries?",
                    SparqlQuery = @"SELECT ?answer (COUNT(?item) AS ?question)
                        WHERE 
                        {
                          ?item wdt:P31 wd:Q6256.
                          ?item wdt:P30 ?continent.
                          ?continent wdt:P31 wd:Q5107.
                          OPTIONAL {?continent rdfs:label ?answer ;
                                    filter(lang(?answer) = 'en')
                                          }
                        }
                        GROUP BY ?continent ?answer
                        ORDER BY MD5(CONCAT(STR(?answer), STR(NOW())))
                        LIMIT 4"
                },
                new Question
                {
                    Id = "9a70639b-3447-475a-905a-e866a0c98a1c",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is a part of continent {0}?",
                    SparqlQuery = @"SELECT ?answer ?question
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
                            MINUS { ?continentStatement pq:P582 ?endTime. }
                          } ORDER BY MD5(CONCAT(STR(?state), STR(NOW())))
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
                          }
                          LIMIT 1
                        } AS %threeStates
                        WITH {
                          # dump continent for false answers (needed for sorting)
                          SELECT ?state ?empty WHERE {
                            INCLUDE %states.
                            FILTER NOT EXISTS { INCLUDE %selectedContinent. }
                          }
                          LIMIT 3
                        } AS %oneState
                        WHERE {
                            { INCLUDE %oneState. } UNION
                            { INCLUDE %threeStates. }

                          SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'.
                            ?state  rdfs:label ?answer.
                            ?continent rdfs:label ?question.
                          }
                        }
                        ORDER BY DESC(?question)"
                },
                new Question
                {
                    Id = "46679c4f-ef97-445d-9a70-d95a5337720f",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is no basin country of the Baltic Sea?",
                    SparqlQuery = @"SELECT DISTINCT ?question ?answer
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
                        order by DESC(?question)"
                },
                new Question
                {
                    Id = "4f6c477e-7025-44b4-a3b0-f3ebd8902902",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is no basin country of the {0}?",
                    SparqlQuery = @"SELECT DISTINCT ?question ?answer
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
                        order by DESC(?noSea)"
                },
                new Question
                {
                    Id = "a6a470de-9efb-4fde-9388-6eb20f2ff1f4",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is no basin country of the {0}?",
                    SparqlQuery = @"SELECT DISTINCT ?question ?answer
                        WITH {
                          SELECT DISTINCT (?state as ?country) WHERE {
                            ?state wdt:P31/wdt:P279* wd:Q3624078;
                                   p:P463 ?memberOfStatement.
                            ?memberOfStatement a wikibase:BestRank;
                                                 ps:P463 wd:Q1065.
                            MINUS { ?memberOfStatement pq:P582 ?endTime. }
                            MINUS { ?state wdt:P576|wdt:P582 ?end. }
                          }
                          ORDER BY MD5(CONCAT(STR(?state), STR(NOW())))
                        } AS %states
                        WITH { 
                              SELECT DISTINCT ?country WHERE {
                                  BIND(wd:Q4918 AS ?sea).
                                  ?sea wdt:P205 ?country.
                                } LIMIT 3
                            } as %threeBasins
                        WITH {
                          SELECT DISTINCT ?country ?noSea
                            WHERE {
                              BIND(wd:Q4918 AS ?noSea).
                              INCLUDE %states.
                              ?country wdt:P361 ?region.
                              VALUES ?region { wd:Q7204 wd:Q984212 wd:Q27449 wd:Q263686 wd:Q50807777 wd:Q27468 wd:Q27381 }.
                              FILTER NOT EXISTS {?country wdt:P31 wd:Q51576574.}
                          } LIMIT 1
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
                        order by DESC(?noSea)"
                },
                new Question
                {
                    Id = "29fed1d0-d306-4946-8109-63b8aaf0262e",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "What is the longest river in {0}?",
                    SparqlQuery = @"SELECT DISTINCT ?answer ?question WHERE {
                        { SELECT DISTINCT ?river ?continent (avg(?length2) as ?length)
                            WHERE
                            {
                              ?river wdt:P31/wdt:P279* wd:Q355304;
                                 wdt:P2043 ?length2;
                                 wdt:P30 ?continent.
                              {
                                SELECT DISTINCT ?continent WHERE {
                                  VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15  } # ohne Ozeanien
                                } ORDER BY MD5(CONCAT(STR(?continent), STR(NOW()))) LIMIT 1
                               } 
                            }
                            group by ?river ?continent
                        }
                        OPTIONAL {?continent rdfs:label ?question;
                            filter(lang(?question) = 'en')
                            ?river rdfs:label ?answer ;
                            filter(lang(?answer) = 'en')
                        }
                    }
                    order by desc(?length)
                    limit 4"
                },
                new Question
                {
                    Id = "f88a4dc0-8187-43c4-8775-593822bf4af1",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.BlurryImage,
                    TaskDescription = "Which famous monument is this: {0}?",
                    SparqlQuery = @"SELECT ?question (CONCAT( ?ans, ' (', ?country, ')' ) as ?answer) WHERE {
                      { SELECT DISTINCT (?answer as ?ans) (MAX(?image) as ?question) ?country WHERE { 
                        ?landmark wdt:P31/wdt:P279* wd:Q2319498;
                                 wikibase:sitelinks ?sitelinks;
                                 wdt:P18 ?image;
                                 wdt:P17 ?cntr.
                        ?landmark wdt:P1435 ?type.
                        FILTER(?sitelinks >= 10)

                        SERVICE wikibase:label { 
                            bd:serviceParam wikibase:language 'en'.
                            ?cntr rdfs:label ?country.
                            ?landmark rdfs:label ?answer.}
                        }
                        GROUP BY ?answer ?country
                        ORDER BY MD5(CONCAT(STR(?question), STR(NOW())))
                        LIMIT 4 
                      }
                    }"
                },
                new Question
                {
                    Id = "bc7a22ee-4985-44c3-9388-5c7dd6b8762e",
                    CategoryId = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort countries by {0} (ascending)",
                    SparqlQuery = @"#sort countries by number of inhabitants (ascending)
                                    SELECT (?stateLabel as ?answer) ?question 
                                    WITH {
                                      # subquery: get 4 random countries with their average number of inhabitants
                                      SELECT DISTINCT ?state ?stateLabel (ROUND(AVG(?population) / 1000) * 1000 as ?population) {

                                        {
                                          # subquery: list of all countries in the world
                                          SELECT DISTINCT ?state ?stateLabel ?population ?dateOfCensus
                                                                 WHERE {
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
                                      }
                                      GROUP BY ?state ?stateLabel
                                      ORDER BY MD5(CONCAT(STR(?item), STR(NOW()))) LIMIT 4
                                    } as %states

                                    WHERE {
                                      # fill the question (hard-coded) and sort by population (= correct sort order needed for sorting game)
                                      INCLUDE %states.
                                      BIND('number of inhabitants' as ?question).
                                    } ORDER BY ?population"
                },
                // Space
                new Question
                {
                    Id = "a4a7289a-3053-4ad7-9c60-c75a18305243",
                    CategoryId = "1b9185c0-c46b-4abf-bf82-e464f5116c7d",
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort planets by {0} (ascending)",
                    SparqlQuery = @"# sort planets by average distance to sun
                        SELECT ?answer ?question WHERE {
                          {SELECT DISTINCT ?answer ?avgDistanceToSun
                                                   WHERE 
                                                   {
                                                     # fetch planets in our solar system
                                                     ?planet wdt:P31/wdt:P279+ wd:Q17362350.
                                                     ?planet p:P2243/psv:P2243 [wikibase:quantityAmount ?apoapsis; wikibase:quantityUnit ?apoapsisUnit].
                                                     ?planet p:P2244/psv:P2244 [wikibase:quantityAmount ?periapsis; wikibase:quantityUnit ?periapsisUnit].
                                                     # NOTE: there are only three planets with apoapsis and periapsis in AU; 4 planets in total
                                                     # FILTER (?apoapsisUnit = wd:Q1811 && ?periapsisUnit = wd:Q1811)
                                                     BIND ((?apoapsis + ?periapsis) / 2 as ?avgDistanceToSun)
                                                     FILTER (?apoapsisUnit = wd:Q828224 && ?periapsisUnit = wd:Q828224)
                                                     SERVICE wikibase:label { 
                                                       bd:serviceParam wikibase:language 'en'.
                                                       ?planet  rdfs:label ?answer.} 
                                                   } ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) LIMIT 4}
                          BIND('average distance to sun' as ?question)
                        } ORDER BY ?avgDistanceToSun"
                },
                new Question
                {
                    Id = "2ed01768-9ab6-4895-8cbf-09dbc6f957e0",
                    CategoryId = "1b9185c0-c46b-4abf-bf82-e464f5116c7d", // Space
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort planets by {0} (ascending)",
                    SparqlQuery = @"# sort planets by radius
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
                        ORDER BY ?radius"
                },
                new Question
                {
                    Id = "14d93797-c61c-4415-b1ed-359d180237ff",
                    CategoryId = "1b9185c0-c46b-4abf-bf82-e464f5116c7d", // Space
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which of these moons belongs to planet {0}?",
                    SparqlQuery = @"#Which of these moons belongs to the planet {0}?
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
                        } ORDER BY MD5(CONCAT(STR(?moon), STR(NOW()))) # order by random
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
                                       ORDER BY MD5(CONCAT(STR(?parentLabel), STR(NOW()))) # order by random
                                       LIMIT 1
                            }
                          }
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
                          }
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
                            ?parent  rdfs:label ?answer.
                            ?moon rdfs:label ?question.
                          }
                        } ORDER BY DESC(?answer)"
                }
                );
        }
    }
}
