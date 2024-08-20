using System.IdentityModel.Tokens.Jwt;
using Core.Configs;
using Core.Enums;
using Core.Extensions;
using Core.Interfaces.Services;
using Core.Models;
using Core.Services;
using Core.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Configs;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Tests.Core.Services;

public class AuthenticationServiceTests : BaseTestClass
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);

        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockUserService = new Mock<IUserService>();
        _authenticationService = new AuthenticationService(
            _mockUserService.Object,
            mockConfig.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ValidUser_CreatesUser()
    {
        // Arrange
        var createUserRequest = FakeCreateUserRequest.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture);
        _mockUserService
            .Setup(x => x.CreateUserAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(user);
        // Act
        var authenticationResponse = await _authenticationService.CreateUserAsync(createUserRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jsonToken!.Claims.First(x => x.Type == "sub").Value.Should().Be(user.EmailAddress);
    }

    [Fact]
    public void AuthenticateAsync_UserDoesntExist_Throws()
    {
        // Arrange
        var authenticationRequest = FakeAuthenticationRequest.CreateValid();
        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(false);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authenticationService.AuthenticateAsync(authenticationRequest));
    }

    [Fact]
    public async Task AuthenticateAsync_ValidRequest_ReturnsAuthenticationResponse()
    {
        // Arrange
        var authenticationRequest = FakeAuthenticationRequest.CreateValid();
        var user = FakeUser.CreateValid(Fixture);

        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(true);
        _mockUserService
            .Setup(x => x.GetUserAsync(It.IsAny<Username>()))
            .ReturnsAsync(user);

        // Act
        var authenticationResponse = await _authenticationService.AuthenticateAsync(authenticationRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jsonToken!.Claims.First(x => x.Type == "sub").Value.Should().Be(user.EmailAddress);
    }
}