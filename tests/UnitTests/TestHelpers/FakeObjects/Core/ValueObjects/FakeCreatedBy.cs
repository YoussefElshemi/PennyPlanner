using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeCreatedBy
{
    public static CreatedBy CreateValid(IFixture fixture)
    {
        return new CreatedBy(fixture.Create<Guid>());
    }
}