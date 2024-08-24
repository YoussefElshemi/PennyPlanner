using AutoFixture;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.User;

namespace UnitTests.Tests.Presentation.Mappers;

public class UpdateUserRequestDtoMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenResetPasswordRequestDto_ReturnsResetPasswordRequest()
    {
        // Arrange
        var userId = Fixture.Create<Guid>();
        var userUpdateRequestDto = FakeUpdateUserRequestDto.CreateValid();

        // Act
        var userManagementUserUpdateRequestDto = UpdateUserRequestDtoMapper.Map(userId, userUpdateRequestDto);

        // Assert
        userManagementUserUpdateRequestDto.UserId.Should().Be(userId);
        userManagementUserUpdateRequestDto.Username.Should().Be(userUpdateRequestDto.Username);
        userManagementUserUpdateRequestDto.EmailAddress.Should().Be(userUpdateRequestDto.EmailAddress);
    }
}