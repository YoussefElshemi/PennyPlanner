using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class UserProfileResponseMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenAuthenticationResponse_ReturnsAuthenticationResponseDto()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        // Act
        var userProfileResponse = UserProfileResponseMapper.Map(user);

        // Assert
        userProfileResponse.Username.Should().Be(user.Username);
        userProfileResponse.EmailAddress.Should().Be(user.EmailAddress);
        userProfileResponse.UserRole.Should().Be(user.UserRole.ToString());
    }
}