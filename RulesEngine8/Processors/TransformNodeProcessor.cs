using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            var inputData = context.InputData;
            if (inputData != null)
            {
                var transformedData = ApplyTransformation(config, inputData);

                context.InputData = transformedData;
                context.Result = transformedData;
            }

            await Task.CompletedTask;
        }

        private JObject ApplyTransformation(TransformNodeConfig config, JObject inputData)
        {
            var value = inputData["value"]?.Value<int>() ?? 0;
            inputData["value"] = value * 2;
            return inputData;
        }
    }

    public class TransformNodeConfig
    {
        public string TransformationLogic { get; set; }
    }
}
