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

namespace UnitTests.Tests.Core.Services;

public class PasswordResetServiceTests : BaseTestClass
{
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IPasswordResetRepository> _mockPasswordResetRepository;
    private readonly PasswordResetService _passwordResetService;

    public PasswordResetServiceTests()
    {
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture);
        Mock<IOptions<AppConfig>> mockConfig = new();
        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _passwordResetService = new PasswordResetService(
            _mockPasswordResetRepository.Object,
            _mockEmailService.Object,
            mockConfig.Object,
            MockTimeProvider.Object);
    }

    [Fact]
    public async Task InitiateAsync_Handles()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        _mockPasswordResetRepository
            .Setup(x => x.CreateAsync(It.IsAny<PasswordReset>()))
            .Verifiable();

        _mockEmailService
            .Setup(x => x.SendEmailAsync(It.IsAny<EmailMessage>()))
            .Verifiable();

        // Act
        await _passwordResetService.InitiateAsync(user);

        // Assert
        _mockPasswordResetRepository.Verify(x => x.CreateAsync(It.IsAny<PasswordReset>()), Times.Once);
        _mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
    }
}