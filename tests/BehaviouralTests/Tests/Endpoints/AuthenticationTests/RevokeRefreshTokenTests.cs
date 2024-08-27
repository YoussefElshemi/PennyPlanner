using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Authentication.Endpoints;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;
using Xunit;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.AuthenticationTests;

[Collection("Sequential")]
public class RevokeRefreshTokenTests(TestFixture testFixture) : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IServiceProvider _serviceProvider = testFixture.Services;

    [Fact]
    public async Task RevokeRefreshToken_RefreshTokenNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var refreshTokenRequest = FakeRefreshTokenRequestDto.CreateValid(_fixture);
        var existingLoginEntity = FakeLoginEntity.CreateValid(_fixture) with
        {
            RefreshToken = string.Join("", refreshTokenRequest.RefreshToken.ToCharArray().Reverse()),
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsRevoked = false
        };

        await InsertLogin(existingLoginEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<RevokeRefreshToken, RefreshTokenRequestDto, ProblemDetails>(refreshTokenRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(RefreshTokenRequestDtoValidator.RefreshTokenDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task RevokeRefreshToken_RefreshTokenIsExpired_ReturnsUnauthorized()
    {
        // Arrange
        var refreshTokenRequest = FakeRefreshTokenRequestDto.CreateValid(_fixture);
        var existingLoginEntity = FakeLoginEntity.CreateValid(_fixture) with
        {
            RefreshToken = refreshTokenRequest.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(-10),
            IsRevoked = false
        };

        await InsertLogin(existingLoginEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<RevokeRefreshToken, RefreshTokenRequestDto, ProblemDetails>(refreshTokenRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(RefreshTokenRequestDtoValidator.RefreshTokenIsExpiredErrorMessage);
        await AssertRefreshTokenIsRevoked(refreshTokenRequest.RefreshToken, false);
    }

    [Fact]
    public async Task RevokeRefreshToken_RefreshTokenIsRevoked_ReturnsUnauthorized()
    {
        // Arrange
        var refreshTokenRequest = FakeRefreshTokenRequestDto.CreateValid(_fixture);
        var existingLoginEntity = FakeLoginEntity.CreateValid(_fixture) with
        {
            RefreshToken = refreshTokenRequest.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsRevoked = true
        };

        await InsertLogin(existingLoginEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<RevokeRefreshToken, RefreshTokenRequestDto, ProblemDetails>(refreshTokenRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(RefreshTokenRequestDtoValidator.RefreshTokenIsRevokedErrorMessage);
    }

    [Fact]
    public async Task RevokeRefreshToken_RefreshTokenIsValid_ReturnsNoContent()
    {
        // Arrange
        var refreshTokenRequest = FakeRefreshTokenRequestDto.CreateValid(_fixture);
        var existingLoginEntity = FakeLoginEntity.CreateValid(_fixture) with
        {
            RefreshToken = refreshTokenRequest.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsRevoked = false
        };

        await InsertLogin(existingLoginEntity);

        // Act
        var httpResponseMessage =
            await testFixture.Client.POSTAsync<RevokeRefreshToken, RefreshTokenRequestDto>(refreshTokenRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await AssertRefreshTokenIsRevoked(refreshTokenRequest.RefreshToken, true);
    }

    private async Task InsertLogin(LoginEntity existingLoginEntity)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.Logins.AddAsync(existingLoginEntity);
        await context.SaveChangesAsync();
    }

    private async Task AssertRefreshTokenIsRevoked(string refreshToken, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var entity = await context.Logins.SingleAsync(x => x.RefreshToken == refreshToken);
        entity.IsRevoked.Should().Be(expected);
    }

    protected override async Task TearDownAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }
}