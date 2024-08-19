using Presentation.WebApi.Models.Authentication;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

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