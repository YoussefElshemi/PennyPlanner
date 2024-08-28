using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakePasswordReset
{
    public static PasswordReset CreateValid(IFixture fixture)
    {
        return new PasswordReset
        {
            PasswordResetId = FakePasswordResetId.CreateValid(fixture),
            UserId = FakeUserId.CreateValid(fixture),
            User = FakeUser.CreateValid(fixture),
            ResetToken = FakePasswordResetToken.CreateValid(fixture),
            ExpiresAt = FakeExpiresAt.CreateValid(fixture),
            IsUsed = FakeIsUsed.CreateValid(fixture),
            CreatedBy = FakeUsername.CreateValid(),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedBy = FakeUsername.CreateValid(),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}