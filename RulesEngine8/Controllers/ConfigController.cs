using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine8.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;

namespace RuleEngine8.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly RulesEngineDBContext _context;
        private readonly ConfigFileParsingService _fileParsingService;

        public ConfigController(RulesEngineDBContext context, ConfigFileParsingService fileParsingService)
        {
            _context = context;
            _fileParsingService = fileParsingService;
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
            //var file = Request.Form.Files[0]; //access first file uploaded
            foreach (var file in Request.Form.Files)
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File not selected or empty.");
                }

                //string configType = "deviceSettings"; // needs to be changed depending on what's coming from react select thingy  //
                string configType = "DI";

                if (configType == "deviceSettings")
                {
                    Dictionary<string, string> settings = await _fileParsingService.ParseSettingsFromFileAsync(file);
                    var configItem = _context.ConfigItems.FirstOrDefault(x => x.DeviceID == settings["DeviceID"]);
                    if (configItem != null)
                    {
                        if (configItem.Config.email != settings["recipients"])
                        {
                            configItem.Config.email = settings["recipients"];
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        /// Create a new ConfigItem since it doesn't exist
                        var newconfigItem = new ConfigItemModel
                        {
                            DeviceID = settings["DeviceID"],
                            Config = new ConfigJson
                            {
                                email = settings["recipients"]
                            }
                        };

                        // Add the new configItem to the database and save changes
                        _context.ConfigItems.Add(newconfigItem);
                        await _context.SaveChangesAsync();
                    }
                    return Ok(settings);
                }

                if (configType == "DI")
                {
                    (List<Dictionary<string, string>> DI, List<Dictionary<string, string>> subDI) = await _fileParsingService.ParseDIFromFileAsync(file);
                    //var configItem = _context.ConfigItems.FirstOrDefault(x => x.DeviceID == );
                    //DI

                    //subDI

                    return Ok(subDI);

                }

            }

            

            

            
            
            
            //List<Dictionary<string, string>> DIs = await ParseDIFromFileAsync(file);


          
            return Ok("done");
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
