using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RulesEngine8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly RulesEngineDBContext _context;
        public SensorController(RulesEngineDBContext context)
        {
            _context = context;
        }
        // GET: api/<SensorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SensorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SensorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST api/<ConfigController>
        [HttpPost("districts/{district}/installations/{assetType}/{assetKey}/sensors/{sensorKey}:{sensorType}")]
        public IActionResult Post([FromQuery, Required] string hostname, string district, string assetType, string assetKey, string sensorKey, string sensorType, [FromBody] SensorModel sensor)
        {
            // compare with DB values
            // Retrieve the row from the database where Id = 1
            var configItem = _context.ConfigItems.FirstOrDefault(x => x.AssetID == assetKey);
            if (configItem != null)
            {
                // Parse the JSON stored in the Config column
                var configJson = configItem.Config;

                // Extract the value of "sendEmail" from the JSON
                bool sendEmailValue = (bool)configJson.sendEmail;

                // testing db read, json data retreival"
                if (sendEmailValue)
                {
                    return Ok(new { hi = "Email will be sent!", Emailaddress = configJson.email });
                }
            }
            // return params in the response
            return Ok(new { Hostname = hostname, District = district, AssetType = assetType, AssetKey = assetKey, SensorKey = sensorKey, SensorType = sensorType, RequestBody = sensor });
        }

        // PUT api/<SensorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SensorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
