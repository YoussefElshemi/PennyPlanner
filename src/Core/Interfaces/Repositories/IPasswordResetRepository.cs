using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPasswordResetRepository
{
    Task CreateAsync(PasswordReset passwordReset);
    Task UpdateAsync(PasswordReset passwordReset);
    Task<bool> ExistsAsync(Guid passwordResetToken);
    Task<PasswordReset> GetAsync(Guid passwordResetToken);
}