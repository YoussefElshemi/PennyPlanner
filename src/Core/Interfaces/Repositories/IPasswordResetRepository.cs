using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPasswordResetRepository
{
    Task CreateAsync(PasswordReset passwordReset);
    Task UpdateAsync(PasswordReset passwordReset);
    Task<bool> ExistsAsync(string passwordResetToken);
    Task<PasswordReset> GetAsync(string passwordResetToken);
}