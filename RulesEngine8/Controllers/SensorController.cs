using Microsoft.AspNetCore.Mvc;
using RulesEngine8.Models;
using RulesEngine8.Processors;
using RulesEngine8.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
        [FromBody] JsonElement sensor)
    {
        // Combine route parameters and body into a single JsonObject
        var inputData = new JsonObject
        {
            ["hostname"] = hostname,
            ["district"] = district,
            ["assetType"] = assetType,
            ["assetKey"] = assetKey,
            ["sensorKey"] = sensorKey,
            ["sensorType"] = sensorType,
            ["sensor"] = JsonObject.Parse(sensor.GetRawText())
        };

        var endpoint = "/api/Sensor/districts/{district}/installations/{assetType}/{assetKey}/sensors/{sensorKey}:{sensorType}";
        

        // Find active rule chains with a listening node matching the endpoint
        var activeRuleChains = await _context.RuleChains
            .Include(rc => rc.Nodes)
            .Where(rc => rc.IsActive)
            .ToListAsync();

        foreach (var ruleChain in activeRuleChains)
        {
            //System.Diagnostics.Debug.WriteLine($"rulechainID: {ruleChain.RuleChainId}"); //correct ruleChainId! 
            //System.Diagnostics.Debug.WriteLine(endpoint); // ok
            //System.Diagnostics.Debug.WriteLine(ruleChain.Nodes.FirstOrDefault(n => n.NodeType == "Listening"));
            var listeningNode = ruleChain.Nodes
                .FirstOrDefault(n => n.NodeType == "Listening" &&
                                     JsonSerializer.Deserialize<ListeningNodeConfig>(n.ConfigurationJson).Endpoint == endpoint);

            if (listeningNode != null)
            {
                var context = new RuleExecutionContext { InputData = inputData, Result = new JsonObject() };
                await _ruleEngine.ExecuteRuleChain(ruleChain.RuleChainId, context);
                return Ok(new { message = "Rule chain executed successfully.", result = context.Result });
            }
        }

        return BadRequest(new { error = "No active rule chain listening on this endpoint." });
    }
}
