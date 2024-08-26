using AutoFixture;
using Presentation.WebApi.Authentication.Models.Requests;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

public static class FakeRefreshTokenRequestDto
{
    public static RefreshTokenRequestDto CreateValid(IFixture fixture)
    {
        return new RefreshTokenRequestDto
        {
            RefreshToken = fixture.Create<string>()
        };
    }
}