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
    private readonly Mock<ILoginService> _mockLoginService;
    private readonly Mock<IPasswordResetService> _mockPasswordResetService;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockUserService = new Mock<IUserService>();
        _mockPasswordResetService = new Mock<IPasswordResetService>();
        _mockLoginService = new Mock<ILoginService>();
        _authenticationService = new AuthenticationService(
            _mockUserService.Object,
            _mockPasswordResetService.Object,
            _mockLoginService.Object,
            MockTimeProvider.Object,
            mockConfig.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidUser_CreatesUser()
    {
        // Arrange
        var createUserRequest = FakeCreateUserRequest.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            User = user
        };
        _mockUserService
            .Setup(x => x.CreateAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(user);
        _mockLoginService
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<IpAddress>()))
            .ReturnsAsync(login);

        // Act
        var authenticationResponse = await _authenticationService.CreateAsync(createUserRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jsonToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId);
    }

    [Fact]
    public async Task AuthenticateAsync_ValidRequest_ReturnsAuthenticationResponse()
    {
        // Arrange
        var authenticationRequest = FakeAuthenticationRequest.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            User = user
        };

        _mockUserService
            .Setup(x => x.GetAsync(It.IsAny<Username>()))
            .ReturnsAsync(user);
        _mockLoginService
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<IpAddress>()))
            .ReturnsAsync(login);

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

    [Fact]
    public async Task ResetPassword_GivenResetPasswordRequest_ChangesPassword()
    {
        // Arrange
        var resetPasswordRequest = FakeResetPasswordRequest.CreateValid(Fixture);
        var passwordReset = FakePasswordReset.CreateValid(Fixture);
        _mockPasswordResetService
            .Setup(x => x.GetAsync(It.IsAny<PasswordResetToken>()))
            .ReturnsAsync(passwordReset);
        _mockPasswordResetService
            .Setup(x => x.UpdateAsync(It.IsAny<PasswordReset>()))
            .Verifiable();
        _mockUserService
            .Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<Password>()))
            .Verifiable();

        // Act
        await _authenticationService.ResetPassword(resetPasswordRequest);

        // Assert
        _mockPasswordResetService.Verify(x => x.UpdateAsync(It.IsAny<PasswordReset>()), Times.Once);
        _mockUserService.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<Password>()), Times.Once);
    }

    [Fact]
    public async Task RefreshToken_GivenRefreshTokenRequest_RefreshesToken()
    {
        // Arrange
        var authenticationRequest = FakeRefreshTokenRequest.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            User = user
        };

        _mockLoginService
            .Setup(x => x.GetAsync(It.IsAny<RefreshToken>()))
            .ReturnsAsync(login);


        // Act
        var authenticationResponse = await _authenticationService.RefreshToken(authenticationRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jsonToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId);
    }
}