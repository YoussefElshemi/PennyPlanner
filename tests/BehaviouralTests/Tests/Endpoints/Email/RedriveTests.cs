using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Enums;
using Core.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Email.Endpoints;
using Presentation.WebApi.Email.Models;
using Presentation.WebApi.Email.Validators;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Email.Models.Request;
using Xunit;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.Email;

[Collection("Sequential")]
public class RedriveTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public RedriveTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }


    [Fact]
    public async Task Redrive_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Redrive_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin
        };

        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Redrive_UserNoLongerExists_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(true)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Redrive_UserNotAdmin_ReturnsForbidden()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.User,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Redrive_EmailNotFound_ReturnsNotFound()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };

        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        var redriveEmailRequest = FakeRedriveEmailRequestDto.CreateValid(_fixture);
        var existingEmailMessageOutboxEntity = FakeEmailMessageOutboxEntity.CreateValid(_fixture) with
        {
            EmailId = new Guid(redriveEmailRequest.EmailId.ToByteArray().Reverse().ToArray())
        };

        await InsertEmailMessage(existingEmailMessageOutboxEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto, ProblemDetails>(redriveEmailRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        problemDetails.Detail.Should().Be(RedriveEmailRequestDtoValidator.EmailDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task Redrive_EmailAlreadyProcessed_ReturnsConflict()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };

        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        var redriveEmailRequest = FakeRedriveEmailRequestDto.CreateValid(_fixture);
        var existingEmailMessageOutboxEntity = FakeEmailMessageOutboxEntity.CreateValid(_fixture) with
        {
            EmailId = redriveEmailRequest.EmailId,
            IsProcessed = true
        };

        await InsertEmailMessage(existingEmailMessageOutboxEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto, ProblemDetails>(redriveEmailRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(RedriveEmailRequestDtoValidator.EmailAlreadyProcessedErrorMessage);
    }

    [Fact]
    public async Task Redrive_EmailIdIsValid_ReturnsOk()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };

        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        var redriveEmailRequest = FakeRedriveEmailRequestDto.CreateValid(_fixture);
        var existingEmailMessageOutboxEntity = FakeEmailMessageOutboxEntity.CreateValid(_fixture) with
        {
            EmailId = redriveEmailRequest.EmailId,
            IsProcessed = false
        };

        await InsertEmailMessage(existingEmailMessageOutboxEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage =
            await _testFixture.Client.POSTAsync<Redrive, RedriveEmailRequestDto>(redriveEmailRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task InsertEmailMessage(EmailMessageOutboxEntity existingEmailMessageOutboxEntity)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.Emails.AddAsync(existingEmailMessageOutboxEntity);
        await context.SaveChangesAsync();
    }

    protected override async Task TearDownAsync()
    {
        await _testFixture.ResetDatabaseAsync();
    }
}