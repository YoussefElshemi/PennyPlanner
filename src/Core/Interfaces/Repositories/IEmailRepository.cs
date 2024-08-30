using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IEmailRepository
{
    Task<bool> ExistsAsync(Guid emailId);
    Task<EmailMessage> GetAsync(Guid emailId);
    Task CreateAsync(EmailMessage emailMessage);
    Task UpdateAsync(EmailMessage emailMessage);
}