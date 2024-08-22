using Core.Configs;
using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;

namespace Core.Services;

public class PasswordResetService(
    IPasswordResetRepository passwordResetRepository,
    IEmailService emailService,
    IOptions<AppConfig> appConfig,
    TimeProvider timeProvider) : IPasswordResetService
{
    public async Task InitiateAsync(User user)
    {
        var passwordReset = new PasswordReset
        {
            PasswordResetId = new PasswordResetId(Guid.NewGuid()),
            UserId = user.UserId,
            User = user,
            ResetToken = new PasswordResetToken(Guid.NewGuid()),
            IsUsed = new IsUsed(false),
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await passwordResetRepository.CreateAsync(passwordReset);

        var passwordResetUrl = $"{appConfig.Value.ServiceConfig.BaseUrl}{ApiUrls.Authentication.ResetPassword}?token={passwordReset.ResetToken}";

        var emailMessage = new EmailMessage
        {
            EmailAddress = user.EmailAddress,
            EmailSubject = new EmailSubject("Password Reset"),
            EmailBody = new EmailBody($"Please reset your password by clicking <a href={passwordResetUrl}>here</a>")
        };

        await emailService.SendEmailAsync(emailMessage);
    }
}