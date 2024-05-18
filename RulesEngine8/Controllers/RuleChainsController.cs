using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesEngine8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleChainsController : ControllerBase
    {
        private readonly RulesEngineDBContext _context;

        public RuleChainsController(RulesEngineDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<RuleChain>> PostRuleChain(RuleChain ruleChain)
        {
            if (ruleChain.Nodes == null)
            {
                return BadRequest(new { error = "Nodes field is required but is null." });
            }

            // NodesJson is automatically set by the RuleChain model
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

            // Nodes are automatically deserialized by the RuleChain model
            return ruleChain;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleChain>>> GetRuleChains()
        {
            var ruleChains = await _context.RuleChains.ToListAsync();
            return ruleChains;
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
    }
}
