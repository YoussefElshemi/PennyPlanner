using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
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
public class RegisterTests(TestFixture testFixture) : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IServiceProvider _serviceProvider = testFixture.Services;

    [Fact]
    public async Task Register_GivenInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var registerRequest = FakeRegisterRequestDto.CreateInvalid();

        // Act
        var (httpResponseMessage, validationProblemDetails) =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto, ValidationProblemDetails>(registerRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationProblemDetails.Errors.Should().HaveCount(3);
        await AssertUserExists(registerRequest.Username, registerRequest.EmailAddress, false);
    }

    [Fact]
    public async Task Register_UsernameInUse_ReturnsConflict()
    {
        // Arrange
        var registerRequest = FakeRegisterRequestDto.CreateValid();
        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            Username = registerRequest.Username,
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto, ProblemDetails>(registerRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(RegisterRequestDtoValidator.UsernameTakenErrorMessage);
        await AssertUserExists(registerRequest.Username, registerRequest.EmailAddress, false);
    }

    [Fact]
    public async Task Register_EmailInUse_ReturnsConflict()
    {
        // Arrange
        var registerRequest = FakeRegisterRequestDto.CreateValid();
        var existingUserEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            EmailAddress = registerRequest.EmailAddress,
            IsDeleted = false
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto, ProblemDetails>(registerRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(RegisterRequestDtoValidator.EmailAddressInUseErrorMessage);
        await AssertUserExists(registerRequest.Username, registerRequest.EmailAddress, false);
    }

    [Fact]
    public async Task Register_GivenValidRequest_ReturnsOk()
    {
        // Arrange
        var registerRequest = FakeRegisterRequestDto.CreateValid();

        // Act
        var httpResponseMessage =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto>(registerRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertUserExists(registerRequest.Username, registerRequest.EmailAddress, true);
    }

    private async Task AssertUserExists(string username, string emailAddress, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

        var exists = await context.Users.AnyAsync(x => x.Username == username && x.EmailAddress == emailAddress);
        exists.Should().Be(expected);
    }

    protected override async Task SetupAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }

    protected override async Task TearDownAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }
}