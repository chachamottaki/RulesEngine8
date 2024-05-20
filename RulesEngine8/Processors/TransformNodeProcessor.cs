using Newtonsoft.Json;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.Threading.Tasks;

namespace RulesEngine8.Processors
{
    public class TransformNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Transform";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonConvert.DeserializeObject<TransformNodeConfig>(node.ConfigurationJson);

            // Apply transformation logic based on configuration
            context.InputData = ApplyTransformation(config, context.InputData);

            await Task.CompletedTask;
        }

        private object ApplyTransformation(TransformNodeConfig config, object inputData)
        {
            // Implement your transformation logic here
            return inputData; // Return the transformed data
        }
    }

    public class TransformNodeConfig
    {
        public string TransformationLogic { get; set; }
    }
}
