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
    public Task CreateAsync(EmailMessage emailMessage)
    {
        return Task.CompletedTask;
    }

    public Task ProcessAwaitingEmailsAsync()
    {
        return Task.CompletedTask;
    }
}