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
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Tests.Core.Services;

public class AuthenticationServiceTests : BaseTestClass
{
    private readonly AuthenticationService _authenticationService;
    private readonly Mock<ILoginService> _mockLoginService;
    private readonly Mock<IPasswordResetService> _mockPasswordResetService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IOneTimePasscodeService> _mockOneTimePasscodeService;

    public AuthenticationServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockUserService = new Mock<IUserService>();
        _mockPasswordResetService = new Mock<IPasswordResetService>();
        _mockLoginService = new Mock<ILoginService>();
        _mockOneTimePasscodeService = new Mock<IOneTimePasscodeService>();
        _authenticationService = new AuthenticationService(
            _mockUserService.Object,
            _mockPasswordResetService.Object,
            _mockOneTimePasscodeService.Object,
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

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jwtToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId.ToString());
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
        var authenticationResponse = await _authenticationService.AuthenticateAsync(user, authenticationRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jwtToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId.ToString());
    }

    [Fact]
    public async Task TwoFactorAuthenticationAsync_ValidRequest_ReturnsAuthenticationResponse()
    {
        // Arrange
        var twoFactorRequest = FakeTwoFactorRequest.CreateValid(Fixture);
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture);
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

        _mockOneTimePasscodeService
            .Setup(x => x.GetAsync(It.IsAny<UserId>(), It.IsAny<Passcode>()))
            .ReturnsAsync(oneTimePasscode);

        _mockOneTimePasscodeService
            .Setup(x => x.UpdateAsync(It.IsAny<OneTimePasscode>()))
            .Verifiable();

        // Act
        var authenticationResponse = await _authenticationService.TwoFactorAuthenticationAsync(twoFactorRequest);

        // Assert
        authenticationResponse.UserId.Should().Be(user.UserId);
        authenticationResponse.TokenType.Should().Be(TokenType.Bearer);

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jwtToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId.ToString());
    }

    [Fact]
    public async Task RequestPasswordReset_UserDoesNotExist_Returns()
    {
        // Arrange
        var requestPasswordResetRequest = FakeRequestPasswordResetRequest.CreateValid();

        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(false);

        // Act
        await _authenticationService.RequestPasswordReset(requestPasswordResetRequest);

        // Assert
        _mockPasswordResetService.Verify(x => x.InitiateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RequestPasswordReset_UserDoesExist_InitiatesPasswordReset()
    {
        // Arrange
        var requestPasswordResetRequest = FakeRequestPasswordResetRequest.CreateValid();
        var user = FakeUser.CreateValid(Fixture);

        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(true);

        _mockUserService
            .Setup(x => x.GetAsync(It.IsAny<Username>()))
            .ReturnsAsync(user);

        // Act
        await _authenticationService.RequestPasswordReset(requestPasswordResetRequest);

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

        // Act
        await _authenticationService.ResetPassword(resetPasswordRequest);

        // Assert
        _mockPasswordResetService.Verify(x => x.UpdateAsync(It.IsAny<PasswordReset>()), Times.Once);
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

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
        jwtToken!.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.Should().Be(user.UserId.ToString());
    }

    [Fact]
    public async Task RevokeToken_GivenRefreshTokenRequest_Returns()
    {
        // Arrange
        var refreshTokenRequest = FakeRefreshTokenRequest.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture);

        _mockLoginService
            .Setup(x => x.GetAsync(It.IsAny<RefreshToken>()))
            .ReturnsAsync(login);

        // Act
        await _authenticationService.RevokeToken(refreshTokenRequest);

        // Assert
        _mockLoginService.Verify(x => x.UpdateAsync(It.IsAny<Login>()), Times.Once);
    }

    [Fact]
    public void Authenticate_PasswordMatches_ReturnsTrue()
    {
        // Arrange
        var password = FakePassword.CreateValid();
        var user = FakeUser.CreateValid(Fixture);
        user = user with
        {
            PasswordHash = new PasswordHash(AuthenticationService.HashPassword(password, user.PasswordSalt))
        };

        // Act
        var authenticated = _authenticationService.Authenticate(user, password);

        // Assert
        authenticated.Should().BeTrue();
    }

    [Fact]
    public void Authenticate_PasswordDoesNotMatch_ReturnsFalse()
    {
        // Arrange
        var password = FakePassword.CreateValid();
        var user = FakeUser.CreateValid(Fixture);
        user = user with
        {
            PasswordHash = new PasswordHash(AuthenticationService.HashPassword(password, user.PasswordSalt))
        };

        // Act
        var authenticated = _authenticationService.Authenticate(user, new Password("not password"));

        // Assert
        authenticated.Should().BeFalse();
    }
}