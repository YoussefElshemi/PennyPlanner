using System.IdentityModel.Tokens.Jwt;
using Core.Configs;
using Core.Enums;
using Core.Extensions;
using Core.Interfaces.Services;
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
    private readonly Mock<IOptions<AppConfig>> _mockConfig;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        _mockConfig = new Mock<IOptions<AppConfig>>();
        _mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockUserService = new Mock<IUserService>();
        _authenticationService = new AuthenticationService(
            _mockUserService.Object,
            _mockConfig.Object);
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
    public void AuthenticateAsync_PasswordIncorrect_Throws()
    {
        // Arrange
        var authenticationRequest = FakeAuthenticationRequest.CreateValid();
        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash(Guid.NewGuid().ToString("N").ToUpper())
        };

        _mockUserService
            .Setup(x => x.ExistsAsync(It.IsAny<Username>()))
            .ReturnsAsync(true);
        _mockUserService
            .Setup(x => x.GetUserAsync(It.IsAny<Username>()))
            .ReturnsAsync(user);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authenticationService.AuthenticateAsync(authenticationRequest));
    }

    [Fact]
    public async Task AuthenticateAsync_PasswordCorrect_ReturnsAuthenticationResponse()
    {
        // Arrange
        var authenticationRequest = FakeAuthenticationRequest.CreateValid() with
        {
            Password = new Password(FakePassword.Valid)
        };
        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash(FakePassword.Valid.Md5Hash())
        };

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