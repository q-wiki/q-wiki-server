using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using VDS.RDF.Query;
using VDS.RDF.Nodes;
using System.Diagnostics;

namespace WikidataGame.Backend.Services
{
    public abstract class MinigameServiceBase
    {
        protected readonly IMinigameRepository _minigameRepo;
        protected readonly DataContext _dataContext;
        protected SparqlRemoteEndpoint endpoint = new WikidataSparqlEndpoint();


        public MinigameServiceBase(
            IMinigameRepository minigameRepo,
            DataContext dataContext)
        {
            _minigameRepo = minigameRepo;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Query Wikidata for results
        /// </summary>
        /// <param name="sparql">the sparql query as a String</param>
        /// <returns>List of 4 Tuples(question part (placeholder),answer) -> the first tuple contains the correct answer! </returns>
        protected List<Tuple<string, string>> QueryWikidata(String sparql)
        {
            var results = new List<Tuple<string, string>>();

            // query results...
            try
            {
                SparqlResultSet res = endpoint.QueryWithResultSet(sparql);

                // get possible answers
                foreach (SparqlResult result in res)
                {
                    string q = (result["question"] != null) ? result["question"].AsValuedNode().AsString() : "";
                    string a = (result["answer"] != null) ? result["answer"].AsValuedNode().AsString() : "";
                    results.Add(new Tuple<string, string>(q, a));
                }

                return results;
            }catch(RdfQueryException exception)
            {
                Debug.WriteLine(exception.ToString());
            }
            return null;   
        }


        public static bool IsMiniGameAnswerCorrect(Models.MiniGame miniGame, IEnumerable<string> answers)
        {
            if (answers == null || answers.Count() < 1)
                return false;

            return answers.SequenceEqual(miniGame.CorrectAnswer);
        }
    }
}
