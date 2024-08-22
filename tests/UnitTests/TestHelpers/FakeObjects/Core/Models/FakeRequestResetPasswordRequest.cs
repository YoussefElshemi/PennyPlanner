using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeRequestResetPasswordRequest
{
    public static RequestResetPasswordRequest CreateValid()
    {
        return new RequestResetPasswordRequest
        {
            EmailAddress = FakeEmailAddress.CreateValid()
        };
    }
}