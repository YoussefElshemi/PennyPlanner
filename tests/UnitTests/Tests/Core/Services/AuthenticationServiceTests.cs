using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Configs;
using Core.Enums;
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

namespace UnitTests.Tests.Core.Services;

public class AuthenticationServiceTests : BaseTestClass
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IPasswordResetService> _mockPasswordResetService;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockUserService = new Mock<IUserService>();
        _mockPasswordResetService = new Mock<IPasswordResetService>();
        _authenticationService = new AuthenticationService(
            _mockUserService.Object,
            _mockPasswordResetService.Object,
            mockConfig.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidUser_CreatesUser()
    {
        // Arrange
        var createUserRequest = FakeCreateUserRequest.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture);
        _mockUserService
            .Setup(x => x.CreateAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(user);
        // Act
        var authenticationResponse = await _authenticationService.CreateAsync(createUserRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jsonToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId);
    }

    [Fact]
    public void AuthenticateAsync_UserDoesNotExist_Throws()
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
            .Setup(x => x.GetAsync(It.IsAny<Username>()))
            .ReturnsAsync(user);

        // Act
        var authenticationResponse = await _authenticationService.AuthenticateAsync(authenticationRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jsonToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId);
    }

    [Fact]
    public async Task RequestResetPassword_UserDoesNotExist_Returns()
    {
        // Arrange
        var requestResetPasswordRequest = FakeRequestResetPasswordRequest.CreateValid();

        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(false);

        // Act
        await _authenticationService.RequestResetPassword(requestResetPasswordRequest);

        // Assert
        _mockPasswordResetService.Verify(x => x.InitiateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RequestResetPassword_UserDoesExist_InitiatesPasswordReset()
    {
        // Arrange
        var requestResetPasswordRequest = FakeRequestResetPasswordRequest.CreateValid();
        var user = FakeUser.CreateValid(Fixture);

        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(true);

        _mockUserService
            .Setup(x => x.GetAsync(It.IsAny<Username>()))
            .ReturnsAsync(user);

        // Act
        await _authenticationService.RequestResetPassword(requestResetPasswordRequest);

        // Assert
        _mockPasswordResetService.Verify(x => x.InitiateAsync(It.IsAny<User>()), Times.Never);
    }
}