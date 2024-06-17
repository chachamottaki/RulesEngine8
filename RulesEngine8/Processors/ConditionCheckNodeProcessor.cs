using System.Text.Json;
using System.Threading.Tasks;
using RulesEngine8.Models;
using RulesEngine8.Services;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace RulesEngine8.Processors
{
    public class ConditionCheckNodeProcessor : IRuleNodeProcessor
    {
        private readonly RulesEngineDBContext _context;

        public ConditionCheckNodeProcessor(RulesEngineDBContext context)
        {
            _context = context;
        }
        public string NodeType => "ConditionCheck";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var nodeConfig = JsonNode.Parse(node.ConfigurationJson);
            string condition = nodeConfig?["script"]?.ToString();

            var configItem = (ConfigItem)context.State["ConfigItem"];
            var alarm = (DI)context.State["Alarm"];

            if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("Condition cannot be null or empty.");
            }
            bool conditionMet = await EvaluateCondition(condition, context);
            
            if (conditionMet)
            {
                System.Diagnostics.Debug.WriteLine($"CONDITION IS MET {conditionMet}");
                context.State["ConditionMet"] = true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"CONDITION IS NOT MET {conditionMet}");
                context.State["ConditionMet"] = false;
                context.State["StopExecution"] = true;

                var historyRecord = new HistoryTable
                {
                    isAlarm = alarm.IsAlarm,
                    alarmId = alarm.alarmId,
                    DeviceId = configItem.DeviceID,
                    assetId = alarm.InstallationKey,
                    timestamp = DateTime.Now
                };
                _context.HistoryTables.Add(historyRecord);
                System.Diagnostics.Debug.WriteLine("ADD HISTORY RECORD FALSE");
                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        private async Task<bool> EvaluateCondition(string condition, RuleExecutionContext context)
        {
            // Prepare a scripting context
            var scriptOptions = ScriptOptions.Default
                .AddReferences(typeof(object).Assembly)
                .AddReferences(typeof(System.Linq.Expressions.Expression).Assembly)
                .AddReferences(typeof(System.Text.Json.JsonDocument).Assembly)
                .AddImports("System", "System.Linq", "System.Collections.Generic");

            // Create a scripting state with the context's state variables
            var globals = new ScriptGlobals { 
                State = context.State,
                sendEmail = (bool)context.State["sendEmail"],
                invertSendEmail = (bool)context.State["invertSendEmail"]
        };

            // Evaluate the condition dynamically
            var result = await CSharpScript.EvaluateAsync<bool>(condition, scriptOptions, globals);
            return result;
        }
    }

    public class ConditionCheckNodeConfig
    {
        public string Condition { get; set; }
    }

    public class ScriptGlobals
    {
        public Dictionary<string, object> State { get; set; }
        public bool sendEmail { get; set; }
        public bool invertSendEmail { get; set; }
    }
}
