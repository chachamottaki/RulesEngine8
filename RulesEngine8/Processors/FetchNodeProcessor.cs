using Newtonsoft.Json;
using RulesEngine8.Models;
using RulesEngine8.Services;
using System.Threading.Tasks;

namespace RulesEngine8.Processors
{
    public class FetchNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "Fetch";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonConvert.DeserializeObject<FetchNodeConfig>(node.ConfigurationJson);

            // Example processing (if needed)
            context.State["FetchedData"] = context.InputData;

            await Task.CompletedTask;
        }
    }

    public class FetchNodeConfig
    {
        public string DataSource { get; set; }
    }
}
