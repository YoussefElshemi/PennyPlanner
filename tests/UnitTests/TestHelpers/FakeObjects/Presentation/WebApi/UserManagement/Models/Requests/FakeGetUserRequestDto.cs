using AutoFixture;
using Presentation.WebApi.UserManagement.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;

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