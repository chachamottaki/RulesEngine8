using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RulesEngine8.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            // Replace these with your actual SMTP server credentials
            string smtpServer = _configuration["SmtpSettings:Host"];
            string smtpUsername = _configuration["SmtpSettings:Username"];
            string smtpPassword = _configuration["SmtpSettings:Password"];
            int smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");

            // Create SmtpClient within a using block for proper disposal
            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                // Create MailMessage object
                var message = new MailMessage
                {
                    From = new MailAddress("noreply@gmail.com", "TPST"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(recipient);

                try
                {
                    // Send email asynchronously
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    // Handle exceptions here
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw; // Rethrow the exception to propagate it upwards
                }
            }
        }
    }
}
