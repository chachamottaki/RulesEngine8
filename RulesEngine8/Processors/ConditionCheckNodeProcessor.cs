using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using RulesEngine8.Models;
using RulesEngine8.Services;

namespace RulesEngine8.Processors
{
    public class ConditionCheckNodeProcessor : IRuleNodeProcessor
    {
        public string NodeType => "ConditionCheck";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<ConditionCheckNodeConfig>(node.ConfigurationJson);
            var inputData = context.InputData;

            if (inputData != null && context.State.ContainsKey("DigitalInputs"))
            {
                var digitalInputs = (List<DI>)context.State["DigitalInputs"];
                var alarm = digitalInputs.FirstOrDefault(di => di.alarmId == inputData["alarmId"].GetValue<string>());

                if (alarm != null)
                {
                    context.State["Alarm"] = alarm;
                    context.State["ShouldSendEmail"] = alarm.sendEmail && !alarm.Invert;
                }
            }

            await Task.CompletedTask;
        }
    }

    public class ConditionCheckNodeConfig
    {
        public string Condition { get; set; }
    }
}
