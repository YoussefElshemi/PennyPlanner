using System.Net.Mail;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
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
    public async Task ProcessAwaitingEmailsAsync_OneAwaitingEmail_SendsEmailAndUpdatesEntity()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        _mockEmailRepository
            .Setup(x => x.GetAwaitingEmailsAsync())
            .ReturnsAsync([emailMessage]);

        _mockSmtpClient
            .Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        await _emailService.ProcessAwaitingEmailsAsync();

        // Assert
        _mockSmtpClient.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        _mockEmailRepository.Verify(x => x.UpdateAsync(It.IsAny<EmailMessage>()), Times.Once);
    }
}