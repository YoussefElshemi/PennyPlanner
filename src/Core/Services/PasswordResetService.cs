using Core.Configs;
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
            ResetToken = new PasswordResetToken(Guid.NewGuid().ToString("N")),
            IsUsed = new IsUsed(false),
            CreatedBy = user.Username,
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedBy = user.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await passwordResetRepository.CreateAsync(passwordReset);

        var passwordResetUrl = $"{appConfig.Value.ServiceConfig.PasswordResetUrl}?token={passwordReset.ResetToken}";

        var emailMessage = new EmailMessage
        {
            EmailAddress = user.EmailAddress,
            EmailSubject = new EmailSubject("Password Reset"),
            EmailBody = new EmailBody($"Please reset your password by clicking <a href={passwordResetUrl}>here</a>")
        };

        await emailService.SendEmailAsync(emailMessage);
    }

    public Task CreateAsync(PasswordReset passwordReset)
    {
        return passwordResetRepository.CreateAsync(passwordReset);
    }

    public Task UpdateAsync(PasswordReset passwordReset)
    {
        return passwordResetRepository.UpdateAsync(passwordReset);
    }

    public Task<bool> ExistsAsync(PasswordResetToken passwordResetToken)
    {
        return passwordResetRepository.ExistsAsync(passwordResetToken);
    }

    public Task<PasswordReset> GetAsync(PasswordResetToken passwordResetToken)
    {
        return passwordResetRepository.GetAsync(passwordResetToken);
    }
}