using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

            return CreatedAtAction(nameof(GetConfigItem), new { id = configItem.Id }, configItem);
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected or empty.");
            }

            //string firstLine;
            //string fileContent;
            List<string> sender = new List<string>();
            List<string> recipients = new List<string>();
            List<string> elementsR = new List<string>();
            List<string> elementsS = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {

                //firstLine = await reader.ReadLineAsync();
                //fileContent = await reader.ReadToEndAsync();
                for (string line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
                {
                    // Check each line against patterns using switch case
                    switch (line)
                    {
                        case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sMailFrom"):
                            
                            elementsR.AddRange(line.Split("'"));
                            recipients.Add(elementsR[1]);
                            break;
                        case string s when s.Contains("PROJECTSETTINGS.typMailSettings.asReceipients"):
                            elementsS.AddRange(line.Split("'"));
                            sender.Add(elementsS[1]);
                            break;
                        // more cases
                        default:
                            //don't match any pattern
                            break;
                    }
                }
            }
         
            
            return Ok(new { message = String.Format("File Uploaded! Any alarm mail for this device will be sent TO: {0}, FROM: {1}", recipients[0], sender[0]) });
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