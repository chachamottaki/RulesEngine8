using System.Threading.Tasks;
using RulesEngine8.Services;
using System.Text.Json.Nodes;
using RulesEngine8.Models;

namespace RulesEngine8.Processors
{
    public class EmailSendingNodeProcessor : IRuleNodeProcessor
    {
        private readonly IEmailService _emailService;

        public EmailSendingNodeProcessor(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public string NodeType => "EmailSending";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            if (context.State.ContainsKey("EmailRecipient") && context.State.ContainsKey("EmailContent"))
            {
                var recipient = context.State["EmailRecipient"].ToString();
                var content = context.State["EmailContent"].ToString();

                // Send the email
                await _emailService.SendEmailAsync(recipient, "Alarm Triggered", content);

                // Save history record
                var historyRecord = new HistoryTable
                {
                    isAlarm = ((DI)context.State["Alarm"]).IsAlarm,
                    alarmId = context.InputData["alarmId"].GetValue<string>(),
                    assetId = context.InputData["assetKey"].GetValue<string>(),
                    DeviceId = context.InputData["hostname"].GetValue<string>(),
                    emailSent = true,
                    emailRecipient = recipient,
                    emailContent = content,
                    timestamp = DateTime.Now
                };

                context.State["HistoryRecord"] = historyRecord;
            }

            await Task.CompletedTask;
        }
    }
}
