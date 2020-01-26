using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Helpers
{
    public static class TextLinkHelper
    {
        public static string LinkFromTextAndUrl(string text, string url, bool asHtml)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return text;
            }

            if (asHtml)
            {
                return $"<a href=\"{url}\" target=\"_blank\">{text}</a>";
            }

            return $"<link=\"{url}\">{text}</link>";
        }
    }
}
