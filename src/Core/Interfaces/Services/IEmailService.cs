using Core.Models;

namespace Core.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}