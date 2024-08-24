using AutoFixture;
using Presentation.WebApi.Models.Authentication;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Authentication;

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