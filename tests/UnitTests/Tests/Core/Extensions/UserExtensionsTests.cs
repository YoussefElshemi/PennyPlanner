using Core.Extensions;
using Core.Services;
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
        const string password = "password";
        var user = FakeUser.CreateValid(Fixture);
        user = user with
        {
            PasswordHash = new PasswordHash(AuthenticationService.HashPassword(password, Convert.FromBase64String(user.PasswordSalt.ToString())))
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
        const string password = "password";
        var user = FakeUser.CreateValid(Fixture);
        user = user with
        {
            PasswordHash = new PasswordHash(AuthenticationService.HashPassword(password, Convert.FromBase64String(user.PasswordSalt.ToString())))
        };

        // Act
        var authenticated = user.Authenticate("not password");

        // Assert
        authenticated.Should().BeFalse();
    }
}