using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AddGroupID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Questions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"),
                column: "GroupId",
                value: new Guid("d375ff0f-cb79-4eac-84e6-c4bf65c2382a"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("14d93797-c61c-4415-b1ed-359d180237ff"),
                column: "GroupId",
                value: new Guid("98a751f6-eee0-4d79-9401-992417283aa9"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15679f96-27bd-4e88-b367-4eb05e5f6d95"),
                column: "GroupId",
                value: new Guid("fee91818-2fb5-4845-affa-2504d4191ee1"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1687d325-eda8-43ab-9821-711be8d1fea6"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "GroupId",
                value: new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"),
                column: "GroupId",
                value: new Guid("f88bb7ba-a1dc-45c1-8c6f-1c918bf87217"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"),
                column: "GroupId",
                value: new Guid("3de1256d-f8d2-4418-a932-d459d5ee44d6"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("30556891-ae34-4151-b55f-cd5a8b814235"),
                column: "GroupId",
                value: new Guid("70a291e1-4513-4e41-87c5-2746f40a4e0c"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3609a5f7-c90a-4ecf-a713-0224fa8a4215"),
                column: "GroupId",
                value: new Guid("0ef8c0e0-640b-49b4-8aee-b5ab8f1a6773"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("374a02cb-037d-4720-9952-1e3cb96f22ae"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "GroupId",
                value: new Guid("f8717bdd-75df-4064-9394-af163034a1c0"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3a244446-94f0-4d1f-825e-8bd40e6a5d06"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"),
                column: "GroupId",
                value: new Guid("26333055-8b72-4b65-b622-1bfac80e0adc"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3fb180e6-99ae-466b-89e9-16ac0101daed"),
                column: "GroupId",
                value: new Guid("9417595a-641b-4ce9-9219-b9c14e65621e"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"),
                column: "GroupId",
                value: new Guid("289b9977-07e8-4540-9f06-bfcd147b5063"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "GroupId",
                value: new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "GroupId",
                value: new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f42f192-1dc7-44b2-b648-1fee97766b98"),
                column: "GroupId",
                value: new Guid("984112bc-d178-4ac5-8940-3e4fd2fc3105"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                column: "GroupId",
                value: new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f701ada-11d3-45a7-8251-6745d39ffc9a"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("50120520-4441-48c1-b387-1c923a038194"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                              # This query includes: carnivore, artiodactyla, primates
                              SELECT DISTINCT ?question (?name as ?answer) ?image
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
                                   BIND('Which animal is in the image?' as ?question)
                                 } ORDER BY DESC(?image)
                            ", "Which animal is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5272473e-6ef3-4d32-8d64-bb18fa977b29"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            # This query includes: artiodactyla, primates, marsupials
                            SELECT DISTINCT ?question (?name as ?answer) ?image
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
                               BIND('Which animal is in the image?' as ?question)
                             } ORDER BY DESC(?image)
                            ", "Which animal is is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                                # This query includes: primates + artiodactyla + rodentia
                                SELECT DISTINCT ?question (?name as ?answer) ?image

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
                                   BIND('Which animal is in the image?' as ?question)
                                 } ORDER BY DESC(?image)
                            ", "Which animal is this" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"),
                column: "GroupId",
                value: new Guid("72b3fa13-3526-4bd5-964c-442a3f3a5d31"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"),
                columns: new[] { "GroupId", "SparqlQuery" },
                values: new object[] { new Guid("848e9590-10f6-4d16-b2cd-ca282adaee99"), @"
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
                            " });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6ba60225-d7be-4c97-ad19-eaf895a14734"),
                column: "GroupId",
                value: new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("72cd4102-718e-4d5b-adcc-f1e86e8d7cd6"),
                column: "GroupId",
                value: new Guid("bce51234-32b7-4629-b65a-d23beb8b43c3"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"),
                column: "GroupId",
                value: new Guid("ba587fa0-9601-4d99-a56b-7e92a5ccbe13"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"),
                column: "GroupId",
                value: new Guid("976e1e61-3b95-43bf-8e4c-1963b6795113"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8f47586c-5a63-401b-88fb-b63f628a3fe4"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"),
                column: "GroupId",
                value: new Guid("2af47804-eeaa-4bcd-98e3-f515aeaf30b5"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"),
                column: "GroupId",
                value: new Guid("e1a1fbf7-850f-4fdb-878c-9e8190b54d6b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9d630c39-6606-49ff-8cef-b34695d8ed91"),
                column: "GroupId",
                value: new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9ea7b09b-7991-4eb4-b2a1-571e926c5790"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9fd3f504-eb96-42f3-91bc-606a17759e45"),
                column: "GroupId",
                value: new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"),
                column: "GroupId",
                value: new Guid("6cfc621c-7a35-464a-80cd-3937a6d2af3d"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"),
                column: "GroupId",
                value: new Guid("e6ec8ea0-39ee-476c-81f5-b17bd99e715f"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                column: "GroupId",
                value: new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a79cb648-dbfd-4e03-a5a4-315fd4146120"),
                column: "GroupId",
                value: new Guid("039acc70-30d3-40fe-a28a-0b44964d49e7"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ac86282f-1fa1-48ba-a088-f4c202ea0177"),
                column: "GroupId",
                value: new Guid("f20a404e-4d02-4d45-a2bf-cd152b2cbf43"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"),
                column: "GroupId",
                value: new Guid("ffd0f0da-b31d-4c01-b946-8b81fa30b00e"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b32fe12c-4016-4eed-a6d6-0bbb505553a0"),
                column: "GroupId",
                value: new Guid("d834932d-1203-4039-9baf-68322b176bae"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "GroupId",
                value: new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"),
                columns: new[] { "GroupId", "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), 1, @"
                            # This query includes: rodentia, carnivora, marsupial
                            SELECT DISTINCT ?question (?name as ?answer) ?image

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
                               BIND('Which animal is in the image?' as ?question)
                             } ORDER BY DESC(?image)
                            ", "Which animal is this?" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b95607f9-8cd6-48e8-bc99-c9c305e812be"),
                column: "GroupId",
                value: new Guid("acc3d752-2880-4882-ba16-e3deb3ee9cee"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bae2897b-61c9-448e-bedf-8fc069dd62b0"),
                columns: new[] { "GroupId", "SparqlQuery" },
                values: new object[] { new Guid("a2f299e0-493c-425e-b338-19a29b723847"), @"
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
                           BIND('Which animal is in the image?' as ?question)
                         }
                        ORDER BY DESC(?image)
                        " });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"),
                column: "GroupId",
                value: new Guid("f5d0100f-a7bf-4d6d-9767-b5a4463daeb5"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"),
                column: "GroupId",
                value: new Guid("15f9b57e-118a-4448-b24f-b66806197ff8"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("beb15e73-d985-4322-a1a5-e3dec8ac1d28"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d0fcf5ac-3215-4355-9090-b6a49cf66cc3"),
                column: "GroupId",
                value: new Guid("ac3e0a15-376e-4dbc-a8f8-6df4c9fe39e7"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"),
                column: "GroupId",
                value: new Guid("fc74c29e-b4a9-428b-96c9-b41127869a31"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "GroupId",
                value: new Guid("7c2995a2-b025-4033-bc60-f938f3c95ac7"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"),
                column: "GroupId",
                value: new Guid("184f3c3b-a831-4a0b-8c01-a846608f139b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"),
                column: "GroupId",
                value: new Guid("62f3426e-6c47-43de-b9fc-db3e8d988986"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e9ab8641-23b7-428f-b8d6-d96ae7d17e6f"),
                column: "GroupId",
                value: new Guid("be6894ce-de74-4f73-9cb9-0a5edd6d9249"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "GroupId",
                value: new Guid("0b1ff760-e02f-4ddc-8f32-5161931ebcbe"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("f64d3784-3584-4f19-be07-aa44fd9a4086"),
                columns: new[] { "GroupId", "SparqlQuery" },
                values: new object[] { new Guid("848e9590-10f6-4d16-b2cd-ca282adaee99"), @"
                            # Based on the example question 'Former capitals''

                            SELECT ?question ?answer(SAMPLE(?image) AS ?image)
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
                            " });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("fd92d683-fa21-4210-93c7-6a99b8968919"),
                column: "GroupId",
                value: new Guid("a2f299e0-493c-425e-b338-19a29b723847"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("50120520-4441-48c1-b387-1c923a038194"),
                columns: new[] { "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { 2, @"
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
                            ", "Which animal is {0}" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5ab7c050-06c1-4307-b100-32237f5c0429"),
                columns: new[] { "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { 2, @"
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
                            ", "Which animal is {0}" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                columns: new[] { "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { 2, @"
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
                            ", "Which animal is {0}" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("6a5c5b99-f1ce-49ee-a220-429c1fb54d7c"),
                column: "SparqlQuery",
                value: @"
                            # Which famous landmark is this: {image}
                            SELECT ?question ?answer ?image
                            WITH{
                            SELECT ?tes (CONCAT( ?ans, '(', ?country, ')' ) as ?answer) WHERE {
                              { SELECT DISTINCT(?answer as ? ans)(MAX(?image) as ? tes) ? country WHERE { 
                                ?landmark wdt:P31 / wdt:P279 * wd:Q2319498;
                                    wikibase: sitelinks? sitelinks;
                                    wdt: P18? image;
                                    wdt: P17? cntr.
                                ?landmark wdt:P1435? type.
                                FILTER(?sitelinks >= 10)

                                    ? landmark rdfs:label? answer

                                    filter(lang(?answer) = 'en').
                                    ? cntr rdfs:label? country filter(lang(?country) = 'en').
                                }
                                    GROUP BY ?answer? country
                                ORDER BY MD5(CONCAT(STR(? name), STR(NOW())))
                              }
                            }
                            } as %allMonuments

                            WITH
                            {
                                SELECT ?tes ?answer
                              WHERE
                                {
                                    Include % allMonuments
                              }
                                Limit 1
                            } as %selectedMonument

                            WITH
                            {
                                SELECT  ?tes ?answer
                            WHERE
                                {
                                    Include % allMonuments
                                 FILTER NOT EXISTS { INCLUDE % selectedMonument}
                                }
                                Limit 3
                            } as %decoyMonuments

                            WHERE
                            {
                              { INCLUDE % selectedMonument}
                                UNION
                                { INCLUDE % decoyMonuments}
                                Bind(?tes as ?image)
                              BIND('Which famous landmark is this' as ?question)
                            }
                            ORDER BY DESC(?question)
                            ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b5f2a986-4f0d-43e2-9f73-9fc22e76c2ab"),
                columns: new[] { "MiniGameType", "SparqlQuery", "TaskDescription" },
                values: new object[] { 2, @"
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
                            ", "Which animal is {0}" });

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
                           BIND('Which animal is in the image?' as ?question)
                         }
                        ORDER BY DESC(?image)
                        ");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("f64d3784-3584-4f19-be07-aa44fd9a4086"),
                column: "SparqlQuery",
                value: @"
                            # Based on the example question 'Former capitals''

                            SELECT ? question ? answer(SAMPLE(?image) AS ? image)
                            WHERE
                            {
                              # Fetches capitals from current or former members of the U.N.
                              # Capital is always the most recent capital, states are always only existing states
                              ?country wdt:P463 wd:Q1065.
                              ? country p:P36 ? stat.
                              ? stat ps : P36 ? capital.


                              ? country rdfs : label ? countryLabel.
                              ? capital rdfs : label ? capitalLabel.


                              ? capital wdt : P18 ? image.

                              OPTIONAL {
                                ?country wdt:P582 | wdt:P576 ? ended.
                              }
                              OPTIONAL {
                                ?capital wdt:P582 | wdt:P576 ? ended.
                              }
                              OPTIONAL {
                                ?stat pq:P582 ? ended.
                              }


                              FILTER(!BOUND(?ended)).
                              FILTER(LANG(?countryLabel) = 'en').
                              FILTER(LANG(?capitalLabel) = 'en').

                              BIND(CONCAT(?capitalLabel, ', ', ? countryLabel) as ? answer).
                              BIND('Where are we?' AS ? question).
                            }
                            GROUP BY ? question ? answer
                            ORDER BY MD5(CONCAT(STR(?answer), STR(NOW()))) # order by random
                            LIMIT 4
                            ");
        }
    }
}
