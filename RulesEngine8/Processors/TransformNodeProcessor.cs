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
            inputData["hostname"] = 6 ;
            inputData["district"] = 4;
            inputData["assetType"] = 34;
            inputData["assetKey"] = 33;
            inputData["sensorKey"] = 33;
            inputData["sensorType"] = 33;
            inputData["hostname"] = 33;
            inputData["sensor"] = 33;
            
            return inputData;
        }
    }
    public class TransformNodeConfig
    {
        public string TransformationLogic { get; set; }
    }
}
