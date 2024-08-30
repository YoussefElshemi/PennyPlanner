using System.Net.Mail;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Configs;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Infrastructure.Services;

public class EmailServiceTests : BaseTestClass
{
    private readonly Mock<ISmtpClient> _mockSmtpClient;
    private readonly Mock<IEmailRepository> _mockEmailRepository;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        Mock<IOptions<AppConfig>> mockConfig = new();
        var fakeAppConfig = FakeAppConfig.CreateValid(Fixture) with
        {
            SmtpConfig = FakeSmtpConfig.CreateValid(Fixture) with
            {
                NumberOfRetries = 1
            }
        };

        mockConfig.SetupGet(x => x.Value).Returns(fakeAppConfig);

        _mockSmtpClient = new Mock<ISmtpClient>();
        _mockEmailRepository = new Mock<IEmailRepository>();

        _emailService = new EmailService(_mockSmtpClient.Object,
            _mockEmailRepository.Object,
            MockTimeProvider.Object,
            mockConfig.Object);
    }

    [Fact]
    public async Task SendEmailAsync_SuccessfulSend_UpdatesEmailMessage()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        _mockEmailRepository
            .Setup(x => x.CreateAsync(It.IsAny<EmailMessage>()))
            .Returns(Task.CompletedTask);

        _mockEmailRepository
            .Setup(x => x.UpdateAsync(It.IsAny<EmailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        await _emailService.SendEmailAsync(emailMessage);

        // Assert
        _mockEmailRepository.Verify(x => x.CreateAsync(It.IsAny<EmailMessage>()), Times.Once);
        _mockEmailRepository.Verify(x => x.UpdateAsync(It.IsAny<EmailMessage>()), Times.Once);
    }

    [Fact]
    public async Task SendEmailAsync_FailedSend_DoesNotUpdateEmailMessage()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        _mockEmailRepository
            .Setup(x => x.CreateAsync(It.IsAny<EmailMessage>()))
            .Returns(Task.CompletedTask);

        _mockEmailRepository
            .Setup(x => x.UpdateAsync(It.IsAny<EmailMessage>()))
            .Returns(Task.CompletedTask);

        _mockSmtpClient
            .Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
            .ThrowsAsync(new Exception());

        // Act
        await _emailService.SendEmailAsync(emailMessage);

        // Assert
        _mockEmailRepository.Verify(x => x.CreateAsync(It.IsAny<EmailMessage>()), Times.Once);
        _mockEmailRepository.Verify(x => x.UpdateAsync(It.IsAny<EmailMessage>()), Times.Once);
    }

    [Fact]
    public async Task RedriveEmailAsync_SuccessfulSend_UpdatesEmailMessage()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        _mockEmailRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(emailMessage);

        _mockEmailRepository
            .Setup(x => x.UpdateAsync(It.IsAny<EmailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _emailService.RedriveEmailAsync(emailMessage.EmailId);

        // Assert
        _mockEmailRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        _mockEmailRepository.Verify(x => x.UpdateAsync(It.IsAny<EmailMessage>()), Times.Once);

        result.IsProcessed.Should().Be(new IsProcessed(true));
    }

    [Fact]
    public async Task RedriveEmailAsync_FailedSend_DoesNotUpdateEmailMessage()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        _mockEmailRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(emailMessage);

        _mockEmailRepository
            .Setup(x => x.UpdateAsync(It.IsAny<EmailMessage>()))
            .Returns(Task.CompletedTask);

        _mockSmtpClient
            .Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _emailService.RedriveEmailAsync(emailMessage.EmailId);

        // Assert
        _mockEmailRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        _mockEmailRepository.Verify(x => x.UpdateAsync(It.IsAny<EmailMessage>()), Times.Once);

        result.IsProcessed.Should().Be(new IsProcessed(false));
    }
}