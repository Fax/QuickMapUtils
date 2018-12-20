using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebMap.Controllers
{
    [Route("")]
    [ApiController]
    public class MapController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var seed = new Random().Next();
            var gen = new QuickMap.Generator(seed);
            var result = gen.MasterWorldGen();
            return Ok(gen.MapToString(result));
        }

        // GET api/values/5
        [HttpGet("{seed}")]
        public ActionResult<string> Get(int seed)
        {
            var gen = new QuickMap.Generator(seed);
            var result = gen.MasterWorldGen();
            return Ok(gen.MapToString(result));
        }

     
    }
}
