using System;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using System.Linq;
using VDS.RDF;

namespace SPARQLtest
{
    class Program
    {
        static void Main(string[] args)
        {
            // guide: https://github.com/dotnetrdf/dotnetrdf/wiki/UserGuide-Querying-With-SPARQL


            Console.WriteLine("Test1");
            //****** 1st try ****************/
            //Create a Parameterized String
            SparqlParameterizedString queryString = new SparqlParameterizedString();

            //First we need an instance of the SparqlQueryParser
            SparqlQueryParser parser = new SparqlQueryParser();

            //Add a namespace declaration
            // https://www.mediawiki.org/wiki/Wikidata_Query_Service/User_Manual
            //queryString.Namespaces.AddNamespace("wdt", new Uri("https://query.wikidata.org"));
            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("wds", new Uri("http://www.wikidata.org/entity/statement/"));
            queryString.Namespaces.AddNamespace("wdv", new Uri("http://www.wikidata.org/value/"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryString.Namespaces.AddNamespace("wikibase", new Uri("http://wikiba.se/ontology#"));
            queryString.Namespaces.AddNamespace("p", new Uri("http://www.wikidata.org/prop/"));
            queryString.Namespaces.AddNamespace("ps", new Uri("http://www.wikidata.org/prop/statement/"));
            queryString.Namespaces.AddNamespace("pq", new Uri("http://www.wikidata.org/prop/qualifier/"));
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("bd", new Uri("http://www.bigdata.com/rdf#"));

            //Set the SPARQL command
            //For more complex queries we can do this in multiple lines by using += on the CommandText property
            //Note we can use @name style parameters here
            queryString.CommandText = "SELECT ?president ?president_name ?signature WHERE { ?president wdt:P39 wd:Q11696. ";
            queryString.CommandText += "?president wdt:P109 ?signature. ";
            queryString.CommandText += "OPTIONAL {?president rdfs:label ?president_name ";
            queryString.CommandText += "filter(lang(?president_name) = 'en').}}";
            queryString.CommandText += " ORDER BY RAND() LIMIT 4";

            //When we call ToString() we get the full command text with namespaces appended as PREFIX
            //declarations and any parameters replaced with their declared values
            Console.WriteLine(queryString.ToString());

            //We can turn this into a query by parsing it as in our previous example
            SparqlQuery query = parser.ParseFromString(queryString);


            //Get the Query processor
            ISparqlQueryProcessor processor = new RemoteQueryProcessor(new SparqlRemoteEndpoint(new Uri("https://query.wikidata.org/bigdata/namespace/wdq/sparql")));

            var results = processor.ProcessQuery(query);

            if (results is SparqlResultSet)
            {
                //Print out the Results
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (SparqlResult result in rset)
                {
                    Console.WriteLine(result.ToString());
                }
            }

            Console.WriteLine();
            //********* 2nd try **********/
            Console.WriteLine("Test2");

            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri(("https://query.wikidata.org/bigdata/namespace/wdq/sparql")));
            SparqlResultSet results2 = endpoint.QueryWithResultSet(queryString.CommandText);
            var random = new Random();
            results2.OrderBy(item => random.Next());
            System.Collections.Generic.List<SparqlResult> test = results2.ToList();
            test.OrderBy(item => random.Next());
            foreach (SparqlResult result in results2)
            {
                Console.WriteLine(result.ToString());
                //With method
                INode value = result.Value("?president");
                Console.WriteLine(value);

            }

            foreach (SparqlResult result in test)
            {
                Console.WriteLine(result.ToString());

            }

            Console.WriteLine();
            //********* 4th try **********/
            Console.WriteLine("Test4");
            //Create a Parameterized String
            SparqlParameterizedString queryString4 = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString4.Namespaces.AddNamespace("ex", new Uri("http://example.org/ns#"));

            //Set the SPARQL command
            //For more complex queries we can do this in multiple lines by using += on the
            //CommandText property
            //Note we can use @name style parameters here
            queryString4.CommandText = "SELECT * WHERE { ?s ex:property @value }";

            //Inject a Value for the parameter
            queryString4.SetUri("value", new Uri("http://example.org/value"));

            //When we call ToString() we get the full command text with namespaces appended as PREFIX
            //declarations and any parameters replaced with their declared values
            Console.WriteLine(queryString4.ToString());

            //We can turn this into a query by parsing it as in our previous example
            SparqlQueryParser parser4 = new SparqlQueryParser();
            SparqlQuery query4 = parser4.ParseFromString(queryString4);
        }
    }
}
