using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using RulesEngine8.Models;
using RulesEngine8.Services;

namespace RulesEngine8.Processors
{
    public class EmailCreationNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "EmailCreation";
        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var nodeConfig = JsonNode.Parse(node.ConfigurationJson);
            string emailTemplate = nodeConfig?["emailTemplate"]?.ToString();

            if (context.State.ContainsKey("ConfigItem") && context.State.ContainsKey("Alarm"))
            {
                var configItem = (ConfigItem)context.State["ConfigItem"];
                var alarm = (DI)context.State["Alarm"];

                string deviceID = configItem.DeviceID;
                string assetKey = alarm.InstallationKey;
                string shortDescription = alarm.shortDescription;
                string longDescription = alarm.longDescription;
                string recipient = configItem.Config.email.ToString();
                string email = emailTemplate;
                //add other inputs like sensortype, sensorkey, assetType, district,.. (look on TPST)

                email = email.Replace("{deviceID}", deviceID)
                         .Replace("{assetKey}", assetKey)
                         .Replace("{shortDescription}", shortDescription)
                         .Replace("{longDescription}", longDescription);

                context.State["EmailRecipient"] = recipient;
                context.State["EmailContent"] = email;
                context.Result["email"] = email;
                context.Result["recipient"] = recipient;
            }
            await Task.CompletedTask;
        }
    }
    public class EmailCreationNodeConfig
    {
        public string EmailTemplate { get; set; }
    }
}
