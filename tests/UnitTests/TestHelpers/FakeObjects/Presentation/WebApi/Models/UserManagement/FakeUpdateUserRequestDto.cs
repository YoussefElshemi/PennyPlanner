using AutoFixture;
using Presentation.WebApi.Models.UserManagement;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.UserManagement;

public static class FakeUpdateUserRequestDto
{
    public static UpdateUserRequestDto CreateValid(IFixture fixture)
    {
        return new UpdateUserRequestDto
        {
            UserId = FakeUserId.CreateValid(fixture),
            EmailAddress = FakeEmailAddress.Valid,
            Username = FakeUsername.Valid
        };
    }
}