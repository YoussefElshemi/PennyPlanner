using System.Net.Mail;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Infrastructure.Services;

public class EmailService(ISmtpClient smtpClient,
    IEmailRepository emailRepository,
    TimeProvider timeProvider,
    IOptions<AppConfig> config) : IEmailService
{
    private readonly AsyncRetryPolicy _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(config.Value.SmtpConfig.NumberOfRetries,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public Task<PagedResponse<EmailMessage>> GetAllAsync(PagedRequest pagedRequest)
    {
        return emailRepository.GetAllAsync<EmailMessage>(pagedRequest);
    }

    public async Task CreateAsync(EmailMessage emailMessage)
    {
        await emailRepository.CreateAsync(emailMessage);
    }

    public async Task ProcessAwaitingEmailsAsync()
    {
        var emailsToSend = await emailRepository.GetAwaitingEmailsAsync();

        foreach (var email in emailsToSend)
        {
            await SendEmailAsync(email);
        }
    }

    private async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var success = await SendMessageAsync(emailMessage);

        emailMessage = emailMessage with
        {
            IsProcessed = new IsProcessed(success),
            UpdatedBy = new Username(Username.SystemUsername),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await emailRepository.UpdateAsync(emailMessage);
    }

    private async Task<bool> SendMessageAsync(EmailMessage emailMessage)
    {
        var fromAddress = new MailAddress(config.Value.SmtpConfig.EmailAddress, config.Value.SmtpConfig.Name);
        var toAddress = new MailAddress(emailMessage.EmailAddress);
        using var message = new MailMessage(fromAddress, toAddress);

        message.Subject = emailMessage.EmailSubject;
        message.Body = emailMessage.EmailBody;
        message.IsBodyHtml = true;

        var result = await _retryPolicy.ExecuteAndCaptureAsync(async () =>
        {
            await smtpClient.SendMailAsync(message);
        });

        return result.Outcome == OutcomeType.Successful;
    }
}