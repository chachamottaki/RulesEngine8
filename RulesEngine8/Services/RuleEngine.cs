using System;
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

        public RuleEngine(RulesEngineDBContext context, IEnumerable<IRuleNodeProcessor> processors)
        {
            _context = context;
            _nodeProcessors = processors.ToDictionary(p => p.NodeType, p => p);
        }

        public async Task ExecuteRuleChain(int ruleChainId, RuleExecutionContext context)
        {
            var ruleChain = await _context.RuleChains
                .Include(rc => rc.Nodes)
                .FirstOrDefaultAsync(rc => rc.RuleChainId == ruleChainId);

            if (ruleChain == null)
                throw new Exception("Rule chain not found");

            // Initialize a HashSet to track processed nodes
            var processedNodes = new HashSet<int>();

            foreach (var node in ruleChain.Nodes)
            {
                await ProcessNodeAsync(node, context, processedNodes);
            }
        }

        private async Task ProcessNodeAsync(RuleNode node, RuleExecutionContext context, HashSet<int> processedNodes)
        {
            // Skip if the node has already been processed
            if (processedNodes.Contains(node.RuleNodeId))
                return;

            // Mark the node as processed
            processedNodes.Add(node.RuleNodeId);

            if (_nodeProcessors.TryGetValue(node.NodeType, out var processor))
            {
                await processor.ProcessAsync(node, context);

                foreach (var connection in node.NodeConnections)
                {
                    var targetNode = await _context.RuleNodes.FirstOrDefaultAsync(n => n.RuleNodeId == connection.TargetNodeIndex);
                    if (targetNode != null)
                    {
                        await ProcessNodeAsync(targetNode, context, processedNodes);
                    }
                }
            }
            else
            {
                throw new Exception($"No processor found for node type {node.NodeType}");
            }
        }
    }
}
