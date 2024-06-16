using System.Threading.Tasks;
using RulesEngine8.Services;
using System.Text.Json.Nodes;
using RulesEngine8.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace RulesEngine8.Processors
{
    public class EmailSendingNodeProcessor : IRuleNodeProcessor
    {
        //private readonly IEmailService _emailService;
        private readonly RulesEngineDBContext _context;

        public EmailSendingNodeProcessor(IEmailService emailService, RulesEngineDBContext context)
        {
            //_emailService = emailService;
            _context = context;
        }

        public string NodeType => "EmailSending";

        public async Task ProcessAsync(RuleNode node, RuleExecutionContext context)
        {
            var configItem = (ConfigItem)context.State["ConfigItem"];
            var alarm = (DI)context.State["Alarm"];
            System.Diagnostics.Debug.WriteLine("IN EMAILSENDING NODE");
            if (context.State.ContainsKey("EmailRecipient") && context.State.ContainsKey("EmailContent"))
            {
                var recipient = context.State["EmailRecipient"].ToString();
                var content = context.State["EmailContent"].ToString();
                bool shouldSendEmail = (context.State.ContainsKey("sendEmail") && (bool)context.State["sendEmail"]) &&
                                       (!context.State.ContainsKey("invertSendEmail") || !(bool)context.State["invertSendEmail"]);
                if (shouldSendEmail)
                {
                    var historyRecord = new HistoryTable
                    {
                        isAlarm = alarm.IsAlarm,
                        alarmId = alarm.alarmId,
                        assetId = alarm.InstallationKey,
                        DeviceId = configItem.DeviceID,
                        emailSent = true,
                        emailRecipient = recipient,
                        emailContent = (string)context.State["EmailContent"],
                        timestamp = DateTime.UtcNow
                    };
                    _context.HistoryTables.Add(historyRecord);
                    System.Diagnostics.Debug.WriteLine("ADD HISTORY RECORD FALSE");
                    await _context.SaveChangesAsync();

                    // Send the email
                    //await _emailService.SendEmailAsync(recipient, "Alarm Triggered", content);
                }
                else
                {
                    var historyRecord = new HistoryTable
                    {
                        isAlarm = alarm.IsAlarm,
                        alarmId = alarm.alarmId,
                        assetId = alarm.InstallationKey,
                        emailSent = true,
                        emailRecipient = recipient,
                        emailContent = content,
                        timestamp = DateTime.UtcNow
                    };
                    _context.HistoryTables.Add(historyRecord);
                    System.Diagnostics.Debug.WriteLine("ADD HISTORY RECORD FALSE");
                    await _context.SaveChangesAsync();   
                }
            }
            await Task.CompletedTask;
        }
    }
}
