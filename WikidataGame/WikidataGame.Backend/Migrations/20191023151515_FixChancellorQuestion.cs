using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class FixChancellorQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "86b64102-8074-4c4e-8f3e-71a0e52bb261",
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"# German Chancellors
                        SELECT ?answer (CONCAT(STR(?startYear), ' to ', STR(?endYear)) AS ?question) WHERE {
                          ?person p:P39 ?Bundeskanzler.
                          ?Bundeskanzler ps:P39 wd:Q4970706;
                            pq:P580 ?start.
                          ?Bundeskanzler pq:P582 ?end. # <- make end mandatory
                          BIND(YEAR(?start) AS ?startYear)
                          BIND(IF(!(BOUND(?end)), 'today', YEAR(?end)) AS ?endYear)
                          SERVICE wikibase:label {
                            bd:serviceParam wikibase:language 'en'.
                            ?person rdfs:label ?answer.
                          }
                        }
                        ORDER BY (MD5(CONCAT(STR(?person), STR(NOW()))))
                        LIMIT 4", "Who was Federal Chancellor of Germany from {0}?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "86b64102-8074-4c4e-8f3e-71a0e52bb261",
                columns: new[] { "SparqlQuery", "TaskDescription" },
                values: new object[] { @"# German Chancellors
                        SELECT ?answer (CONCAT(STR(?startYear), ' - ', STR(?endYear)) AS ?question) WHERE {
                          ?person p:P39 ?Bundeskanzler.
                          ?Bundeskanzler ps:P39 wd:Q4970706;
                            pq:P580 ?start.
                          OPTIONAL { ?Bundeskanzler pq:P582 ?end. }
                          BIND(YEAR(?start) AS ?startYear)
                          BIND(IF(!(BOUND(?end)), 'today', YEAR(?end)) AS ?endYear)
                          SERVICE wikibase:label {
                            bd:serviceParam wikibase:language 'en'.
                            ?person rdfs:label ?answer.
                          }
                        }
                        ORDER BY (MD5(CONCAT(STR(?person), STR(NOW()))))
                        LIMIT 4", "Who was Federal Chancellor of Germany in {0}?" });
        }
    }
}
