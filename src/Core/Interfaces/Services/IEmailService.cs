using Core.Models;

namespace Core.Interfaces.Services;

public interface IEmailService
{
    IDictionary<string, string> GetSortableFields();
    IDictionary<string, string> GetSearchableFields();
    Task<PagedResponse<EmailMessage>> GetAllAsync(PagedRequest pagedRequest);
    Task CreateAsync(EmailMessage emailMessage);
    Task ProcessAwaitingEmailsAsync();
}