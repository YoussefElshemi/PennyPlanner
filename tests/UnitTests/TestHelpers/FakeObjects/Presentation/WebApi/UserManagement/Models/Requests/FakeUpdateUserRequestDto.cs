using AutoFixture;
using Presentation.WebApi.UserManagement.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;

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