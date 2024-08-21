using Presentation.WebApi.Models.User;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

public static class FakeChangePasswordRequestDto
{
    public static ChangePasswordRequestDto CreateValid()
    {
        return new ChangePasswordRequestDto
        {
            Password = FakePassword.Valid,
            ConfirmPassword = FakePassword.Valid,
        };
    }
}