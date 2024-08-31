using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Helpers;
using Core.Services;
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
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;
using Xunit;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.AuthenticatedUserTests;

[Collection("Sequential")]
public class ChangePasswordTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public ChangePasswordTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task ChangePassword_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.PATCHAsync<ChangePassword, UserProfileResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePassword_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.PATCHAsync<ChangePassword, UserProfileResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePassword_UserNoLongerExists_ReturnsUnauthorized()
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
        var (httpResponseMessage, _) = await _testFixture.Client.PATCHAsync<ChangePassword, UserProfileResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePassword_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var updateUserRequest = FakeChangePasswordRequestDto.CreateInvalid() with
        {
            CurrentPassword = FakePassword.Valid
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
        var (httpResponseMessage, validationProblemDetails) =
            await _testFixture.Client.PATCHAsync<ChangePassword, ChangePasswordRequestDto, ValidationProblemDetails>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationProblemDetails.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task ChangePassword_PasswordDidNotChange_ReturnsBadRequest()
    {
        // Arrange
        var updateUserRequest = FakeChangePasswordRequestDto.CreateValid();
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(new Password(updateUserRequest.Password), passwordSalt);

        var user = FakeUser.CreateValid(_fixture) with
        {
            PasswordSalt = passwordSalt,
            PasswordHash = new PasswordHash(passwordHash),
            IsDeleted = new IsDeleted(false)
        };

        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, problemDetails) =
            await _testFixture.Client.PATCHAsync<ChangePassword, ChangePasswordRequestDto, ProblemDetails>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetails.Detail.Should().Be(ChangePasswordRequestDtoValidator.PasswordDidNotChangeErrorMessage);
    }

    [Fact]
    public async Task ChangePassword_GivenValidRequest_ReturnsCreated()
    {
        // Arrange
        var password = string.Join("", FakePassword.Valid.ToCharArray().Reverse());
        var updateUserRequest = FakeChangePasswordRequestDto.CreateValid() with
        {
            Password = password,
            ConfirmPassword = password
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
            await _testFixture.Client.PATCHAsync<ChangePassword, ChangePasswordRequestDto, UserProfileResponseDto>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        userProfileResponse.UserId.Should().Be(user.UserId);
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