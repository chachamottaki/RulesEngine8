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
            string emailTemplate = nodeConfig?["content"]?.ToString();

            if (context.State.ContainsKey("ConfigItem") && context.State.ContainsKey("Alarm"))
            {
                var configItem = (ConfigItem)context.State["ConfigItem"];
                var alarm = (DI)context.State["Alarm"];

                string deviceID = configItem.DeviceID;
                string assetKey = alarm.InstallationKey;
                string shortDescription = alarm.shortDescription;
                string longDescription = alarm.longDescription;
                string email = emailTemplate;
                string alarmID = (string)context.State["AlarmId"];


                //add other inputs like sensortype, sensorkey, assetType, district,.. (look on TPST)

                string defaultRecipient = configItem.Config.email.ToString();
                string userRecipient = nodeConfig?["recipient"]?.ToString(); // Get user-provided recipient
                string recipient;
                if (!string.IsNullOrEmpty(userRecipient) && userRecipient != "{{recipientEmail}}")
                {
                    recipient = userRecipient; 
                }
                else
                {
                    recipient = defaultRecipient; 
                }


                if (email.Contains("{DeviceID}") && !string.IsNullOrEmpty(deviceID))
                {
                    email = email.Replace("{DeviceID}", deviceID);
                }
                if (email.Contains("{assetKey}") && !string.IsNullOrEmpty(assetKey))
                {
                    email = email.Replace("{assetKey}", assetKey);
                }
                if (email.Contains("{shortDescription}") && !string.IsNullOrEmpty(shortDescription))
                {
                    email = email.Replace("{shortDescription}", shortDescription);
                }
                if (email.Contains("{longDescription}") && !string.IsNullOrEmpty(longDescription))
                {
                    email = email.Replace("{longDescription}", longDescription);
                }
                if (email.Contains("{AlarmID}") && !string.IsNullOrEmpty(alarmID))
                {
                    email = email.Replace("{AlarmID}", alarmID);
                }


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
