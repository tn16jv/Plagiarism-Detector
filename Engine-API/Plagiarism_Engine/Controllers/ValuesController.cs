using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Plagiarism_Engine.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        //GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            System.Threading.Thread.Sleep(10000);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //System.Threading.Thread.Sleep(10000);
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
