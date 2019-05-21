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


            /*Console.WriteLine("Test1");
            //--------------- 1st try -------------/
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
            }*/

            /*Console.WriteLine();
            //------------ 2nd try ------------/
            Console.WriteLine("Test2");

            //String querytext = queryString.CommandText;
            String querytext = "SELECT ?president ?president_name ?signature WHERE { ?president wdt:P39 wd:Q11696. ";
            querytext += "?president wdt:P109 ?signature. ";
            querytext += "OPTIONAL {?president rdfs:label ?president_name ";
            querytext += "filter(lang(?president_name) = 'en').}}";
            querytext += " ORDER BY RAND() LIMIT 4";

            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri(("https://query.wikidata.org/bigdata/namespace/wdq/sparql")));
            SparqlResultSet results2 = endpoint.QueryWithResultSet(querytext);

            foreach (SparqlResult result in results2)
            {
                //Console.WriteLine(result.ToString());
                // get dedicated values
                INode value1 = result.Value("president");
                // with indexing
                INode value2 = result[1];
                //With Named Indexing
                INode value3 = result["signature"];
                Console.WriteLine(value1 + " " + value2 + " " + value3);

            }*/


            /*Console.WriteLine();
            //---------- 4th try -------/
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
            SparqlQuery query4 = parser4.ParseFromString(queryString4);*/

            Console.WriteLine();
            //---------- 5th try -------/
            Console.WriteLine("Test5");

            String querytext5 = "SELECT ?capital ?country WHERE {  ?item wdt:P31 wd:Q5119. ?item wdt:P1376 ?land. ?land wdt:P31 wd:Q6256. OPTIONAL { ?item rdfs:label ?capital; filter(lang(?capital) = 'en') ?land rdfs:label ?country; filter(lang(?country) = 'en').} }  ORDER BY RAND() LIMIT 4";
            SparqlRemoteEndpoint endpoint5 = new SparqlRemoteEndpoint(new Uri(("https://query.wikidata.org/bigdata/namespace/wdq/sparql")));
            SparqlResultSet results5 = endpoint5.QueryWithResultSet(querytext5);

            String question = "Was ist die Hauptstadt von " + results5[0]["country"] + "?";
            String possAns = "";
            foreach (SparqlResult result in results5)
            {
                possAns += result["capital"] + ", ";
            }

                Console.WriteLine("Question: " + question);
            Console.WriteLine("Possible answers: " + possAns);

            Console.WriteLine("Correct Answer: " + results5[0]["capital"]);

            Console.WriteLine();
            //---------- 6th try -------/
            Console.WriteLine("Test6 (more generic than 5)");

            String querytext6 = "SELECT ?answer ?question WHERE {  ?item wdt:P31 wd:Q5119. ?item wdt:P1376 ?land. ?land wdt:P31 wd:Q6256. OPTIONAL { ?item rdfs:label ?answer; filter(lang(?answer) = 'en') ?land rdfs:label ?question; filter(lang(?question) = 'en').} }  ORDER BY RAND() LIMIT 4";
            SparqlRemoteEndpoint endpoint6 = new SparqlRemoteEndpoint(new Uri(("https://query.wikidata.org/bigdata/namespace/wdq/sparql")));
            SparqlResultSet results6 = endpoint6.QueryWithResultSet(querytext6);

            String quest = "Was ist die Hauptstadt von " + results6[0]["question"] + "?";
            String possAns6 = "";
            foreach (SparqlResult result in results6)
            {
                possAns6 += result["answer"] + ", ";
            }

            Console.WriteLine("Question: " + quest);
            Console.WriteLine("Possible answers: " + possAns6);

            Console.WriteLine("Correct Answer: " + results6[0]["answer"]);

        }

    }
}
