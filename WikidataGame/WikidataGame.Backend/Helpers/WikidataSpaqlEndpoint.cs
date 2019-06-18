using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace WikidataGame.Backend.Helpers
{
    public class WikidataSparqlEndpoint : SparqlRemoteEndpoint
    {
        public const string WikidataSparqlEndpointUrl = "https://query.wikidata.org/bigdata/namespace/wdq/sparql";
        public WikidataSparqlEndpoint() : base(new Uri(WikidataSparqlEndpointUrl)) { }

        protected override void ApplyCustomRequestOptions(HttpWebRequest httpRequest)
        {
            base.ApplyCustomRequestOptions(httpRequest);
            httpRequest.UserAgent = "QWiki/1.0 Wikidata Game Bot (HTW Berlin & Wikimedia Germany)";
        }
    }
}
