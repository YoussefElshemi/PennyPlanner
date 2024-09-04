using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Enums;
using Core.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure.Entities;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
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
public class GetUserTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public GetUserTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task GetUser_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUser_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var getUserRequest = FakeUserRequestDto.CreateValid(_fixture);
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var httpResponseMessage = await _testFixture.Client.GETAsync<GetUser, UserRequestDto>(getUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUser_UserNoLongerExists_ReturnsUnauthorized()
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
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetUser_UserNotAdmin_ReturnsForbidden()
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
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetUser, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetUser_GivenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var getUserRequest = FakeUserRequestDto.CreateValid(_fixture);
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
        var httpResponseMessage = await _testFixture.Client.GETAsync<GetUser, UserRequestDto>(getUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUser_GivenValidRequest_ReturnsOk()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);

        var userToGet = FakeUserEntity.CreateValid(_fixture) with
        {
            UserRoleId = (int)UserRole.User,
            IsDeleted = new IsDeleted(false)
        };

        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);
        await DatabaseSeeder.InsertUser(_serviceProvider, userToGet);

        var getUserRequest = FakeUserRequestDto.CreateValid(_fixture) with
        {
            UserId = userToGet.UserId
        };

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, userProfileResponse) = await _testFixture.Client.GETAsync<GetUser, UserRequestDto, UserProfileResponseDto>(getUserRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        userProfileResponse.UserId.Should().Be(userToGet.UserId);
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