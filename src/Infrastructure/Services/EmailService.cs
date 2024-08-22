using System.Net;
using System.Net.Mail;
using Core.Configs;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class EmailService(IOptions<AppConfig> config) : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var fromAddress = new MailAddress(config.Value.SmtpConfig.EmailAddress, config.Value.SmtpConfig.Name);
        var fromPassword = config.Value.SmtpConfig.Password;

        var toAddress = new MailAddress(emailMessage.EmailAddress);

        var smtp = new SmtpClient
        {
            Host = config.Value.SmtpConfig.Host,
            Port = config.Value.SmtpConfig.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using var message = new MailMessage(fromAddress, toAddress);

        message.Subject = emailMessage.EmailSubject;
        message.Body = emailMessage.EmailBody;
        message.IsBodyHtml = true;

        await smtp.SendMailAsync(message);
    }
}