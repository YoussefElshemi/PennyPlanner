using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IOneTimePasscodeRepository
{
    Task CreateAsync(OneTimePasscode oneTimePasscode);
    Task UpdateAsync(OneTimePasscode oneTimePasscode);
    Task<bool> ExistsAsync(Guid userId, string passcode);
    Task<OneTimePasscode> GetAsync(Guid userId, string passcode);
}