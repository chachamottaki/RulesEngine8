using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using RulesEngine8.Models;
using RulesEngine8.Services;

namespace RulesEngine8.Processors
{
    public class TransformNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Transform";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<TransformNodeConfig>(node.ConfigurationJson);

            var inputData = context.InputData;
            if (inputData != null)
            {
                var transformedData = ApplyTransformation(config, inputData);

                context.InputData = transformedData;
                context.Result = transformedData;
            }

            await Task.CompletedTask;
        }

        private JsonObject ApplyTransformation(TransformNodeConfig config, JsonObject inputData)
        {
            var value = inputData["value"]?.GetValue<int>() ?? 0;
            inputData["value"] = value * 2;
            return inputData;
        }
    }

    public class TransformNodeConfig
    {
        public string TransformationLogic { get; set; }
    }
}
