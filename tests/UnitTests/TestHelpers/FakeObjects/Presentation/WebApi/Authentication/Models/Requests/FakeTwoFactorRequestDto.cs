using AutoFixture;
using Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

public static class FakeTwoFactorRequestDto
{
    public static TwoFactorRequestDto CreateValid(IFixture fixture)
    {
        return new TwoFactorRequestDto
        {
            Username = FakeUsername.Valid,
            Passcode = fixture.Create<string>()
        };
    }
}