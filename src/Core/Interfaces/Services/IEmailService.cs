using Core.Models;

namespace Core.Interfaces.Services;

public interface IEmailService
{
    Task<PagedResponse<EmailMessage>> GetAllAsync(PagedRequest pagedRequest);
    Task CreateAsync(EmailMessage emailMessage);
    Task ProcessAwaitingEmailsAsync();
}