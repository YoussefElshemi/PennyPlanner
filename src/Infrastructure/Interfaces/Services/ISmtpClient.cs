using System.Net.Mail;

namespace Infrastructure.Interfaces.Services;

public interface ISmtpClient : IDisposable
{
    void Send(MailMessage mailMessage);
    Task SendMailAsync(MailMessage mailMessage);
}