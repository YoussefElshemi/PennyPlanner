using Core.Interfaces.Services;
using Core.Models;

namespace BehaviouralTests.Mocks;

public class MockEmailService : IEmailService
{
    public Task SendEmailAsync(EmailMessage emailMessage)
    {
        return Task.CompletedTask;
    }
}