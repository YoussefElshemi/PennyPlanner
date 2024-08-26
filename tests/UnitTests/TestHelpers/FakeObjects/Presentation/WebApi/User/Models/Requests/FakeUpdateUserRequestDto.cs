using Presentation.WebApi.AuthenticatedUser.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;

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