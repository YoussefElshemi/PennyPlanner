using AutoFixture;
using Core.Enums;
using Presentation.WebApi.UserManagement.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;

public static class FakeUpdateUserRequestDto
{
    public static UserManagementUpdateUserRequestDto CreateValid(IFixture fixture)
    {
        return new UserManagementUpdateUserRequestDto
        {
            UserId = FakeUserId.CreateValid(fixture),
            EmailAddress = FakeEmailAddress.Valid,
            Username = FakeUsername.Valid,
            UserRole = fixture.Create<UserRole>().ToString()
        };
    }
}