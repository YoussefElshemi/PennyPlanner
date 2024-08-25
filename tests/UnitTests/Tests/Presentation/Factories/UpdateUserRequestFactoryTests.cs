using Core.ValueObjects;
using FluentAssertions;
using Presentation.Factories;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.User;

namespace UnitTests.Tests.Presentation.Factories;

public class UpdateUserRequestFactoryTests : BaseTestClass
{
    [Fact]
    public void Map_GivenUserAndUpdateUserRequestDto_ReturnsUpdatedUser()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid();

        // Act
        var createUserRequest = UpdateUserRequestFactory.Map(user, updateUserRequestDto);

        // Assert
        createUserRequest.UserId.Should().Be(new UserId(user.UserId));
        createUserRequest.EmailAddress.Should().Be(new EmailAddress(createUserRequest.EmailAddress));
        createUserRequest.Username.Should().Be(new Username(createUserRequest.Username));
    }
}