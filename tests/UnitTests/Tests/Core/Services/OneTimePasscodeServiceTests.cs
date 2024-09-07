using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Configs;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Tests.Core.Services;

public class OneTimePasscodeServiceTests : BaseTestClass
{
    private readonly OneTimePasscodeService _oneTimePasscodeService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IOneTimePasscodeRepository> _mockOneTimePasscodeRepository;

    public OneTimePasscodeServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockEmailService = new Mock<IEmailService>();
        _mockOneTimePasscodeRepository = new Mock<IOneTimePasscodeRepository>();

        _oneTimePasscodeService = new OneTimePasscodeService(
            _mockEmailService.Object,
            _mockOneTimePasscodeRepository.Object,
            mockConfig.Object,
            MockTimeProvider.Object);
    }

    [Fact]
    public async Task InitiateAsync_ValidRequest_CreateOneTimePasscode()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);
        var ipAddress = FakeIpAddress.CreateValid(Fixture);

        _mockOneTimePasscodeRepository
            .Setup(x => x.CreateAsync(It.IsAny<OneTimePasscode>()))
            .Verifiable();

        _mockEmailService
            .Setup(x => x.CreateAsync(It.IsAny<EmailMessage>()))
            .Verifiable();

        // Act
        await _oneTimePasscodeService.InitiateAsync(user, ipAddress);

        // Assert
        _mockOneTimePasscodeRepository.Verify(x => x.CreateAsync(It.IsAny<OneTimePasscode>()), Times.Once);
        _mockEmailService.Verify(x => x.CreateAsync(It.IsAny<EmailMessage>()), Times.Once);
    }
}