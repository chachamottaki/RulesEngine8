using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace RulesEngine8.Controllers
{
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

            // Check if there's any active rule chain listening to this endpoint
            var context = _ruleEngine.GetActiveRuleExecutionContext(HttpContext.Request.Path.Value);

            if (context != null)
            {
                context.InputData = inputData;
                await _ruleEngine.ExecuteRuleChain(context.RuleChainId, context);
                return Ok(new { message = "Rule chain executed successfully.", result = context.Result });
            }

            return BadRequest(new { error = "No active rule chain listening on this endpoint." });
        }
    }
}
