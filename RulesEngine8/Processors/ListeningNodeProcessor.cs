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

            // give inputData to next node, or any processing needed

            await Task.CompletedTask;
        }
    }

    public class ListeningNodeConfig
    {
        public string Endpoint { get; set; }
    }
}
