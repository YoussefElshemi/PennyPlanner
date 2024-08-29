using System.Net;
using System.Net.Mail;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Infrastructure.Services;

public class EmailService(IEmailRepository emailRepository, IOptions<AppConfig> config) : IEmailService
{
    private readonly AsyncRetryPolicy _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(config.Value.SmtpConfig.NumberOfRetries,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await emailRepository.CreateAsync(emailMessage);

        var fromAddress = new MailAddress(config.Value.SmtpConfig.EmailAddress, config.Value.SmtpConfig.Name);
        var credentials = new NetworkCredential(fromAddress.Address, config.Value.SmtpConfig.Password);

        var toAddress = new MailAddress(emailMessage.EmailAddress);
        using var message = new MailMessage(fromAddress, toAddress);

        message.Subject = emailMessage.EmailSubject;
        message.Body = emailMessage.EmailBody;
        message.IsBodyHtml = true;

        await _retryPolicy.ExecuteAsync(async () =>
        {
            var smtp = new SmtpClient
            {
                Host = config.Value.SmtpConfig.Host,
                Port = config.Value.SmtpConfig.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = credentials
            };

            await smtp.SendMailAsync(message);

            emailMessage.IsProcessed = new IsProcessed(true);
            emailMessage.UpdatedBy = new Username(Username.SystemUsername);
            await emailRepository.UpdateAsync(emailMessage);
        });
    }
}