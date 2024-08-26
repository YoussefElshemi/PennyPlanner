using Core.ValueObjects;
using FluentAssertions;
using Presentation.Factories;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;

namespace UnitTests.Tests.Presentation.Factories;

public class UpdateUserRequestFactoryTests : BaseTestClass
{
    [Fact]
    public void Create_GivenUserAndUpdateUserRequestDto_ReturnsUpdatedUser()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid();

        // Act
        var createUserRequest = UpdateUserRequestFactory.Create(user, updateUserRequestDto);

        // Assert
        createUserRequest.UserId.Should().Be(new UserId(user.UserId));
        createUserRequest.EmailAddress.Should().Be(new EmailAddress(createUserRequest.EmailAddress));
        createUserRequest.Username.Should().Be(new Username(createUserRequest.Username));
    }
}