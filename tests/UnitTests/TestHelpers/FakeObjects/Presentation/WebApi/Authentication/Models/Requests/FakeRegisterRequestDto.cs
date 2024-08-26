using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

public static class FakeRegisterRequestDto
{
    public static RegisterRequestDto CreateValid()
    {
        return new RegisterRequestDto
        {
            Username = FakeUsername.Valid,
            EmailAddress = FakeEmailAddress.Valid,
            Password = FakePassword.Valid,
            ConfirmPassword = FakePassword.Valid
        };
    }
}