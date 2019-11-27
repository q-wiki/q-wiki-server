using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class InitFoodCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { "3e888b3e-f04f-40fc-a4e0-78768a49506b", "Food" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "d647f166-4ac0-455b-b9db-e50787d961b1", "3e888b3e-f04f-40fc-a4e0-78768a49506b", 0, @"
                       SELECT DISTINCT (Sample(GROUP_CONCAT(DISTINCT ?question; SEPARATOR=', ')) AS ?question) (SAMPLE(GROUP_CONCAT(DISTINCT SAMPLE(?answer); SEPARATOR=', ')) AS ?answer) 
                        ?year
                        WHERE{
                        {SELECT DISTINCT ?softDrink ?softDrinkLabel ?year ?inception ?question
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
                           ORDER BY (MD5(CONCAT(STR(?question), STR(NOW()))))
                           LIMIT 5
                          }
                           SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'.
                                                     ?softDrinkLabel rdfs:label ?answer.
                                                   }
                           BIND('Order Softdrinks by inception' as ?question)
                        }
                        group by ?year 
                        order by ?year", "Sort these drinks by inception (ascending)" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { "a38e6677 -092e-4191-a46e-b8e73134da60", "3e888b3e-f04f-40fc-a4e0-78768a49506b", 2, @"
                        SELECT ?question ?answer 
                        WITH{
                        SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                        WHERE {
                        ?item wdt:P31 wd:Q134768. 
                        ?item wdt:P495 ?origin.
                        ?origin wdt:P30 ?originContinent.
                        }
                        } AS %cocktails
                        WITH{
                        SELECT ?item ?itemLabel ?origin ?originLabel ?originContinent
                        WHERE {
                        ?item wdt:P31 wd:Q2536409. 
                        ?item wdt:P495 ?origin.
                        ?origin wdt:P30 ?originContinent.
                        }
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
                          SELECT DISTINCT ?origin ?originLabel ?answer
                          WHERE{
                             ?origin wdt:P31 wd:Q3624078.
                             FILTER NOT EXISTS { INCLUDE %selectedCountry. }
                             {
                               ?originContinent wdt:P31 wd:Q5107.
                               INCLUDE %selectedContinent
                             }
                             ?origin wdt:P30 ?originContinent.
                           }
                          ORDER BY (MD5(CONCAT(STR(?item), STR(NOW())))) 
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
                    ", "Where is the cocktail {0} from" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "a38e6677 -092e-4191-a46e-b8e73134da60");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d647f166-4ac0-455b-b9db-e50787d961b1");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "3e888b3e-f04f-40fc-a4e0-78768a49506b");
        }
    }
}
