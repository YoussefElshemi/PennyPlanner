using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IPasswordResetService
{
    Task InitiateAsync(User user);
    Task CreateAsync(PasswordReset passwordReset);
    Task UpdateAsync(PasswordReset passwordReset);
    Task<bool> ExistsAsync(PasswordResetToken passwordResetToken);
    Task<PasswordReset> GetAsync(PasswordResetToken passwordResetToken);
}