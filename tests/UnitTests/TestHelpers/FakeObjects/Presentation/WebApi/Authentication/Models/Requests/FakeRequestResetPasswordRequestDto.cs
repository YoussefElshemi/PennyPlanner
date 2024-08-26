using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

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