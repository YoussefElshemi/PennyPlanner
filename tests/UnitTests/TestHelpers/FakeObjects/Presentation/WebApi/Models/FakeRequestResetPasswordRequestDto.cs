using Presentation.WebApi.Models.Authentication;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

public static class FakeRequestResetPasswordRequestDto
{
    public static RequestResetPasswordRequestDto CreateValid()
    {
        return new RequestResetPasswordRequestDto
        {
            EmailAddress = FakeEmailAddress.Valid
        };
    }
}