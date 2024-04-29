namespace RulesEngine8.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipient, string subject, string body);
    }
}
