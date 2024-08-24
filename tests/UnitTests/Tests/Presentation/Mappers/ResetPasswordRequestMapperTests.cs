using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Authentication;

namespace UnitTests.Tests.Presentation.Mappers;

public class ResetPasswordRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenResetPasswordRequestDto_ReturnsResetPasswordRequest()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);

        // Act
        var resetPasswordRequest = ResetPasswordRequestMapper.Map(resetPasswordRequestDto);

        // Assert
        resetPasswordRequest.PasswordResetToken.Should().Be(new PasswordResetToken(resetPasswordRequestDto.PasswordResetToken));
        resetPasswordRequest.Password.Should().Be(new Password(resetPasswordRequestDto.Password));
    }
}