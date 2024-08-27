using Presentation.WebApi.AuthenticatedUser.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;

public static class FakeChangePasswordRequestDto
{
    public static ChangePasswordRequestDto CreateValid()
    {
        return new ChangePasswordRequestDto
        {
            Password = FakePassword.Valid,
            ConfirmPassword = FakePassword.Valid
        };
    }

    public static ChangePasswordRequestDto CreateInvalid()
    {
        return new ChangePasswordRequestDto
        {
            Password = FakePassword.Invalid,
            ConfirmPassword = FakePassword.Invalid
        };
    }
}