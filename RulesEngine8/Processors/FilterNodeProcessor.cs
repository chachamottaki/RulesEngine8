using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System;
using System.Threading.Tasks;

namespace RulesEngine8.Processors
{
    public class FilterNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Filter";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonConvert.DeserializeObject<FilterNodeConfig>(node.ConfigurationJson);

            var inputData = context.InputData;
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

        public bool EvaluateFilter(FilterNodeConfig config, JObject inputData)
        {
            System.Diagnostics.Debug.WriteLine($"inputData fr fr: {inputData}");
            var value = inputData["value"]?.Value<int>() ?? 0;
            System.Diagnostics.Debug.WriteLine($"Evaluating filter: inputData['value'] = {value}, Condition: {config.FilterCondition}");

            // Replace this line with actual condition evaluation logic if needed
            bool result = value > 10;
            Console.WriteLine();
            System.Diagnostics.Debug.WriteLine($"Filter result: {result}");
            return result;
        }
    }

    public class FilterNodeConfig
    {
        public string FilterCondition { get; set; }
    }
}
