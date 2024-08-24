using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Authentication;

namespace UnitTests.Tests.Presentation.Mappers;

public class RequestResetPasswordRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenRequestResetPasswordRequestDto_ReturnsRequestResetPasswordRequest()
    {
        // Arrange
        var requestResetPasswordRequestDto = FakeRequestResetPasswordRequestDto.CreateValid();

        // Act
        var requestResetPasswordRequest = RequestResetPasswordRequestMapper.Map(requestResetPasswordRequestDto);

        // Assert
        requestResetPasswordRequest.EmailAddress.Should().Be(new EmailAddress(requestResetPasswordRequestDto.EmailAddress));
    }
}