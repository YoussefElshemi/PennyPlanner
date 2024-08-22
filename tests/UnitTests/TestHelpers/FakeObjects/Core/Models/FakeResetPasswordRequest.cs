using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeResetPasswordRequest
{
    public static ResetPasswordRequest CreateValid(IFixture fixture)
    {
        return new ResetPasswordRequest
        {
            PasswordResetToken = FakePasswordResetToken.CreateValid(fixture),
            Password = FakePassword.CreateValid()
        };
    }
}