using Core.Configs;
using Core.Interfaces.Repositories;
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

public class LoginServiceTests : BaseTestClass
{
    private readonly Mock<ILoginRepository> _mockLoginRepository;
    private readonly LoginService _loginService;

    public LoginServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockLoginRepository = new Mock<ILoginRepository>();
        _loginService = new LoginService(_mockLoginRepository.Object,
            mockConfig.Object,
            MockTimeProvider.Object);
    }

    [Fact]
    public async Task CreateAsync_SuccessfulCreation_ReturnsLogin()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);
        var ipAddress = FakeIpAddress.CreateValid(Fixture);

        _mockLoginRepository
            .Setup(x => x.CreateAsync(It.IsAny<Login>()))
            .Verifiable();

        // Act
        var login = await _loginService.CreateAsync(user, ipAddress);

        // Assert
        _mockLoginRepository.Verify(x => x.CreateAsync(It.IsAny<Login>()), Times.Once);
        login.UserId.Should().Be(user.UserId);
        login.IpAddress.Should().Be(ipAddress);
        login.IsRevoked.Should().Be(new IsRevoked(false));
        login.User.Should().Be(user);
    }
}