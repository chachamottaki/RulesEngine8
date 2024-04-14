using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RulesEngine8.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RuleEngine8.Controllers
{
    [Route("")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly RulesEngineDBContext _context;
        public ConfigController(RulesEngineDBContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConfigItemModel>>> GetConfigItems()
        {
            if (_context.ConfigItems == null)
            {
                return NotFound();
            }
            return await _context.ConfigItems.ToListAsync();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConfigItemModel>> GetConfigItem(int id)
        {
            var configItem = await _context.ConfigItems.FindAsync(id);
            if (configItem == null)
            {
                return NotFound();
            }
            return configItem;
        }

        [HttpPost]
        public async Task<ActionResult<ConfigItemModel>> PostConfigItem(ConfigItemModel configItem)
        {
            // Add the configItem to the database and save changes
            _context.ConfigItems.Add(configItem);
            await _context.SaveChangesAsync();

            // Retrieve the row from the database where Id = 1
            var rowWithId1 = _context.ConfigItems.FirstOrDefault(x => x.Id == 7);
            if (rowWithId1 != null)
            {
                // Parse the JSON stored in the Config column
                var configJson = rowWithId1.Config;

                // Extract the value of "sendEmail" from the JSON
                bool sendEmailValue = (bool)configJson.sendEmail;

                // testing db read, json data retreival"
                if (sendEmailValue)
                {
                    return Ok(new { hi= "hi", Emailaddress = configJson.email });
                }
            }

            return CreatedAtAction(nameof(GetConfigItem), new { id = configItem.Id }, configItem);
        }

        // PUT api/<ConfigController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ConfigItemModel>> PutConfigItem(int id, ConfigItemModel configItem)
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
        public async Task<ActionResult<ConfigItemModel>> DeleteConfigItem(int id)
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