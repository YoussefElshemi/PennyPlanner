using System.Net.Mail;
using Infrastructure.Interfaces.Services;
using UnitTests.TestHelpers;

namespace BehaviouralTests.Mocks;

public class MockSmtpClient : BaseTestClass, ISmtpClient
{
    public Task SendMailAsync(MailMessage mailMessage)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}