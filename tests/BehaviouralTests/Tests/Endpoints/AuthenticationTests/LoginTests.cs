using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Helpers;
using Core.Services;
using Core.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Authentication.Endpoints;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Authentication.Models.Responses;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;
using Xunit;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.AuthenticationTests;

[Collection("Sequential")]
public class LoginTests(TestFixture testFixture) : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IServiceProvider _serviceProvider = testFixture.Services;

    [Fact]
    public async Task Login_UsernameIncorrect_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = FakeLoginRequestDto.CreateValid();
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(new Password(loginRequest.Password), passwordSalt);

        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            Username = string.Join("", loginRequest.Username.ToCharArray().Reverse()),
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash,
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Login, LoginRequestDto, ProblemDetails>(loginRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(LoginRequestDtoValidator.IncorrectLoginDetailsErrorMessage);
        await AssertLoginExists(existingUserEntity.UserId, false);
    }

    [Fact]
    public async Task Login_PasswordIncorrect_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = FakeLoginRequestDto.CreateValid();
        var password = new Password(string.Join("", loginRequest.Password.ToCharArray().Reverse()));
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(password, passwordSalt);

        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            Username = loginRequest.Username,
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash,
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Login, LoginRequestDto, ProblemDetails>(loginRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(LoginRequestDtoValidator.IncorrectLoginDetailsErrorMessage);
        await AssertLoginExists(existingUserEntity.UserId, false);
    }

    [Fact]
    public async Task Login_GivenValidRequest_ReturnsOk()
    {
        // Arrange
        var loginRequest = FakeLoginRequestDto.CreateValid();
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(new Password(loginRequest.Password), passwordSalt);

        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            Username = loginRequest.Username,
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash,
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var (httpResponseMessage, authenticationResponse) =
            await testFixture.Client.POSTAsync<Login, LoginRequestDto, AuthenticationResponseDto>(loginRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertLoginExists(authenticationResponse.UserId, true);
    }

    private async Task AssertLoginExists(Guid userId, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var exists = await context.Logins.AnyAsync(x => x.UserId == userId);
        exists.Should().Be(expected);
    }

    protected override async Task TearDownAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }
}