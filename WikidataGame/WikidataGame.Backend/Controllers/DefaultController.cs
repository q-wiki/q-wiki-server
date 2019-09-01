using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.IO;

namespace WikidataGame.Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/privacy")]
        [HttpGet]
        [HttpPost]
        public async Task<ContentResult> GetPrivacy()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"{assembly.GetName().Name}.Data.privacy.html";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var privacyInfoText = await reader.ReadToEndAsync();
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        StatusCode = StatusCodes.Status200OK,
                        Content = privacyInfoText
                    };
                }
            }
        }
    }
}