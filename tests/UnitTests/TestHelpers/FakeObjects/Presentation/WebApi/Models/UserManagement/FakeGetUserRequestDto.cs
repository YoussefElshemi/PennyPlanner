using AutoFixture;
using Presentation.WebApi.Models.UserManagement;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.UserManagement;

public static class FakeGetUserRequestDto
{
    public static UserRequestDto CreateValid(IFixture fixture)
    {
        return new UserRequestDto
        {
            UserId = FakeUserId.CreateValid(fixture)
        };
    }
}