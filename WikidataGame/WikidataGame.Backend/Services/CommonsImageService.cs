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

        public static async Task<ImageInfo> RetrieveImageInfoByUrlAsync(string url)
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
                    var imageInfo = jObject.SelectToken("query.pages..imageinfo[0]").ToObject<ImageInfo>();
                    var htmlregex = new Regex("<(\\s*[(/?)\\w+]*)");
                    if (htmlregex.IsMatch(imageInfo.LicenseInfo.ObjectName?.Value))
                    {
                        imageInfo.LicenseInfo.ObjectName = new ValueSource { Value = HttpUtility.UrlDecode(filename), Source = "Q-Wiki" };
                    }

                    return imageInfo;
                }
            }
            catch (Exception)
            {
                throw new UnableToRetrieveLicenseException();
            }
        }

        public static async Task<(string, string)> RetrieveImageInfoStringByUrlAsync(string url, bool asHtml = false)
        {
            string licenseOutput;
            var imageInfo = await RetrieveImageInfoByUrlAsync(url);
            var regex = new Regex("href=\"(?<link>.*?)\".*?>(?<name>.*?)</");
            var match = regex.Match(imageInfo.LicenseInfo.Artist?.Value);
            if (match.Success)
            {
                licenseOutput = $"{LinkFromTextAndUrl(match.Groups["name"].Value, match.Groups["link"].Value, asHtml)}, ";
            }
            else
            {
                licenseOutput = $"{imageInfo.LicenseInfo.Artist} ,";
            }
            licenseOutput += $"{LinkFromTextAndUrl(imageInfo.LicenseInfo.ObjectName?.Value, imageInfo.DescriptionUrl, asHtml)}, ";
            if (imageInfo.LicenseInfo.LicenseUrl != null)
            {
                licenseOutput += $"{LinkFromTextAndUrl(imageInfo.LicenseInfo.LicenseShortName?.Value, imageInfo.LicenseInfo.LicenseUrl?.Value, asHtml)}";
            }
            else
            {
                licenseOutput += imageInfo.LicenseInfo.LicenseShortName?.Value;
            }
            return (imageInfo.ThumbUrl, licenseOutput);
        }
        
        private static string LinkFromTextAndUrl(string text, string url, bool asHtml)
        {
            if (asHtml)
                return $"<a href=\"{url}\" target=\"_blank\">{text}</a>";

            return $"<link=\"{url}\">{text}</link>";
        }
    }

    public class ImageInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("descriptionurl")]
        public string DescriptionUrl { get; set; }

        [JsonProperty("thumburl")]
        public string ThumbUrl { get; set; }

        [JsonProperty("extmetadata")]
        public LicenseInfo LicenseInfo { get; set; }
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
