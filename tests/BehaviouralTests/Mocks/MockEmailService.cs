using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace BehaviouralTests.Mocks;

public class MockEmailService : BaseTestClass, IEmailService
{
    private readonly IFixture _autoFixture = AutoFixtureHelper.Create();
    public Task SendEmailAsync(EmailMessage emailMessage)
    {
        return Task.CompletedTask;
    }

    public Task<EmailMessage> RedriveEmailAsync(EmailId emailId)
    {
        var emailMessage = FakeEmailMessage.CreateValid(_autoFixture);
        return Task.FromResult(emailMessage);
    }
}