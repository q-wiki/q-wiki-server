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
                    return imageInfo;
                }
            }
            catch (Exception)
            {
                throw new UnableToRetrieveLicenseException();
            }
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
