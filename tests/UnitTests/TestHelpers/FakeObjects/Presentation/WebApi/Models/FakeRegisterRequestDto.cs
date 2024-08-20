using Presentation.WebApi.Models.Authentication;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

public static class FakeRegisterRequestDto
{
    public static RegisterRequestDto CreateValid()
    {
        return new RegisterRequestDto
        {
            Username = FakeUsername.Valid,
            EmailAddress = FakeEmailAddress.Valid,
            Password = FakePassword.Valid,
            ConfirmPassword = FakePassword.Valid,
        };
    }
}