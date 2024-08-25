using Presentation.WebApi.Models.AuthenticatedUser;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.User;

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
}