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

            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("https://query.wikidata.org/bigdata/namespace/wdq/sparql?"));

            // Dictionary with questions and queries
            // The queries should always yield exactly 4 result rows, where the first is the one with the correct answer
            // Each row consists of 2 columns: 
            // answer contains the correct answer and 
            // question contains the variable part of the question (e.g. country of which the capital is asked for) -> this goes into surrogate part of question later
            Dictionary<string, string> quests = new Dictionary<string, string>();
            quests.Add("What is the capital of {0}?", "SELECT ?answer ?question WHERE { ?item wdt:P31 wd:Q5119. ?item wdt:P1376 ?land. ?land wdt:P31 wd:Q6256. OPTIONAL { ?item rdfs:label ?answer; filter(lang(?answer) = 'en') ?land rdfs:label ?question; filter(lang(?question) = 'en').} }  ORDER BY RAND() LIMIT 4");
            quests.Add("Which continent has {0} countries?", "SELECT ?answer (COUNT(?item) AS ?question) WHERE { ?item wdt:P31 wd:Q6256. ?item wdt:P30 ?continent. ?continent wdt:P31 wd:Q5107. OPTIONAL { ?continent rdfs:label ?answer; filter(lang(?answer) = 'en') }} GROUP BY ?continent ?answer ORDER BY RAND() LIMIT 4");
            quests.Add("Which U.S. president's signature is this: {0}?", "SELECT ?answer ?question WHERE { ?president wdt:P39 wd:Q11696. ?president wdt:P109 ?question. OPTIONAL { ?president rdfs:label ?answer; filter(lang(?answer) = 'en'). }} ORDER BY RAND() LIMIT 4");
            quests.Add("Which country is a part of continent {0}?", @"SELECT ?answer ?question WITH {SELECT DISTINCT ?state ?continent ?stateLabel ?continentLabel WHERE {?state wdt:P31/wdt:P279* wd:Q3624078;p:P463 ?memberOfStatement.?memberOfStatement a wikibase:BestRank;ps:P463 wd:Q1065. MINUS { ?memberOfStatement pq:P582 ?endTime.}.MINUS {?state wdt:P576|wdt:P582 ?end.}.?state p:P30 ?continentStatement.?continentStatement a wikibase:BestRank;ps:P30 ?continent.VALUES ?continent {wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15}MINUS {?continentStatement pq:P582 ?endTime.}} ORDER BY RAND()} AS %states WITH {SELECT ?state ?continent WHERE {INCLUDE %states. {SELECT DISTINCT ?continent WHERE {VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 }} order by rand() LIMIT 1}}} AS %selectedContinent WITH {SELECT DISTINCT ?state ?continent WHERE {INCLUDE %selectedContinent.} LIMIT 1} AS %threeStates WITH {SELECT ?state ?empty WHERE {INCLUDE %states.FILTER NOT EXISTS { INCLUDE %selectedContinent. }} LIMIT 3 } AS %oneState WHERE {{ INCLUDE %oneState. } UNION { INCLUDE %threeStates. } SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. ?state  rdfs:label ?answer. ?continent rdfs:label ?question. }} ORDER BY DESC(?question)");
            quests.Add("Which country is no basin country of the Baltic Sea?", @"SELECT DISTINCT ?question ?answer WITH { SELECT DISTINCT (?state as ?country) WHERE { ?state wdt:P31/wdt:P279* wd:Q3624078; p:P463 ?memberOfStatement. ?memberOfStatement a wikibase:BestRank; ps:P463 wd:Q1065. MINUS { ?memberOfStatement pq:P582 ?endTime.} MINUS { ?state wdt:P576|wdt:P582 ?end.}}} AS %states WITH { SELECT DISTINCT ?country ?sea WHERE { BIND(wd:Q545 AS ?sea). ?sea wdt:P205 ?country. }} as %basins WITH { SELECT DISTINCT ?country WHERE { INCLUDE %basins. } ORDER BY RAND() LIMIT 3 } as %threeBasins WITH { SELECT DISTINCT ?country ?noSea WHERE { INCLUDE %states. ?country wdt:P30 wd:Q46. BIND(wd:Q545 as ?noSea). FILTER NOT EXISTS { INCLUDE %basins.}} ORDER BY RAND() LIMIT 1} AS %oneOther WHERE {{ INCLUDE %oneOther. } UNION { INCLUDE %threeBasins. } SERVICE wikibase:label {bd:serviceParam wikibase:language 'en'. ?country rdfs:label ?answer. ?noSea rdfs:label ?question. }} order by DESC(?question)");
            quests.Add("Which country is no basin country of the {0}?", @"SELECT DISTINCT ?question ?answer WITH { SELECT DISTINCT (?state as ?country) WHERE {?state wdt:P31/wdt:P279* wd:Q3624078; p:P463 ?memberOfStatement. ?memberOfStatement a wikibase:BestRank; ps:P463 wd:Q1065. MINUS { ?memberOfStatement pq:P582 ?endTime. } MINUS { ?state wdt:P576|wdt:P582 ?end. }}} AS %states WITH { SELECT DISTINCT ?country ?sea WHERE { BIND(wd:Q1247 AS ?sea). { ?sea wdt:P205 ?country.} UNION { INCLUDE %states. ?country wdt:P361 ?region. VALUES ?region {wd:Q664609 wd:Q166131 wd:Q778 wd:Q93259 wd:Q19386 wd:Q5317255}.}} order by rand() } as %basins WITH { SELECT DISTINCT ?country ?sea WHERE { INCLUDE %basins. } order by rand() LIMIT 3 } as %threeBasins WITH { SELECT DISTINCT ?country ?noSea WHERE { INCLUDE %states. ?country wdt:P361 ?region. BIND(wd:Q1247 as ?noSea). VALUES ?region {wd:Q12585 wd:Q653884}. FILTER NOT EXISTS {?country wdt:P31 wd:Q112099.} FILTER NOT EXISTS {?country wdt:P31 wd:Q13107770.} FILTER NOT EXISTS {?country wdt:P361 wd:Q27611.} FILTER NOT EXISTS {INCLUDE %basins.}} order by rand() LIMIT 1 } AS %oneOther WHERE { { INCLUDE %oneOther. } UNION { INCLUDE %threeBasins. } SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. ?country rdfs:label ?answer. ?noSea rdfs:label ?question. }} order by DESC(?noSea)");
            quests.Add("Which country is no basin country of the {0}?", @"SELECT DISTINCT ?question ?answer WITH { SELECT DISTINCT (?state as ?country) WHERE { ?state wdt:P31/wdt:P279* wd:Q3624078; p:P463 ?memberOfStatement. ?memberOfStatement a wikibase:BestRank; ps:P463 wd:Q1065. MINUS { ?memberOfStatement pq:P582 ?endTime. } MINUS { ?state wdt:P576|wdt:P582 ?end. }}} AS %states WITH { SELECT DISTINCT ?country ?sea WHERE { BIND(wd:Q4918 AS ?sea). ?sea wdt:P205 ?country. } order by rand() LIMIT 3 } as %threeBasins WITH { SELECT DISTINCT ?country ?noSea WHERE { BIND(wd:Q4918 AS ?noSea). INCLUDE %states. ?country wdt:P361 ?region. VALUES ?region { wd:Q7204 wd:Q984212 wd:Q27449 wd:Q263686 wd:Q50807777 wd:Q27468 wd:Q27381 }. FILTER NOT EXISTS {?country wdt:P31 wd:Q51576574.}} order by rand() LIMIT 1 } AS %oneOther WHERE {{ INCLUDE %oneOther. } UNION { INCLUDE %threeBasins. } SERVICE wikibase:label { bd:serviceParam wikibase:language 'en'. ?country rdfs:label ?answer. ?noSea rdfs:label ?question.}} order by DESC(?noSea)");
            quests.Add("What is the longest river in {0}?", @"SELECT DISTINCT ?answer ?question WHERE {{ SELECT DISTINCT ?river ?continent (avg(?length2) as ?length) WHERE { ?river wdt:P31/wdt:P279* wd:Q355304; wdt:P2043 ?length2; wdt:P30 ?continent. { SELECT DISTINCT ?continent WHERE { VALUES ?continent { wd:Q49 wd:Q48 wd:Q46 wd:Q18 wd:Q15 }} order by rand() LIMIT 1 }} group by ?river ?continent } OPTIONAL {?continent rdfs:label ?question; filter(lang(?question) = 'en') ?river rdfs:label ?answer; filter(lang(?answer) = 'en') }} order by desc(?length) limit 4");


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
                string qpart = (results[0]["question"] != null) ? results[0]["question"].AsValuedNode().AsString() : "";
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
