using server.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hw1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        // GET: api/<VacationsController>
        [HttpGet]
        public IEnumerable<Vacation> Get()
        {
            Vacation v = new Vacation();
            return v.read();
        }
        
        [HttpGet("GetBySEDate")]
        public IEnumerable<Vacation> GetBySEDate(  DateTime SD, DateTime ED)
        {
            Vacation v = new Vacation();
            return v.readBySEDate(SD, ED);
        }

        // GET api/<VacationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        } 
        
        
        // POST api/<VacationsController>
        [HttpPost]
        public bool Post([FromBody] Vacation v)
        {
            return v.insert();
        }

        // PUT api/<VacationsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VacationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
