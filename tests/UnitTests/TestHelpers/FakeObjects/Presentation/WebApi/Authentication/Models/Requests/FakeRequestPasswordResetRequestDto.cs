using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

public static class FakeRequestPasswordResetRequestDto
{
    public static RequestPasswordResetRequestDto CreateValid()
    {
        return new RequestPasswordResetRequestDto
        {
            EmailAddress = FakeEmailAddress.Valid
        };
    }
}