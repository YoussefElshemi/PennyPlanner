using AutoFixture;
using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

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