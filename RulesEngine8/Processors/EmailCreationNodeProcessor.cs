using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using RulesEngine8.Models;
using RulesEngine8.Services;

namespace RulesEngine8.Processors
{
    public class EmailCreationNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "EmailCreation";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<EmailCreationNodeConfig>(node.ConfigurationJson);

            if (context.State.ContainsKey("ConfigItem") && context.State.ContainsKey("Alarm") && (bool)context.State["ShouldSendEmail"])
            {
                var configItem = (ConfigItem)context.State["ConfigItem"];
                var alarm = (DI)context.State["Alarm"];

                string deviceID = configItem.DeviceID;
                string assetKey = context.InputData["assetKey"].GetValue<string>();
                string shortDescription = alarm.shortDescription;
                string longDescription = alarm.longDescription;
                string recipient = configItem.Config.email.ToString();
                string email = $"Hi! Alarm Triggered for device {deviceID}; asset {assetKey}! Short description: {shortDescription}, Long Description: {longDescription}";

                context.State["EmailRecipient"] = recipient;
                context.State["EmailContent"] = email;
            }

            await Task.CompletedTask;
        }
    }

    public class EmailCreationNodeConfig
    {
        public string EmailTemplate { get; set; }
    }
}
