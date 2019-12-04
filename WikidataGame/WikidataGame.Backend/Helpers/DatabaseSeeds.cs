using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Helpers
{
    public static class DatabaseSeeds
    {
        public static Category CategoryGeography = new Category
        {
            Id = new Guid("cf3111af-8b18-4c6f-8ee6-115157d54b79"),
            Title = "Geography"
        };
        public static Category CategorySpace = new Category
        {
            Id = new Guid("1b9185c0-c46b-4abf-bf82-e464f5116c7d"),
            Title = "Space"
        };
        public static Category CategoryChemistry = new Category
        {
            Id = new Guid("6c22af9b-2f45-413b-995d-7ee6c61674e5"),
            Title = "Chemistry"
        };
        public static Category CategoryHistory = new Category
        {
            Id = new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"),
            Title = "History"
        };
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
        public static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                CategoryChemistry,
                CategoryGeography,
                CategoryHistory,
                CategorySpace);
        }

        public static void SeedQuestions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "What is the name of the capital of {0}?",
                    SparqlQuery = @"SELECT DISTINCT ?answer ?question WHERE {
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
                    LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which continent has {0} countries?",
                    SparqlQuery = @"SELECT ?continent ?answer ?question WHERE {
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
                        LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is a part of {0}?",
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
                        ORDER BY DESC(?question)"
                },
                new Question
                {
                    Id = new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is not a basin country of the Baltic Sea?",
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
                    Id = new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is not a basin country of the Caribbean Sea?",
                    SparqlQuery = @"# Which country is no basin country of the Caribbean Sea?
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
                        order by DESC(?noSea)"
                },
                new Question
                {
                    Id = new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which country is not a basin country of the Mediterranean Sea?",
                    SparqlQuery = @"# Which country is no basin country of the Mediterranean Sea?
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
                        order by DESC(?noSea)"
                },
                new Question
                {
                    Id = new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "What is the longest river in {0}?",
                    SparqlQuery = @"# What is the longest river in {continent}?
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
                              ?river wdt:P31/wdt:P279* wd:Q355304;
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
                        limit 4"
                },
                /*new Question
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
                },*/
                new Question
                {
                    Id = new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"),
                    CategoryId = CategoryGeography.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort countries by {0} (ascending).",
                    SparqlQuery = @"# sort countries by number of inhabitants (ascending)
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
                        } ORDER BY ?population"
                },
                // Space
                new Question
                {
                    Id = new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"),
                    CategoryId = CategorySpace.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort planets by {0} (ascending).",
                    SparqlQuery = @"# sort planets by average distance to sun
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
                        } ORDER BY ?avgDistanceToSun"
                },
                new Question
                {
                    Id = new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"),
                    CategoryId = CategorySpace.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort planets by {0} (ascending).",
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
                    Id = new Guid("14d93797-c61c-4415-b1ed-359d180237ff"),
                    CategoryId = CategorySpace.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which of these moons belongs to {0}?",
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
                        } ORDER BY DESC(?question)"
                },
                // Chemistry
                new Question
                {
                    Id = new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"),
                    CategoryId = CategoryChemistry.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "What's the chemical symbol for {0}?",
                    SparqlQuery = @"# What's the chemical symbol for {element}?
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
                        LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"),
                    CategoryId = CategoryChemistry.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which element has the chemical symbol {0}?",
                    SparqlQuery = @"SELECT ?question ?answer WHERE {
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
                    LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"),
                    CategoryId = CategoryChemistry.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort chemical elements by {0} (ascending).",
                    SparqlQuery = @"#sort chemical elements by number in period system
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
                        } ORDER BY ASC(?number)"
                },
                // History
                new Question
                {
                    Id = new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"),
                    CategoryId = CategoryHistory.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort these US presidents by {0} (ascending).",
                    SparqlQuery = @"
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
                       " 
                },
                new Question
                {
                    Id = new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"),
                    CategoryId = CategoryHistory.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort the countries by {0} (ascending).",
                    SparqlQuery = @"# sort EU countries by the date they joined
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
                        ORDER BY ?date"
                },
                new Question
                {
                    Id = new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"),
                    CategoryId = CategoryHistory.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Who was Federal Chancellor of Germany from {0}?",
                    SparqlQuery = @"# German Chancellors
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
                        LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"),
                    CategoryId = CategoryHistory.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which colony belonged to the {0}?",
                    SparqlQuery = @"# empires and colonies
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
                        } ORDER BY DESC(?empire)"
                },
                new Question
                {
                    Id = new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"),
                    CategoryId = CategoryHistory.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which of these wars started in {0}?",
                    SparqlQuery = @"# wars of the 20th century
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
                        LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"),
                    CategoryId = CategoryChemistry.Id,
                    MiniGameType = MiniGameType.MultipleChoice,
                    TaskDescription = "Which chemical compound has the formula {0}?",
                    SparqlQuery = @"# Which chemical compound has the formula {0}?
                        SELECT DISTINCT ?chemicalCompound ?answer (?chemical_formula AS ?question) ?sitelinks WHERE {
                          ?chemicalCompound wdt:P31 wd:Q11173;
                            wdt:P274 ?chemical_formula;
                            wikibase:sitelinks ?sitelinks.
                          FILTER(?sitelinks >= 50 )
                          ?chemicalCompound rdfs:label ?answer.
                          FILTER((LANG(?answer)) = 'en')
                        }
                        ORDER BY (MD5(CONCAT(STR(?answer), STR(NOW()))))
                        LIMIT 4"
                },
                new Question
                {
                    Id = new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"),
                    CategoryId = CategoryChemistry.Id,
                    MiniGameType = MiniGameType.Sort,
                    TaskDescription = "Sort these chemical elements by {0} (ascending)!",
                    SparqlQuery = @"
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
                        ORDER BY ?value"
                }
                );
        }
    }
}
