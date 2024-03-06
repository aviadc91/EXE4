using server.BL;
using Microsoft.AspNetCore.Mvc;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hw1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        // GET: api/<FlatsController>
        [HttpGet]
        public IEnumerable<Flat> Get()
        {
            Flat f = new Flat();
            return f.read();
        } 
        
        
        // GET: api/<FlatsController>
        [HttpGet("GetByMaxPrice")]
        public IEnumerable<Flat> FlatsByMaxPrice(double MaxPrice)
        {
            Flat f = new Flat();
            return f.FlatsByMaxPrice(MaxPrice);
        }

        // GET api/<FlatsController>/5
        [HttpGet("{id}")]
        public List<Flat> Get(int id)
        {
            Flat f = new Flat();
            return f.read();
        }

        // POST api/<FlatsController>
        [HttpPost]
        public bool Post([FromBody] Flat f)
        {
            return f.insert();
        }

        // PUT api/<FlatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<FlatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
