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
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
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

        await InsertUser(existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Login, LoginRequestDto, ProblemDetails>(loginRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(LoginRequestDtoValidator.IncorrectLoginDetailsErrorMessage);
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

        await InsertUser(existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Login, LoginRequestDto, ProblemDetails>(loginRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        problemDetails.Detail.Should().Be(LoginRequestDtoValidator.IncorrectLoginDetailsErrorMessage);
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

        await InsertUser(existingUserEntity);
        // Act
        var (httpResponseMessage, authenticationResponse) =
            await testFixture.Client.POSTAsync<Login, LoginRequestDto, AuthenticationResponseDto>(loginRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertLoginExists(authenticationResponse.UserId);
    }

    protected override async Task SetupAsync()
    {
        await testFixture.SeedDatabaseAsync();
    }

    protected override async Task TearDownAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }

    private async Task InsertUser(UserEntity existingUserEntity)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.Users.AddAsync(existingUserEntity);
        await context.SaveChangesAsync();
    }

    private async Task AssertLoginExists(Guid userId)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var user = await context.Logins.FirstOrDefaultAsync(x => x.UserId == userId);
        (user is not null).Should().BeTrue();
    }
}