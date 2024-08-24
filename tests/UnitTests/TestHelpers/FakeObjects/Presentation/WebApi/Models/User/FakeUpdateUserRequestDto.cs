using Presentation.WebApi.Models.User;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.User;

public static class FakeUpdateUserRequestDto
{
    public static UpdateUserRequestDto CreateValid()
    {
        return new UpdateUserRequestDto
        {
            EmailAddress = FakeEmailAddress.Valid,
            Username = FakeUsername.Valid
        };
    }
}