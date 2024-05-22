using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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

            context.RuleChainId = ruleChainId;

            foreach (var node in ruleChain.Nodes)
            {
                System.Diagnostics.Debug.WriteLine(node.NodeType);
                await ProcessNodeAsync(node, context);
            }
        }

        private async Task ProcessNodeAsync(RuleNode node, RuleExecutionContext context)
        {
            // Check if the node has already been processed in this context
            if (context.ProcessedNodes.Contains(node.RuleNodeId))
            {
                System.Diagnostics.Debug.WriteLine($"Skipping already processed node {node.RuleNodeId}");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Processing node: {node.NodeType} with ID: {node.RuleNodeId}");
            context.ProcessedNodes.Add(node.RuleNodeId); // Mark this node as processed

            if (_nodeProcessors.TryGetValue(node.NodeType, out var processor))
            {
                await processor.ProcessAsync(node, context);

                foreach (var connection in node.NodeConnections)
                {
                    var targetNode = await _context.RuleNodes.FirstOrDefaultAsync(n => n.RuleNodeId == connection.TargetNodeIndex);
                    if (targetNode != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Moving from node {node.RuleNodeId} to node {targetNode.RuleNodeId}");
                        await ProcessNodeAsync(targetNode, context);
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
