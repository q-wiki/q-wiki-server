using System;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using System.Linq;
using VDS.RDF;
using System.Collections.Generic;
using VDS.RDF.Nodes;

namespace SPARQLtest
{
    class Program
    {
        static void Main(string[] args)
        {

            var timestamp = DateTime.UtcNow.ToString();
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri(($"https://query.wikidata.org/bigdata/namespace/wdq/sparql?foo={timestamp}")));

            // Dictionary with questions and queries
            // The queries should always yield exactly 4 result rows, where the first is the one with the correct answer
            // Each row consists of 2 columns: 
            // answer contains the correct answer and 
            // question contains the variable part of the question (e.g. country of which the capital is asked for) -> this goes into surrogate part of question later
            Dictionary<string, string> quests = new Dictionary<string, string>();
            quests.Add("What is the capital of {0}?", "SELECT ?answer ?question WHERE { ?item wdt:P31 wd:Q5119. ?item wdt:P1376 ?land. ?land wdt:P31 wd:Q6256. OPTIONAL { ?item rdfs:label ?answer; filter(lang(?answer) = 'en') ?land rdfs:label ?question; filter(lang(?question) = 'en').} }  ORDER BY RAND() LIMIT 4");
            quests.Add("Which continent has {0} countries?", "SELECT ?answer (COUNT(?item) AS ?question) WHERE { ?item wdt:P31 wd:Q6256. ?item wdt:P30 ?continent. ?continent wdt:P31 wd:Q5107. OPTIONAL { ?continent rdfs:label ?answer; filter(lang(?answer) = 'en') }} GROUP BY ?continent ?answer ORDER BY RAND() LIMIT 4");
            quests.Add("Which U.S. president's signature is this: {0}?", "SELECT ?answer ?question WHERE { ?president wdt:P39 wd:Q11696. ?president wdt:P109 ?question. OPTIONAL { ?president rdfs:label ?answer; filter(lang(?answer) = 'en'). }} ORDER BY RAND() LIMIT 4");


            foreach (var q in quests)
            {
                // query results...
                SparqlResultSet results = endpoint.QueryWithResultSet(q.Value);

                // get possible answers   ->TODO: needs to be shuffled!
                String possAns = "";
                foreach (SparqlResult result in results)
                {
                    possAns += result["answer"].AsValuedNode().AsString() + ", ";
                }

                // as described above: Platzhalter of question and answer
                string qpart = results[0]["question"].AsValuedNode().AsString();
                string ans = results[0]["answer"].AsValuedNode().AsString();

                // Output
                Console.WriteLine("Question: " + q.Key, qpart);
                Console.WriteLine("Possible answers: " + possAns);
                Console.WriteLine();
                Console.WriteLine("Correct Answer: " + ans);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

    }
}
