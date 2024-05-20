using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace RulesEngine8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleChainsController : ControllerBase
    {
        private readonly RulesEngineDBContext _context;
        private readonly IRuleEngine _ruleEngine;

        public RuleChainsController(RulesEngineDBContext context, IRuleEngine ruleEngine)
        {
            _context = context;
            _ruleEngine = ruleEngine;
        }

        [HttpPost]
        public async Task<ActionResult<RuleChain>> PostRuleChain(RuleChain ruleChain)
        {
            if (ruleChain.Nodes == null)
            {
                return BadRequest(new { error = "Nodes field is required but is null." });
            }

            // Ensure ConfigurationJson is correctly serialized
            foreach (var node in ruleChain.Nodes)
            {
                node.ConfigurationJson = JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonElement>(node.ConfigurationJson));
            }

            _context.RuleChains.Add(ruleChain);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRuleChain), new { id = ruleChain.RuleChainId }, ruleChain);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RuleChain>> GetRuleChain(int id)
        {
            var ruleChain = await _context.RuleChains.FindAsync(id);

            if (ruleChain == null)
            {
                return NotFound();
            }

            return ruleChain;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleChain>>> GetRuleChains()
        {
            return await _context.RuleChains.ToListAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuleChain(int id)
        {
            var ruleChain = await _context.RuleChains.FindAsync(id);
            if (ruleChain == null)
            {
                return NotFound();
            }

            _context.RuleChains.Remove(ruleChain);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{ruleChainId}/nodes")]
        public async Task<ActionResult<RuleNode>> PostRuleNode(int ruleChainId, RuleNode ruleNode)
        {
            ruleNode.RuleChainID = ruleChainId;
            _context.RuleNodes.Add(ruleNode);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRuleNode), new { id = ruleNode.RuleNodeId }, ruleNode);
        }

        [HttpGet("{ruleChainId}/nodes/{nodeId}")]
        public async Task<ActionResult<RuleNode>> GetRuleNode(int ruleChainId, int nodeId)
        {
            var ruleNode = await _context.RuleNodes
                .FirstOrDefaultAsync(rn => rn.RuleNodeId == nodeId && rn.RuleChainID == ruleChainId);

            if (ruleNode == null)
                return NotFound();

            return ruleNode;
        }

        [HttpPost("{id}/execute")]
        public async Task<IActionResult> ExecuteRuleChain(int id)
        {
            try
            {
                var context = new RuleExecutionContext { Result = new JsonObject() };

                System.Diagnostics.Debug.WriteLine($"Executing rule chain ID: {id}");

                await _ruleEngine.ExecuteRuleChain(id, context);

                return Ok(new { message = "Rule chain executed successfully, waiting for data.", result = context.Result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
