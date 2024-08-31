using System.Net.Mail;

namespace Infrastructure.Interfaces.Services;

public interface ISmtpClient : IDisposable
{
    Task SendMailAsync(MailMessage mailMessage);
}