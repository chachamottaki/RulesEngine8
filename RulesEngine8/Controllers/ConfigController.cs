using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine8.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RuleEngine8.Controllers
{
    [Route("[controller]")]
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

        [HttpPut("upload")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected or empty.");
            }

            List<string> temp = new List<string>();
            List<string> tempRelements = new List<string>();
            List<string> tempSelements = new List<string>();
            Dictionary<string, string> settings = new Dictionary<string, string>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                for (string line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
                {
                    switch (line)
                    {
                        case string s when s.Contains("PROJECTSETTINGS.sLuxHostname"):
                            temp.AddRange(line.Split("'"));
                            settings["DeviceID"] = temp[1];
                            temp.Clear();
                            break;
                        case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sASMTP"):

                            break;
                        case string s when s.Contains("PROJECTSETTINGS.typMailSettings.wSMTPPort"):

                            break;
                        case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sMailFrom"):
                            tempRelements.AddRange(line.Split("'"));
                            settings["recipients"] = tempRelements[1];
                            break;
                        case string s when s.Contains("PROJECTSETTINGS.typMailSettings.asReceipients"):
                            tempSelements.AddRange(line.Split("'"));
                            settings["sender"] = tempSelements[1];
                            break;
                        case string s when s.Contains(""):
                            
                            break;
                        default:
                            //don't match any pattern
                            break;
                    }
                }
            }

            var configItem = _context.ConfigItems.FirstOrDefault(x => x.DeviceID == settings["DeviceID"] );
            if (configItem.Config.email != settings["recipients"])
            {
                configItem.Config.email = settings["recipients"];
                _context.SaveChanges();
            }
            return Ok(new { message = String.Format("File Uploaded! Any alarm mail for this device should be sent TO: {0}, FROM: {1}", settings["recipients"], settings["sender"]) });
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