using Core.Models;

namespace Core.Interfaces.Services;

public interface IPasswordResetService
{
    Task InitiateAsync(User user);
}