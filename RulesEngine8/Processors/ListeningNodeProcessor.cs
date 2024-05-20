using System.Text.Json;
using System.Threading.Tasks;
using RulesEngine8.Models;
using RulesEngine8.Services;

namespace RulesEngine8.Processors
{
    public class ListeningNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Listening";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<ListeningNodeConfig>(node.ConfigurationJson);

            // Store the endpoint in the context
            context.ListeningEndpoint = config.Endpoint;
            System.Diagnostics.Debug.WriteLine(context.ListeningEndpoint);

            // The listening node will wait for data from the SensorController endpoint
            await Task.CompletedTask;
        }
    }

    public class ListeningNodeConfig
    {
        public string Endpoint { get; set; }
    }
}
