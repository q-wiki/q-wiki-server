using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using VDS.RDF.Query;
using VDS.RDF.Nodes;

namespace WikidataGame.Backend.Services
{
    public abstract class MinigameServiceBase
    {
        protected readonly IMinigameRepository _minigameRepo;
        protected readonly IQuestionRepository _questionRepo;
        protected readonly DataContext _dataContext;
        protected static SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri(("https://query.wikidata.org/bigdata/namespace/wdq/sparql")));


        public MinigameServiceBase(
            IMinigameRepository minigameRepo,
            IQuestionRepository questionRepo,
            DataContext dataContext)
        {
            _minigameRepo = minigameRepo;
            _questionRepo = questionRepo;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Query Wikidata for results
        /// </summary>
        /// <param name="question">the question, containing a placeholder</param>
        /// <param name="sparql">the sparql query as a String</param>
        /// <returns>List of 4 Tuples(question part (placeholder),answer) -> the first tuple contains the correct answer! </returns>
        protected List<Tuple<string, string>> QueryWikidata(String question, String sparql)
        {
            // TODO: use something, that is more safe (e.g. an Object with member for correct answer and stuff) -> must work for all Minigame types

            //question = "What is the capital of {0}?";
            //sparql = "SELECT ?answer ?question WHERE { ?item wdt:P31 wd:Q5119. ?item wdt:P1376 ?land. ?land wdt:P31 wd:Q6256. OPTIONAL { ?item rdfs:label ?answer; filter(lang(?answer) = 'en') ?land rdfs:label ?question; filter(lang(?question) = 'en').} }  ORDER BY RAND() LIMIT 4";
            var results = new List<Tuple<string, string>>();

            // query results...
            SparqlResultSet res = endpoint.QueryWithResultSet(sparql);

            // get possible answers
            foreach (SparqlResult result in res)
            {
                results.Add(new Tuple<string, string>(result["question"].AsValuedNode().AsString(), result["answer"].AsValuedNode().AsString()));
            }

            return results;

        }


        public bool IsMiniGameAnswerCorrect(Models.MiniGame miniGame, IEnumerable<string> answers)
        {
            return answers.SequenceEqual(miniGame.CorrectAnswer);
        }
    }
}
