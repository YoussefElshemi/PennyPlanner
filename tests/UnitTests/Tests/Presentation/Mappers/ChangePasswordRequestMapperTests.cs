using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.User;

namespace UnitTests.Tests.Presentation.Mappers;

public class ChangePasswordRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenChangePasswordRequestDto_ReturnsChangePasswordRequest()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid();

        // Act
        var changePasswordRequest = ChangePasswordRequestMapper.Map(changePasswordRequestDto);

        // Assert
        changePasswordRequest.Password.Should().Be(new Password(changePasswordRequestDto.Password));
    }
}