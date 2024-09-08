using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using Presentation.WebApi.Authentication.Endpoints;
using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;
using Xunit;

namespace BehaviouralTests.Tests.Endpoints.AuthenticationTests;

[Collection("Sequential")]
public class RequestPasswordResetTests(TestFixture testFixture) : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IServiceProvider _serviceProvider = testFixture.Services;

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    [Fact]
    public async Task RequestPasswordReset_UserDoesNotExist_ReturnsAccepted()
    {
        // Arrange
        var requestPasswordResetRequest = FakeRequestPasswordResetRequestDto.CreateValid();
        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            EmailAddress = string.Join("", requestPasswordResetRequest.EmailAddress.ToCharArray().Reverse()),
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var httpResponseMessage =
            await testFixture.Client.POSTAsync<RequestPasswordReset, RequestPasswordResetRequestDto>(requestPasswordResetRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Accepted);
        await AssertPasswordResetExists(existingUserEntity.UserId, false);
    }

    [Fact]
    public async Task RequestPasswordReset_UserDoesExist_ReturnsAccepted()
    {
        // Arrange
        var requestPasswordResetRequest = FakeRequestPasswordResetRequestDto.CreateValid();
        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            EmailAddress = requestPasswordResetRequest.EmailAddress,
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var httpResponseMessage =
            await testFixture.Client.POSTAsync<RequestPasswordReset, RequestPasswordResetRequestDto>(requestPasswordResetRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Accepted);
        await AssertPasswordResetExists(existingUserEntity.UserId, true);
    }

    private async Task AssertPasswordResetExists(Guid userId, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

        try
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await context.PasswordResets.FirstAsync(x => x.UserId == userId);
            });
        }
        catch (Exception)
        {
            if (expected) throw;
        }
    }

    protected override async Task TearDownAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }

}