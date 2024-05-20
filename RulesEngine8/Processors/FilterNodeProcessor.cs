using Newtonsoft.Json;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.Threading.Tasks;

namespace RulesEngine8.Processors
{
    public class FilterNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Filter";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonConvert.DeserializeObject<FilterNodeConfig>(node.ConfigurationJson);

            // Apply filter logic based on configuration
            if (!EvaluateFilter(config, context.InputData))
            {
                // Stop processing if the filter condition is not met
                throw new Exception("Filter condition not met");
            }

            await Task.CompletedTask;
        }

        private bool EvaluateFilter(FilterNodeConfig config, object inputData)
        {
            // Implement your filter evaluation logic here
            return true; // Return true if the condition is met, otherwise false
        }
    }

    public class FilterNodeConfig
    {
        public string FilterCondition { get; set; }
    }
}
