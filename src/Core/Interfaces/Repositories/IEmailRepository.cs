using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IEmailRepository : IPagedRepository<EmailMessage>
{
    Task<bool> ExistsAsync(Guid emailId);
    Task<EmailMessage> GetAsync(Guid emailId);
    Task CreateAsync(EmailMessage emailMessage);
    Task UpdateAsync(EmailMessage emailMessage);
    Task<IEnumerable<EmailMessage>> GetAwaitingEmailsAsync();
}