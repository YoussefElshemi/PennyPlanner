using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Models.User;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

public static class FakeUpdateUserRequestDto
{
    public static UpdateUserRequestDto CreateValid()
    {
        return new UpdateUserRequestDto
        {
            EmailAddress = FakeEmailAddress.Valid
        };
    }
}