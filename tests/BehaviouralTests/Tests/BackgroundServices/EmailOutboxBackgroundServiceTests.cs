using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using FastEndpoints.Testing;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using Xunit;

namespace BehaviouralTests.Tests.BackgroundServices;

[Collection("Sequential")]
public class EmailOutboxBackgroundServiceTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly TestFixture _testFixture;

    public EmailOutboxBackgroundServiceTests(TestFixture testFixture)
    {
        ServicePointManager.ServerCertificateValidationCallback += (_, _, _, _) => true;

        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        _fixture = AutoFixtureHelper.Create();
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    [Fact]
    public async Task ExecuteAsync_NoEmailsToSend_DoesNotSendEmails()
    {
        await AssertEmailIsSent(0);
    }

    [Fact]
    public async Task ExecuteAsync_EmailToSend_SendEmails()
    {
        // Arrange
        var email = FakeEmailMessageOutboxEntity.CreateValid(_fixture) with
        {
            EmailAddress = FakeEmailAddress.Valid,
            IsProcessed = false
        };

        // Act
        await InsertEmails([email]);

        // Assert
        await AssertEmailIsSent(1);
    }

    [Fact]
    public async Task ExecuteAsync_ManyEmailsToSend_SendEmails()
    {
        // Arrange
        const int count = 5;
        var emails = Enumerable.Range(0, count)
            .Select(_ => FakeEmailMessageOutboxEntity.CreateValid(_fixture) with
            {
                EmailAddress = FakeEmailAddress.Valid,
                IsProcessed = false
            }).ToList();

        // Act
        await InsertEmails(emails);

        // Assert
        await AssertEmailIsSent(count);
    }

    private async Task InsertEmails(List<EmailMessageOutboxEntity> emailMessageOutboxEntity)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

        await context.Emails.AddRangeAsync(emailMessageOutboxEntity);
        await context.SaveChangesAsync();
    }

    private async Task AssertEmailIsSent(int count)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:1080/api/emails");
            var responseString = await response.Content.ReadAsStringAsync();

            var emails = JsonConvert.DeserializeObject<List<object>>(responseString) ?? [];

            if (emails.Count != count)
            {
                throw new Exception($"Expected {count} emails but received {emails.Count}.");
            }
        });
    }

    protected override async Task TearDownAsync()
    {
        await new HttpClient().DeleteAsync("http://localhost:1080/api/emails");
        await _testFixture.ResetDatabaseAsync();
    }
}