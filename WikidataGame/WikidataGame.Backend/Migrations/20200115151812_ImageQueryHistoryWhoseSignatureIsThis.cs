using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class ImageQueryHistoryWhoseSignatureIsThis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CategoryId", "MiniGameType", "SparqlQuery", "Status", "TaskDescription" },
                values: new object[] { new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"), new Guid("f9c52d1a-9315-423d-a818-94c1769fffe5"), 1, @"
                        # Which U.S. president's signature is this: {question}
                        SELECT  ?question ?answer ?image
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
                          ORDER BY MD5(CONCAT(STR(?name), STR(NOW())))
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
                          BIND('Whose pressidents signature is this?' as ?question)
                        }
                        ORDER BY DESC(?image)
                        ", 2, "Which of presidents signature is this?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3ad7e147-5862-48f1-aa7a-62d5df7f1222"));
        }
    }
}
