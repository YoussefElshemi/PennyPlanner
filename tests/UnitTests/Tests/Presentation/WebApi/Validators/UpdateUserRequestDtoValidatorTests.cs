using Core.Interfaces.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class UpdateUserRequestDtoValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UpdateUserRequestDtoValidator _validator;

    public UpdateUserRequestDtoValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new UpdateUserRequestDtoValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Validate_EmailAddressAlreadyExists_ReturnsError()
    {
        // Act
        var loginRequestDto = FakeUpdateUserRequestDto.CreateValid();
        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Arrange
        var result = await _validator.TestValidateAsync(loginRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UpdateUserRequestDtoValidator.EmailAddressInUseErrorMessage);
    }

    [Fact]
    public async Task Validate_ValidRequest_IsValid()
    {
        // Arrange
        var loginRequestDto = FakeUpdateUserRequestDto.CreateValid();
        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        // Act
        var result = await _validator.TestValidateAsync(loginRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}