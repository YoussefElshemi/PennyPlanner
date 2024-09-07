using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;

namespace Core.Services;

public class OneTimePasscodeService(
    IEmailService emailService,
    IOneTimePasscodeRepository oneTimePasscodeRepository,
    IOptions<AppConfig> config,
    TimeProvider timeProvider) : IOneTimePasscodeService
{
    public Task CreateAsync(OneTimePasscode oneTimePasscode)
    {
        return oneTimePasscodeRepository.CreateAsync(oneTimePasscode);
    }

    public Task UpdateAsync(OneTimePasscode oneTimePasscode)
    {
        return oneTimePasscodeRepository.UpdateAsync(oneTimePasscode);
    }

    public Task<OneTimePasscode> GetAsync(UserId userId, Passcode passcode)
    {
        return oneTimePasscodeRepository.GetAsync(userId, passcode);
    }

    public async Task InitiateAsync(User user, IpAddress ipAddress)
    {
        var random = new Random();

        var oneTimePasscode = new OneTimePasscode
        {
            OneTimePasscodeId = new OneTimePasscodeId(Guid.NewGuid()),
            UserId = user.UserId,
            IpAddress = ipAddress,
            Passcode = new Passcode(random.Next(0, 999999).ToString("000000")),
            IsUsed = new IsUsed(false),
            ExpiresAt = new ExpiresAt(timeProvider.GetUtcNow().UtcDateTime.AddMinutes(config.Value.AuthenticationConfig.OneTimePasscodeLifetimeInMinutes)),
            CreatedBy = user.Username,
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedBy = user.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime),
            User = user
        };

        await CreateAsync(oneTimePasscode);

        var emailMessage = new EmailMessage
        {
            EmailId = new EmailId(Guid.NewGuid()),
            EmailAddress = user.EmailAddress,
            EmailSubject = new EmailSubject("One Time Passcode"),
            EmailBody = new EmailBody($"Your one time passcode is: {oneTimePasscode.Passcode}<br>It will expire in {config.Value.AuthenticationConfig.OneTimePasscodeLifetimeInMinutes} minutes."),
            IsProcessed = new IsProcessed(false),
            CreatedBy = user.Username,
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedBy = user.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await emailService.CreateAsync(emailMessage);
    }
}