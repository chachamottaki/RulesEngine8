using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RulesEngine8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly RulesEngineDBContext _context;
        private readonly IEmailService _emailService;
        public SensorController(RulesEngineDBContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;

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
        public async Task<IActionResult> Post(
            [FromQuery, Required] string hostname,
            string district,
            string assetType,
            string assetKey,
            string sensorKey,
            string sensorType,
            [FromBody] SensorModel sensor)
        {
            // Retrieve the row from the database where assetKey is the one sent in thru API
            var configItem = _context.ConfigItems.FirstOrDefault(x => x.AssetID == assetKey);
            if (configItem != null)
            {
                var configJson = configItem.Config;
                string UUID = configItem.UUID;
                string deviceID = configItem.DeviceID;
                string shortDescription = configItem.Config.shortDescription;
                string longDescription = configItem.Config.longDescription;
                bool sendEmailValue = (bool)configJson.sendEmail;
                string recipient = configJson.email;
                string email = String.Format("Hi! Alarm Triggered for device {0}; asset {1}! Short description {2} Long Description: {3}", deviceID,assetKey,shortDescription, longDescription);

                if (sendEmailValue)
                {
                    var historyRecord = new HistoryTable
                    {
                        assetUUID = UUID,
                        emailSent = sendEmailValue,
                        emailRecipient = recipient,
                        emailContent = email,
                        timestamp = DateTime.Now
                    };
                    _context.HistoryTables.Add(historyRecord);
                    await _context.SaveChangesAsync();

                    return Ok(new { email_body = "Alarm Triggered!", Emailaddress = recipient });
                }
                else
                {
                    var historyRecord = new HistoryTable
                    {
                        assetUUID = UUID,
                        emailSent = sendEmailValue,
                        emailRecipient = null,
                        timestamp = DateTime.Now
                    };
                    _context.HistoryTables.Add(historyRecord);
                    await _context.SaveChangesAsync();
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
