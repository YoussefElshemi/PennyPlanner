using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IEmailRepository
{
    Task CreateAsync(EmailMessage emailMessage);
    Task UpdateAsync(EmailMessage emailMessage);
}