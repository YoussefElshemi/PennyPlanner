using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeRequestPasswordResetRequest
{
    public static RequestPasswordResetRequest CreateValid()
    {
        return new RequestPasswordResetRequest
        {
            EmailAddress = FakeEmailAddress.CreateValid()
        };
    }
}