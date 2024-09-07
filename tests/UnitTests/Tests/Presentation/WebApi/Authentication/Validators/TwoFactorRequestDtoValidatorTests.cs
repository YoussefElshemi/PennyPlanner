using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.Authentication.Validators;

public class TwoFactorRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IOneTimePasscodeRepository> _mockOneTimePasscodeRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly TwoFactorRequestDtoValidator _validator;

    public TwoFactorRequestDtoValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockOneTimePasscodeRepository = new Mock<IOneTimePasscodeRepository>();
        _validator = new TwoFactorRequestDtoValidator(_mockOneTimePasscodeRepository.Object,
            _mockUserRepository.Object, MockTimeProvider.Object);
    }

    [Fact]
    public async Task ValidateAsync_UsernameIsEmpty_ReturnsError()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture) with
        {
            Username = string.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_PasscodeIsEmpty_ReturnsError()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture) with
        {
            Passcode = string.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_UserNotExist_ReturnsError()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture);

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(TwoFactorRequestDtoValidator.UserNotFoundErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasscodeDoesNotExist_ReturnsError()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture);

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockOneTimePasscodeRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(TwoFactorRequestDtoValidator.PasscodeNotFoundErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasscodeAlreadyUsed_ReturnsError()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture);
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture) with
        {
            IsUsed = new IsUsed(true)
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockOneTimePasscodeRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(oneTimePasscode.User);

        _mockOneTimePasscodeRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(oneTimePasscode);

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(TwoFactorRequestDtoValidator.PasscodeAlreadyUsedErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasscodeExpired_ReturnsError()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture);
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture) with
        {
            IsUsed = new IsUsed(false),
            ExpiresAt = new ExpiresAt(new DateTime(2000, 1, 1))
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockOneTimePasscodeRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(oneTimePasscode.User);

        _mockOneTimePasscodeRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(oneTimePasscode);

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(TwoFactorRequestDtoValidator.PasscodeExpiredErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var twoFactorRequestDto = FakeTwoFactorRequestDto.CreateValid(Fixture);
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture) with
        {
            IsUsed = new IsUsed(false)
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockOneTimePasscodeRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(oneTimePasscode.User);

        _mockOneTimePasscodeRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(oneTimePasscode);

        // Act
        var result = await _validator.TestValidateAsync(twoFactorRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}