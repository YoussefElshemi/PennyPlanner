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
using Presentation.WebApi.Authentication.Models.Responses;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;
using Xunit;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.AuthenticationTests;

[Collection("Sequential")]
public class TwoFactorTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public TwoFactorTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task TwoFactor_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var twoFactorRequest = FakeTwoFactorRequestDto.CreateValid(_fixture);

        // Act
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.POSTAsync<TwoFactor, TwoFactorRequestDto, ProblemDetails>(twoFactorRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        problemDetails.Detail.Should().Be(TwoFactorRequestDtoValidator.UserNotFoundErrorMessage);
    }

    [Fact]
    public async Task TwoFactor_PasscodeNotFound_ReturnsNotFound()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        var twoFactorRequest = FakeTwoFactorRequestDto.CreateValid(_fixture) with
        {
            Username = user.Username
        };

        // Act
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.POSTAsync<TwoFactor, TwoFactorRequestDto, ProblemDetails>(twoFactorRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        problemDetails.Detail.Should().Be(TwoFactorRequestDtoValidator.PasscodeNotFoundErrorMessage);
    }

    [Fact]
    public async Task TwoFactor_PasscodeUsed_ReturnsConflict()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);

        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(_fixture) with
        {
            UserId = userEntity.UserId,
            UserEntity = userEntity,
            IsUsed = true,
            ExpiresAt = TimeProvider.System.GetUtcNow().UtcDateTime.AddMinutes(10)
        };
        await InsertOneTimePasscode(oneTimePasscodeEntity);

        var twoFactorRequest = FakeTwoFactorRequestDto.CreateValid(_fixture) with
        {
            Username = user.Username,
            Passcode = oneTimePasscodeEntity.Passcode
        };

        // Act
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.POSTAsync<TwoFactor, TwoFactorRequestDto, ProblemDetails>(twoFactorRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(TwoFactorRequestDtoValidator.PasscodeAlreadyUsedErrorMessage);
    }

    [Fact]
    public async Task TwoFactor_PasscodeExpired_ReturnsGone()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);

        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(_fixture) with
        {
            UserId = userEntity.UserId,
            UserEntity = userEntity,
            IsUsed = false,
            ExpiresAt = TimeProvider.System.GetUtcNow().UtcDateTime.AddMinutes(-10),
        };
        await InsertOneTimePasscode(oneTimePasscodeEntity);

        var twoFactorRequest = FakeTwoFactorRequestDto.CreateValid(_fixture) with
        {
            Username = user.Username,
            Passcode = oneTimePasscodeEntity.Passcode
        };

        // Act
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.POSTAsync<TwoFactor, TwoFactorRequestDto, ProblemDetails>(twoFactorRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Gone);
        problemDetails.Detail.Should().Be(TwoFactorRequestDtoValidator.PasscodeExpiredErrorMessage);
    }

    [Fact]
    public async Task TwoFactor_ValidRequest_ReturnsOk()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);

        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(_fixture) with
        {
            UserId = userEntity.UserId,
            UserEntity = userEntity,
            IsUsed = false,
            ExpiresAt = TimeProvider.System.GetUtcNow().UtcDateTime.AddMinutes(10),
        };
        await InsertOneTimePasscode(oneTimePasscodeEntity);

        var twoFactorRequest = FakeTwoFactorRequestDto.CreateValid(_fixture) with
        {
            Username = user.Username,
            Passcode = oneTimePasscodeEntity.Passcode
        };

        // Act
        var (httpResponseMessage, authenticationResponse) =
            await _testFixture.Client.POSTAsync<TwoFactor, TwoFactorRequestDto, AuthenticationResponseDto>(twoFactorRequest);

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

    private async Task InsertOneTimePasscode(OneTimePasscodeEntity existingOneTimePasscodeEntity)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.OneTimePasscodes.AddAsync(existingOneTimePasscodeEntity);
        await context.SaveChangesAsync();
    }

    protected override async Task SetupAsync()
    {
        await _testFixture.ResetDatabaseAsync();
    }

    protected override async Task TearDownAsync()
    {
        await _testFixture.ResetDatabaseAsync();
    }
}