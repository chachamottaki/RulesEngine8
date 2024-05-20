using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RulesEngine8.Controllers
{
    [Route("[controller]")]
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

        // POST api/<ConfigController>
        [HttpPost("districts/{district}/installations/{assetType}/{assetKey}/sensors/{sensorKey}:{sensorType}")]
        public async Task<IActionResult> Post(
            [FromQuery, Required] 
            string hostname,
            string district,
            string assetType,
            string assetKey,
            string sensorKey,
            string sensorType,
            [FromBody] SensorModel sensor)
        {
            
            var configItem = _context.ConfigItems.FirstOrDefault(x => x.AssetID == assetKey);
            
            if (configItem != null)
            {
                var digitalInputs = JsonConvert.DeserializeObject<List<DI>>(configItem.DigitalInputsJson);
                var alarm = digitalInputs.FirstOrDefault(di => di.alarmId == sensor.alarmId);
                
                if (alarm != null)
                {
                    var configJson = configItem.Config;
                    string deviceID = configItem.DeviceID;
                    string shortDescription = alarm.shortDescription;
                    string longDescription = alarm.longDescription;
                    bool sendEmailValue = alarm.sendEmail;
                    bool invertSendEMail = alarm.Invert;
                    bool isAlarm = alarm.IsAlarm;
                    string recipient = configJson.email;
                    string email = $"Hi! Alarm Triggered for device {deviceID}; asset {assetKey}! Short description: {shortDescription}, Long Description: {longDescription}";

                    if (sendEmailValue && !invertSendEMail)
                    {
                        var historyRecord = new HistoryTable
                        {
                            isAlarm = isAlarm,
                            alarmId = sensor.alarmId,
                            assetId = assetKey,
                            DeviceId = hostname,
                            emailSent = sendEmailValue,
                            emailRecipient = recipient,
                            emailContent = email,
                            timestamp = DateTime.Now
                        };
                        _context.HistoryTables.Add(historyRecord);
                        await _context.SaveChangesAsync();

                        // Send email
                        //await _emailService.SendEmailAsync(recipient, "Alarm Triggered", email);
                    }
                    else
                    {
                        var historyRecord = new HistoryTable
                        {
                            emailSent = sendEmailValue,
                            emailRecipient = null,
                            alarmId = sensor.alarmId,
                            assetId = assetKey,
                            DeviceId = hostname,
                            timestamp = DateTime.Now
                        };
                        _context.HistoryTables.Add(historyRecord);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return Ok(new { Hostname = hostname, District = district, AssetType = assetType, AssetKey = assetKey, SensorKey = sensorKey, SensorType = sensorType, RequestBody = sensor });
        }
    }
}
