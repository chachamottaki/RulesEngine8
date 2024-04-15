using System.Net;
using System.Net.Mail;

namespace RulesEngine8.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string recipient, string subject, string body)
        {
            // Set up SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mottakichaimae@gmail.com", "Ch26???888"),
                EnableSsl = true
            };

            // Create MailMessage object
            MailMessage message = new MailMessage
            {
                From = new MailAddress("noreply@gmail.com", "TPST"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(recipient);

            // Send email
            client.Send(message);
        }
    }

}
