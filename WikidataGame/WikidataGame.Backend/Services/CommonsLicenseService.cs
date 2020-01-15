using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace WikidataGame.Backend.Services
{
    public static class CommonsLicenseService
    {
        public static async Task<string> RetrieveLicenseInfoByFilenameAsync(string filename)
        {
            try
            {
                //using (var webClient = new WebClient())
                //{
                //    var info = await webClient.DownloadStringTaskAsync($"https://commons-api.arnes.space/license?url={filename}");
                //    var
                //}
                return "license-info (WIP)";
            }
            catch (Exception)
            {
                throw new UnableToRetrieveLicenseException();
            }
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
