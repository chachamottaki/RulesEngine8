using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using System.ComponentModel.DataAnnotations;

namespace RuleEngine.Controllers
{
    [Route("api/Config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ConfigDBContext _context;
        public ConfigController(ConfigDBContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConfigItem>>> GetConfigItems()
        {
            if (_context.ConfigItems == null)
            {
                return NotFound();
            }
            return await _context.ConfigItems.ToListAsync();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConfigItem>> GetConfigItem(int id)
        {
            var configItem = await _context.ConfigItems.FindAsync(id);
            if (configItem == null)
            {
                return NotFound();
            }
            return configItem;
        }

        // POST api/<ConfigController>
        [HttpPost("district/{district}/installations/{assetType}/{assetKey}/sensors/{sensorKey}:{sensorType}")]
        public IActionResult Post([FromQuery, Required] string hostname, string district, string assetType,string assetKey,string sensorKey, string sensorType, [FromBody] Sensor sensor)
        {
            // return params in the response
            return Ok(new {Hostname = hostname, District = district, AssetType = assetType, AssetKey = assetKey, SensorKey = sensorKey, SensorType = sensorType, RequestBody = sensor});
        }

        [HttpPost]
        public async Task<ActionResult<ConfigItem>> PostConfigItem(ConfigItem configItem)
        {
            _context.ConfigItems.Add(configItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConfigItem), new { id = configItem.Id }, configItem);
        }

        // PUT api/<ConfigController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ConfigItem>> PutConfigItem(int id, ConfigItem configItem)
        {
            if (id != configItem.Id)
            {
                return BadRequest();
            }
            _context.Entry(configItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfigItemAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool ConfigItemAvailable(int id)
        {
            return (_context.ConfigItems?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ConfigItem>> DeleteConfigItem(int id)
        {
            if (_context.ConfigItems == null)
            {
                return NotFound();
            }

            var configitem = await _context.ConfigItems.FindAsync(id);
            if (configitem == null)
            {
                return NotFound();
            }
            _context.ConfigItems.Remove(configitem);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}