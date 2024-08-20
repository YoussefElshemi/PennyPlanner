using AutoFixture;
using Core.Extensions;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePasswordHash
{
    public static PasswordHash CreateValid(IFixture fixture)
    {
        return new PasswordHash(fixture.Create<string>().Md5Hash());
    }
}