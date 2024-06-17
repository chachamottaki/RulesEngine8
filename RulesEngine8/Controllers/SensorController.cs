using Microsoft.AspNetCore.Mvc;
using RulesEngine8.Models;
using RulesEngine8.Processors;
using RulesEngine8.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using STJ = System.Text.Json;
using NewtonsoftJson = Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class SensorController : ControllerBase
{
    private readonly RulesEngineDBContext _context;
    private readonly IRuleEngine _ruleEngine;

    public SensorController(RulesEngineDBContext context, IRuleEngine ruleEngine)
    {
        _context = context;
        _ruleEngine = ruleEngine;
    }

    [HttpPost("districts/{district}/installations/{assetType}/{assetKey}/sensors/{sensorKey}:{sensorType}")]
    public async Task<IActionResult> Post(
    string district,
    string assetType,
    string assetKey,
    string sensorKey,
    string sensorType,
    [FromQuery, Required] string hostname,
    [FromBody] SensorModel sensor)
    {
        var inputData = new JsonObject
        {
            ["hostname"] = hostname,
            ["district"] = district,
            ["assetType"] = assetType,
            ["assetKey"] = assetKey,
            ["sensorKey"] = sensorKey,
            ["sensorType"] = sensorType,
            ["alarmId"] = sensor.alarmId
        };

        var endpoint = "/api/Sensor/districts/{district}/installations/{assetType}/{assetKey}/sensors/{sensorKey}:{sensorType}";

        var activeRuleChains = await _context.RuleChains
            .Include(rc => rc.Nodes)
            .Where(rc => rc.IsActive)
            .ToListAsync();

        bool ruleExecuted = false; // Flag to check if at least one rule was executed

        foreach (var ruleChain in activeRuleChains)
        {
            var listeningNode = ruleChain.Nodes
                .FirstOrDefault(n => n.NodeType == "Listening" &&
                                     STJ.JsonSerializer.Deserialize<ListeningNodeConfig>(n.ConfigurationJson).apiEndpoint == endpoint);

            if (listeningNode != null)
            {
                System.Diagnostics.Debug.WriteLine("This is a listening node");
                var configItem = _context.ConfigItems.FirstOrDefault(x => x.AssetID == assetKey);
                var context = new RuleExecutionContext { InputData = inputData, Result = new JsonObject() };

                if (configItem != null)
                {
                    context.State["ConfigItem"] = configItem;
                    context.State["EmailRecipient"] = configItem.Config.email;
                    var digitalInputs = NewtonsoftJson.JsonConvert.DeserializeObject<List<DI>>(configItem.DigitalInputsJson);
                    var alarm = digitalInputs.FirstOrDefault(di => di.alarmId == sensor.alarmId);
                    context.State["Alarm"] = alarm;
                    context.State["AlarmId"] = sensor.alarmId;

                    if (alarm != null)
                    {
                        context.State["sendEmail"] = alarm.sendEmail;
                        context.State["invertSendEmail"] = alarm.Invert;
                    }
                }

                await _ruleEngine.ExecuteRuleChain(ruleChain.RuleChainId, context);
                ruleExecuted = true;
            }
        }

        if (ruleExecuted)
        {
            return Ok(new { message = "Rule chain(s) executed successfully." });
        }
        else
        {
            return BadRequest(new { error = "No active rule chain listening on this endpoint." });
        }
    }
}
