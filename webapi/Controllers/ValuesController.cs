using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IClusterClient _client;
        
        public ValuesController(IClusterClient client)
        {
            _client = client;
        }
        
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var userGrain = _client.GetGrain<IUserGrain>(id, "");
            return new {Info=await userGrain.GetInfo(),Key= userGrain.GetGrainIdentity()};
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}