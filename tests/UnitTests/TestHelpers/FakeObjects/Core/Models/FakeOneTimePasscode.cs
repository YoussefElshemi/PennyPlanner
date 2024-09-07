using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeOneTimePasscode
{
    public static OneTimePasscode CreateValid(IFixture fixture)
    {
        return new OneTimePasscode
        {
            OneTimePasscodeId = FakeOneTimePasscodeId.CreateValid(fixture),
            Passcode = FakePasscode.CreateValid(fixture),
            IpAddress = FakeIpAddress.CreateValid(fixture),
            UserId = FakeUserId.CreateValid(fixture),
            IsUsed = FakeIsUsed.CreateValid(fixture),
            ExpiresAt = FakeExpiresAt.CreateValid(fixture),
            CreatedBy = FakeUsername.CreateValid(),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedBy = FakeUsername.CreateValid(),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture),
            User = FakeUser.CreateValid(fixture)
        };
    }
}