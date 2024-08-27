using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
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

        await InsertUser(existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto, ProblemDetails>(registerRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(RegisterRequestDtoValidator.UsernameTakenErrorMessage);
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

        await InsertUser(existingUserEntity);

        // Act
        var (httpResponseMessage, problemDetails) =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto, ProblemDetails>(registerRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(RegisterRequestDtoValidator.EmailAddressInUseErrorMessage);
    }

    [Fact]
    public async Task Register_GivenValidRequest_ReturnsOk()
    {
        // Arrange
        var registerRequest = FakeRegisterRequestDto.CreateValid();

        // Act
        var (httpResponseMessage, authenticationResponse) =
            await testFixture.Client.POSTAsync<Register, RegisterRequestDto, AuthenticationResponseDto>(registerRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertUserExists(authenticationResponse.UserId);
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

    private async Task AssertUserExists(Guid userId)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var user = await context.Users.FindAsync(userId);
        (user is not null).Should().BeTrue();
    }
}