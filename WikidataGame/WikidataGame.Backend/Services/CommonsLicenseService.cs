using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace WikidataGame.Backend.Services
{
    public static class CommonsLicenseService
    {
        public const string LicenseInfoEndpoint = "https://en.wikipedia.org/w/api.php?action=query&prop=imageinfo&iiprop=extmetadata&format=json&titles=File%3a{0}";

        public static async Task<LicenseInfo> RetrieveLicenseInfoByUrlAsync(string url)
        {
            var regex = new Regex("FilePath/(?<filename>.*?)$");
            var match = regex.Match(url);
            if (!match.Success)
                throw new UnableToRetrieveLicenseException();

            var filename = match.Groups["filename"].Value;
            try
            {
                using (var webClient = new WebClient())
                {
                    var infoJson = await webClient.DownloadStringTaskAsync(string.Format(LicenseInfoEndpoint, filename));
                    var jObject = JObject.Parse(infoJson);
                    var imageInfo = jObject.SelectToken("query.pages..imageinfo[0].extmetadata").ToObject<LicenseInfo>();
                    var htmlregex = new Regex("<(\\s*[(/?)\\w+]*)");
                    if (htmlregex.IsMatch(imageInfo.ObjectName?.Value))
                    {
                        imageInfo.ObjectName = new ValueSource { Value = HttpUtility.UrlDecode(filename), Source = "Q-Wiki" };
                    }

                    return imageInfo;
                }
            }
            catch (Exception)
            {
                throw new UnableToRetrieveLicenseException();
            }
        }

        public static async Task<string> RetrieveLicenseInfoStringByUrlAsync(string url)
        {
            string licenseOoutput;
            var imageInfo = await RetrieveLicenseInfoByUrlAsync(url);
            var regex = new Regex("href=\"(?<link>.*?)\".*?>(?<name>.*?)</");
            var match = regex.Match(imageInfo.Artist?.Value);
            if (match.Success)
            {
                licenseOoutput = $"{LinkFromTextAndUrl(match.Groups["name"].Value, match.Groups["link"].Value)}, ";
            }
            else
            {
                licenseOoutput = $"{imageInfo.Artist} ,";
            }
            licenseOoutput += $"{LinkFromTextAndUrl(imageInfo.ObjectName?.Value, url)}, ";
            if (imageInfo.LicenseUrl != null)
            {
                licenseOoutput += $"{LinkFromTextAndUrl(imageInfo.LicenseShortName?.Value, imageInfo.LicenseUrl?.Value)}";
            }
            else
            {
                licenseOoutput += imageInfo.LicenseShortName?.Value;
            }
            return licenseOoutput;
        }


        private static string LinkFromTextAndUrl(string text, string url)
        {
            return $"<link=\"{url}\">{text}</link>";
        }
    }

    public class LicenseInfo
    {
        public ValueSource ObjectName { get; set; }
        public ValueSource Artist { get; set; }
        public ValueSource LicenseUrl { get; set; }
        public ValueSource LicenseShortName { get; set; }
    }

    public class ValueSource
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public class UnableToRetrieveLicenseException : Exception
    {
        public UnableToRetrieveLicenseException(string message) : base(message)
        {
        }

        public UnableToRetrieveLicenseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnableToRetrieveLicenseException()
        {
        }
    }
}
