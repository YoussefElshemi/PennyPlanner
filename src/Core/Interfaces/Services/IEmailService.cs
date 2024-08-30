using Core.Models;

namespace Core.Interfaces.Services;

public interface IEmailService
{
    Task CreateAsync(EmailMessage emailMessage);
    Task ProcessAwaitingEmailsAsync();
}