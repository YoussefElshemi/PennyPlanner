using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
    Task<EmailMessage> RedriveEmailAsync(EmailId emailId);
}