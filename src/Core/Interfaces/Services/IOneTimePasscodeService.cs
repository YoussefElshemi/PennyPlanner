using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IOneTimePasscodeService
{
    Task InitiateAsync(User user, IpAddress authenticationRequestIpAddress);
    Task CreateAsync(OneTimePasscode oneTimePasscode);
    Task UpdateAsync(OneTimePasscode oneTimePasscode);
    Task<OneTimePasscode> GetAsync(UserId userId, Passcode refreshToken);
}