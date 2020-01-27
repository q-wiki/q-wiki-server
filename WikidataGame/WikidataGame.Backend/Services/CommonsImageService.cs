using AutoMapper;
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
    public static class CommonsImageService
    {
        public const string CommonsInfoEndpoint = "https://en.wikipedia.org/w/api.php?action=query&prop=imageinfo&iiprop=extmetadata|url&format=json&iiurlwidth=800&titles=File%3a{0}";

        public static async Task<Models.ImageInfo> RetrieveImageInfoByUrlAsync(string url, IMapper mapper)
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
                    var infoJson = await webClient.DownloadStringTaskAsync(string.Format(CommonsInfoEndpoint, filename));
                    var jObject = JObject.Parse(infoJson);
                    var imageInfo = jObject.SelectToken("query.pages..imageinfo[0]").ToObject<WikiImageInfo>();
                    var htmlregex = new Regex("<(\\s*[(/?)\\w+]*)");
                    if (htmlregex.IsMatch(imageInfo.LicenseInfo.ObjectName?.Value))
                    {
                        imageInfo.LicenseInfo.ObjectName = new WikiValueSource { Value = HttpUtility.UrlDecode(filename), Source = "Q-Wiki" };
                    }

                    return mapper.Map<Models.ImageInfo>(imageInfo);
                }
            }
            catch (Exception)
            {
                throw new UnableToRetrieveLicenseException();
            }
        }
    }

    public class WikiImageInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("descriptionurl")]
        public string DescriptionUrl { get; set; }

        [JsonProperty("thumburl")]
        public string ThumbUrl { get; set; }

        [JsonProperty("extmetadata")]
        public WikiLicenseInfo LicenseInfo { get; set; }
    }

    public class WikiLicenseInfo
    {
        public WikiValueSource ObjectName { get; set; }
        public WikiValueSource Artist { get; set; }
        public WikiValueSource LicenseUrl { get; set; }
        public WikiValueSource LicenseShortName { get; set; }
    }

    public class WikiValueSource
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
