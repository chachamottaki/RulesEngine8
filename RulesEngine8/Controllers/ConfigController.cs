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

        [HttpPost]
        public async Task<ActionResult<ConfigItem>> PostConfigItem(ConfigItem configItem)
        {
            // Add the configItem to the database and save changes
            _context.ConfigItems.Add(configItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConfigItem), new { id = configItem.Id }, configItem);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files, [FromForm] List<string> configTypes, [FromForm] string device)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("File not selected or empty.");
            }
            if (configTypes == null || configTypes.Count != files.Count)
            {
                return BadRequest("Number of values does not match the number of files.");
            }
            if (string.IsNullOrEmpty(device))
            {
                return BadRequest("Device is not specified.");
            }
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                string configType = configTypes[i];

                if (file == null || file.Length == 0)
                {
                    return BadRequest($"File {i + 1} is empty.");
                }
                if (configType == "DI")
                {
                    (List<Dictionary<string, string>> DI, List<Dictionary<string, string>> subDI) = await _fileParsingService.ParseDIFromFileAsync(file);
                    foreach (var subDiItem in subDI)
                    {
                        string installationKey = subDiItem["InstallationKey"];

                        var existingAsset = await _context.ConfigItems
                            .Include(e => e.DigitalInputs)
                            .FirstOrDefaultAsync(e => e.AssetID == installationKey);
                        

                        DI newDI = new DI
                        {
                            alarmId = subDiItem["id"],
                            isActive = bool.Parse(subDiItem["isActive"]),
                            shortDescription = subDiItem["shortDescription"],
                            longDescription = subDiItem["longDescription"],
                            isTPST = bool.Parse(subDiItem["isTPST"]),
                            sendEmail = bool.Parse(subDiItem["sendEMail"]),
                            Invert = bool.Parse(subDiItem["Invert"]),

                            IsAlarm = bool.Parse(subDiItem["IsAlarm"]),
                            InstallationType = subDiItem["InstallationType"],
                            InstallationKey = subDiItem["InstallationKey"],
                            SensorKey = subDiItem["SensorKey"],
                            SensorType = subDiItem["SensorType"],
                            SendOnChange = bool.Parse(subDiItem["SendOnChange"]),
                            Send = bool.Parse(subDiItem["Send"]),
                            Error = bool.Parse(subDiItem["Error"]),
                            ErrorMsg = subDiItem["ErrorMsg"],
                            LastTime = subDiItem["LastTime"]
                        };

                        if (existingAsset != null)
                        {
                            existingAsset.DeviceID = device;
                            var existingDI = existingAsset.DigitalInputs.FirstOrDefault(di => di.alarmId == subDiItem["id"]);// Check if digital input (ex 1.1) already exists i/t list
                            
                            if (existingDI == null) //if doesn' exist
                            {
                                existingAsset.DigitalInputs.Add(newDI);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                // Update the existing digital input if necessary (properties)
                                existingDI.isActive = bool.Parse(subDiItem["isActive"]);
                                existingDI.shortDescription = subDiItem["shortDescription"];
                                existingDI.longDescription = subDiItem["longDescription"];
                                existingDI.isTPST = bool.Parse(subDiItem["isTPST"]);
                                existingDI.sendEmail = bool.Parse(subDiItem["sendEMail"]);
                                existingDI.Invert = bool.Parse(subDiItem["Invert"]);
                                existingDI.IsAlarm = bool.Parse(subDiItem["IsAlarm"]);
                                existingDI.InstallationType = subDiItem["InstallationType"];
                                existingDI.SensorKey = subDiItem["SensorKey"];
                                existingDI.SensorType = subDiItem["SensorType"];
                                existingDI.SendOnChange = bool.Parse(subDiItem["SendOnChange"]);
                                existingDI.Send = bool.Parse(subDiItem["Send"]);
                                existingDI.Error = bool.Parse(subDiItem["Error"]);
                                existingDI.ErrorMsg = subDiItem["ErrorMsg"];
                                existingDI.LastTime = subDiItem["LastTime"];

                                var DIline = await _context.DigitalInputs
                                .FirstOrDefaultAsync(e => e.alarmId == subDiItem["id"]);

                                DIline.isActive = bool.Parse(subDiItem["isActive"]);
                                DIline.shortDescription = subDiItem["shortDescription"];
                                DIline.longDescription = subDiItem["longDescription"];
                                DIline.isTPST = bool.Parse(subDiItem["isTPST"]);
                                DIline.sendEmail = bool.Parse(subDiItem["sendEMail"]);
                                DIline.Invert = bool.Parse(subDiItem["Invert"]);
                                DIline.IsAlarm = bool.Parse(subDiItem["IsAlarm"]);
                                DIline.InstallationType = subDiItem["InstallationType"];
                                DIline.SensorKey = subDiItem["SensorKey"];
                                DIline.SensorType = subDiItem["SensorType"];
                                DIline.SendOnChange = bool.Parse(subDiItem["SendOnChange"]);
                                DIline.Send = bool.Parse(subDiItem["Send"]);
                                DIline.Error = bool.Parse(subDiItem["Error"]);
                                DIline.ErrorMsg = subDiItem["ErrorMsg"];
                                DIline.LastTime = subDiItem["LastTime"];

                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var deviceConfig = await _context.ConfigItems.FirstOrDefaultAsync(x => x.DeviceID == device && x.Config != null);
                            if (deviceConfig != null)
                            {
                                var newAsset = new ConfigItem
                                {
                                    DeviceID = device,
                                    AssetID = installationKey,
                                    Config = new ConfigJson 
                                    {
                                        email = deviceConfig.Config.email
                                    },
                                    DigitalInputs = new List<DI>() { newDI }
                                };
                                _context.ConfigItems.Add(newAsset);
                            }
                            else
                            {
                                var newAsset = new ConfigItem
                                {
                                    DeviceID = device,
                                    AssetID = installationKey,
                                    DigitalInputs = new List<DI>() { newDI }
                                };
                                _context.ConfigItems.Add(newAsset);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }

                    await _context.SaveChangesAsync();
                    return Ok(_context.ConfigItems);
                }
                if (configType == "deviceSettings")
                {
                    Dictionary<string, string> settings = await _fileParsingService.ParseSettingsFromFileAsync(file);

                    if (settings["recipients"] == null)
                    {
                        return BadRequest($"no email recipients found in file");
                    }

                    var configItems = _context.ConfigItems.Where(x => x.DeviceID == device).ToList();
                    if (configItems.Count > 0)
                    {
                        foreach (var configItem in configItems)
                        {
                            if (configItem.Config != null && settings["recipients"] != null)
                            {
                                if (configItem.Config.email != settings["recipients"])
                                {
                                    configItem.Config.email = settings["recipients"];
                                }
                            }
                            else if (configItem.Config == null && settings["recipients"] != null)
                            {
                                // Create only ConfigJson properties at existing ConfigItem
                                configItem.Config = new ConfigJson
                                {
                                    email = settings["recipients"]
                                };
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var newconfigItem = new ConfigItem
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
                }
            }
            return Ok("done");
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
