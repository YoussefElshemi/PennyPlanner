using Core.Extensions;
using Core.ValueObjects;
using FluentAssertions;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Core.Extensions;

public class UserExtensionsTests : BaseTestClass
{
    [Fact]
    public void Authenticate_PasswordMatches_ReturnsTrue()
    {
        // Arrange
        var password = "password";
        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash(password.Md5Hash())
        };

        // Act
        var authenticated = user.Authenticate(password);

        // Assert
        authenticated.Should().BeTrue();
    }

    [Fact]
    public void Authenticate_PasswordDoesNotMatch_ReturnsFalse()
    {
        // Arrange
        var password = "password";
        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash(password.Md5Hash())
        };

        // Act
        var authenticated = user.Authenticate("not password");

        // Assert
        authenticated.Should().BeFalse();
    }
}