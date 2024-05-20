using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using RulesEngine8.Models;
using RulesEngine8.Services;

namespace RulesEngine8.Processors
{
    public class FilterNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Filter";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<FilterNodeConfig>(node.ConfigurationJson);

            var inputData = context.InputData;
            System.Diagnostics.Debug.WriteLine($"ARIANAAAAA: {node.RuleChainID},{node.NodeType}");
            if (inputData != null)
            {
                if (!EvaluateFilter(config, inputData))
                {
                    throw new Exception("Filter condition not met");
                }

                context.Result = inputData;
            }

            await Task.CompletedTask;
        }

        public bool EvaluateFilter(FilterNodeConfig config, JsonObject inputData)
        {
            //var value = inputData["value"]?.GetValue<int>() ?? 0;
            //Console.WriteLine($"Evaluating filter: inputData['value'] = {value}, Condition: {config.FilterCondition}");

            //// Replace this line with actual condition evaluation logic if needed
            //bool result = value > 10;
            //Console.WriteLine($"Filter result: {result}");
            return false;
        }
    }

    public class FilterNodeConfig
    {
        public string FilterCondition { get; set; }
    }
}
