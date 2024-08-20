using AutoFixture;
using Core.Enums;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeAuthenticationResponse
{
    public static AuthenticationResponse CreateValid(IFixture fixture)
    {
        return new AuthenticationResponse
        {
            UserId = FakeUserId.CreateValid(fixture),
            TokenType = fixture.Create<TokenType>(),
            AccessToken = FakeAccessToken.CreateValid(fixture),
            ExpiresIn = FakeExpiresIn.CreateValid(fixture),
        };
    }
}