using AutoFixture;
using Presentation.WebApi.Models.Authentication;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

public static class FakeResetPasswordRequestDto
{
    public static ResetPasswordRequestDto CreateValid(IFixture fixture)
    {
        return new ResetPasswordRequestDto
        {
            PasswordResetToken = fixture.Create<string>(),
            Password = FakePassword.Valid,
            ConfirmPassword = FakePassword.Valid
        };
    }
}