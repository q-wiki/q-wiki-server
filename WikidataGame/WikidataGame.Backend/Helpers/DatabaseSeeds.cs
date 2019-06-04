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
        public static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = "e9019ee1-0eed-492d-8aa7-feb1974fb265",
                    Title = "Nature"
                },
                new Category
                {
                    Id = "ddd333f7-ef45-4e13-a2ca-fb4494dce324",
                    Title = "Culture"
                },
                new Category
                {
                    Id = "cf3111af-8b18-4c6f-8ee6-115157d54b79",
                    Title = "Geography"
                },
                new Category
                {
                    Id = "1b9185c0-c46b-4abf-bf82-e464f5116c7d",
                    Title = "Space"
                },
                new Category
                {
                    Id = "6c22af9b-2f45-413b-995d-7ee6c61674e5",
                    Title = "Natural Sciences"
                },
                new Category
                {
                    Id = "f9c52d1a-9315-423d-a818-94c1769fffe5",
                    Title = "History"
                },
                new Category
                {
                    Id = "4941c348-b4c4-43b5-b3d4-85794c68eec4",
                    Title = "Celebrities"
                },
                new Category
                {
                    Id = "2a388146-e32c-4a08-a246-472eff12849a",
                    Title = "Entertainment"
                },
                new Category
                {
                    Id = "7f2baca7-cdf4-4e24-855b-c868d9030ba4",
                    Title = "Politics"
                },
                new Category
                {
                    Id = "3d6c54d3-0fda-4923-a00e-e930640430b3",
                    Title = "Sports"
                });
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
                        ORDER BY RAND() LIMIT 4"
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
                        ORDER BY RAND()
                        LIMIT 4"
                });
        }
    }
}
