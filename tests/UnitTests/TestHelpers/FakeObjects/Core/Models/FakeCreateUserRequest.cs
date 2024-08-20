using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeCreateUserRequest
{
    public static CreateUserRequest CreateValid(IFixture fixture)
    {
        return new CreateUserRequest
        {
            Username = FakeUsername.CreateValid(),
            Password = FakePassword.CreateValid(),
            EmailAddress = FakeEmailAddress.CreateValid()
        };
    }
}