using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/games/{id}/minigames")]
    [ApiController]
    public class MinigamesController : ControllerBase
    {
        [HttpGet]
        public string GetString()
        {
            return "Hello World";
        }
    }
}