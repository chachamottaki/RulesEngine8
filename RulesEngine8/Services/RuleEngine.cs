using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RulesEngine8.Models;

namespace RulesEngine8.Services
{
    public class RuleEngine : IRuleEngine
    {
        private readonly RulesEngineDBContext _context;
        private readonly Dictionary<string, IRuleNodeProcessor> _nodeProcessors;
        private readonly ConcurrentDictionary<string, RuleExecutionContext> _activeContexts;

        public RuleEngine(RulesEngineDBContext context, IEnumerable<IRuleNodeProcessor> processors)
        {
            _context = context;
            _nodeProcessors = processors.ToDictionary(p => p.NodeType, p => p);
            _activeContexts = new ConcurrentDictionary<string, RuleExecutionContext>();
        }

        public async Task ExecuteRuleChain(int ruleChainId, RuleExecutionContext context)
        {
            var ruleChain = await _context.RuleChains
                .Include(rc => rc.Nodes)
                .FirstOrDefaultAsync(rc => rc.RuleChainId == ruleChainId);

            if (ruleChain == null)
                throw new Exception("Rule chain not found");

            context.RuleChainId = ruleChainId;

            foreach (var node in ruleChain.Nodes)
            {
                await ProcessNodeAsync(node, context);
            }

            if (!string.IsNullOrEmpty(context.ListeningEndpoint))
            {
                _activeContexts[context.ListeningEndpoint] = context;
            }
        }

        private async Task ProcessNodeAsync(RuleNode node, RuleExecutionContext context)
        {
            if (_nodeProcessors.TryGetValue(node.NodeType, out var processor))
            {
                await processor.ProcessAsync(node, context);

                foreach (var connection in node.NodeConnections)
                {
                    var targetNode = await _context.RuleNodes.FirstOrDefaultAsync(n => n.RuleNodeId == connection.TargetNodeIndex);
                    if (targetNode != null)
                    {
                        await ProcessNodeAsync(targetNode, context);
                    }
                }
            }
            else
            {
                throw new Exception($"No processor found for node type {node.NodeType}");
            }
        }

        public RuleExecutionContext GetActiveRuleExecutionContext(string endpoint)
        {
            _activeContexts.TryGetValue(endpoint, out var context);
            return context;
        }
    }
}
