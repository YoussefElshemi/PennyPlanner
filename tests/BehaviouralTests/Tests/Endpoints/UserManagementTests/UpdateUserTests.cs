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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Common.Models.Responses;
using Presentation.WebApi.UserManagement.Endpoints;
using Presentation.WebApi.UserManagement.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;
using Xunit;
using IMapper = AutoMapper.IMapper;

namespace BehaviouralTests.Tests.Endpoints.UserManagementTests;

[Collection("Sequential")]
public class UpdateUserTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public UpdateUserTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task UpdateUser_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.PUTAsync<UpdateUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateUser_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid(_fixture);
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.PUTAsync<UpdateUser, UserManagementUpdateUserRequestDto>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateUser_UserNoLongerExists_ReturnsUnauthorized()
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
        var (httpResponseMessage, _) = await _testFixture.Client.PUTAsync<UpdateUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateUser_UserNotAdmin_ReturnsForbidden()
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
        var (httpResponseMessage, _) = await _testFixture.Client.PUTAsync<UpdateUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateUser_GivenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid(_fixture);
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);

        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.PUTAsync<UpdateUser, UserManagementUpdateUserRequestDto>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUser_GivenUserIsAdmin_ReturnsForbidden()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);

        var userToUpdate = FakeUserEntity.CreateValid(_fixture) with
        {
            UserRoleId = (int)UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);
        await DatabaseSeeder.InsertUser(_serviceProvider, userToUpdate);

        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid(_fixture) with
        {
            UserId = userToUpdate.UserId,
            Username = string.Join("", user.Username.ToString().ToCharArray().Reverse()),
            EmailAddress = null
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.PUTAsync<UpdateUser, UserManagementUpdateUserRequestDto>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        await AssertUserExists(updateUserRequest.Username!, updateUserRequest.EmailAddress!, false);
    }

    [Fact]
    public async Task UpdateUser_GivenUserIsCurrentUser_ReturnsOk()
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

        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid(_fixture) with
        {
            UserId = user.UserId,
            Username = string.Join("", user.Username.ToString().ToCharArray().Reverse()),
            EmailAddress = null,
            UserRole = null
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.PUTAsync<UpdateUser, UserManagementUpdateUserRequestDto>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertUserExists(updateUserRequest.Username, user.EmailAddress, true);
    }

    [Fact]
    public async Task UpdateUser_GivenValidRequest_ReturnsOk()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);

        var userToUpdate = FakeUserEntity.CreateValid(_fixture) with
        {
            UserRoleId = (int)UserRole.User,
            IsDeleted = new IsDeleted(false)
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);
        await DatabaseSeeder.InsertUser(_serviceProvider, userToUpdate);

        var updateUserRequest = FakeUpdateUserRequestDto.CreateValid(_fixture) with
        {
            UserId = userToUpdate.UserId,
            Username = string.Join("", user.Username.ToString().ToCharArray().Reverse()),
            EmailAddress = null,
            UserRole = null
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.PUTAsync<UpdateUser, UserManagementUpdateUserRequestDto>(updateUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertUserExists(updateUserRequest.Username, userToUpdate.EmailAddress, true);
    }

    private async Task AssertUserExists(string username, string emailAddress, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var exists = await context.Users.AnyAsync(x => x.Username == username && x.EmailAddress == emailAddress);
        exists.Should().Be(expected);
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