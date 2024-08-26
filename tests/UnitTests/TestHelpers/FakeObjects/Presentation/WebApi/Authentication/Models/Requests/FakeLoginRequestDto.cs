using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

public static class FakeLoginRequestDto
{
    public static LoginRequestDto CreateValid()
    {
        return new LoginRequestDto
        {
            Username = FakeUsername.Valid,
            Password = FakePassword.Valid
        };
    }
}