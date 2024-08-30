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
using Presentation.WebApi.Common.Models;
using Presentation.WebApi.UserManagement.Endpoints;
using Presentation.WebApi.UserManagement.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;
using Xunit;
using IMapper = AutoMapper.IMapper;

namespace BehaviouralTests.Tests.Endpoints.UserManagementTests;

[Collection("Sequential")]
public class DeleteUserTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public DeleteUserTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task DeleteUser_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.DELETEAsync<DeleteUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteUser_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var deleteUserRequest = FakeUserRequestDto.CreateValid(_fixture);
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.DELETEAsync<DeleteUser, UserRequestDto>(deleteUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteUser_UserNoLongerExists_ReturnsUnauthorized()
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
        var (httpResponseMessage, _) = await _testFixture.Client.DELETEAsync<DeleteUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_UserNotAdmin_ReturnsForbidden()
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
        var (httpResponseMessage, _) = await _testFixture.Client.DELETEAsync<DeleteUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_GivenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var deleteUserRequest = FakeUserRequestDto.CreateValid(_fixture);
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
        var httpResponseMessage = await _testFixture.Client.DELETEAsync<DeleteUser, UserRequestDto>(deleteUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_GivenUserIsAdmin_ReturnsForbidden()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);

        var userToDelete = FakeUserEntity.CreateValid(_fixture) with
        {
            UserRoleId = (int)UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);
        await DatabaseSeeder.InsertUser(_serviceProvider, userToDelete);

        var deleteUserRequest = FakeUserRequestDto.CreateValid(_fixture) with
        {
            UserId = userToDelete.UserId
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.DELETEAsync<DeleteUser, UserRequestDto>(deleteUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        await AssertUserDeleted(deleteUserRequest.UserId, false);
    }

    [Fact]
    public async Task DeleteUser_GivenUserIsCurrentUser_ReturnsForbidden()
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

        var deleteUserRequest = FakeUserRequestDto.CreateValid(_fixture) with
        {
            UserId = user.UserId
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.DELETEAsync<DeleteUser, UserRequestDto>(deleteUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        await AssertUserDeleted(deleteUserRequest.UserId, false);
    }

    [Fact]
    public async Task DeleteUser_GivenValidRequest_ReturnsNoContent()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);

        var userToDelete = FakeUserEntity.CreateValid(_fixture) with
        {
            UserRoleId = (int)UserRole.User,
            IsDeleted = new IsDeleted(false)
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);
        await DatabaseSeeder.InsertUser(_serviceProvider, userToDelete);

        var deleteUserRequest = FakeUserRequestDto.CreateValid(_fixture) with
        {
            UserId = userToDelete.UserId
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.DELETEAsync<DeleteUser, UserRequestDto>(deleteUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await AssertUserDeleted(deleteUserRequest.UserId, true);
    }

    private async Task AssertUserDeleted(Guid userId, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var user = await context.Users.FirstAsync(x => x.UserId == userId);
        user.IsDeleted.Should().Be(expected);
    }

    protected override async Task TearDownAsync()
    {
        await _testFixture.ResetDatabaseAsync();
    }
}