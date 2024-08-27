using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.AuthenticatedUser.Endpoints;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.AuthenticatedUser.Validators;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;
using Xunit;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.AuthenticatedUserTests;

[Collection("Sequential")]
public class UpdateTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public UpdateTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task Update_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.PUTAsync<Update, UserProfileResponseDto>();

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.PUTAsync<Update, UserProfileResponseDto>();

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_UserNoLongerExists_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            IsDeleted = new IsDeleted(true)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.PUTAsync<Update, UserProfileResponseDto>();

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var updateUserRequest = FakeUpdateUserRequestDto.CreateInvalid();
        var user = FakeUser.CreateValid(_fixture) with
        {
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, validationProblemDetails) =
            await _testFixture.Client.PUTAsync<Update, UpdateUserRequestDto, ValidationProblemDetails>(updateUserRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationProblemDetails.Errors.Should().HaveCount(2);
    }

    [Fact]
    public async Task Update_EmailAddressDidNotChange_ReturnsBadRequest()
    {
        // Arrange
        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = null
        };
        var user = FakeUser.CreateValid(_fixture) with
        {
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.PUTAsync<Update, UpdateUserRequestDto, ProblemDetails>(updateUserRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(UpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage(nameof(EmailAddress)));
    }

    [Fact]
    public async Task Update_UsernameDidNotChange_ReturnsBadRequest()
    {
        // Arrange
        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid() with
        {
            EmailAddress = null
        };
        var user = FakeUser.CreateValid(_fixture) with
        {
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.PUTAsync<Update, UpdateUserRequestDto, ProblemDetails>(updateUserRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(UpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage(nameof(Username)));
    }

    [Fact]
    public async Task Update_GivenValidRequest_ReturnsCreated()
    {
        // Arrange
        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = "username123",
            EmailAddress = "user@email.com"
        };
        var user = FakeUser.CreateValid(_fixture) with
        {
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, userProfileResponse) =
            await _testFixture.Client.PUTAsync<Update, UpdateUserRequestDto, UserProfileResponseDto>(updateUserRequest);

        // Arrange
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        userProfileResponse.UserId.Should().Be(user.UserId);
        userProfileResponse.Username.Should().Be(updateUserRequest.Username);
        userProfileResponse.EmailAddress.Should().Be(updateUserRequest.EmailAddress);
    }

    protected override async Task TearDownAsync()
    {
        await _testFixture.ResetDatabaseAsync();
    }
}