using AutoFixture;

namespace UnitTests.TestHelpers;

public class BaseTestClass
{
    public readonly IFixture Fixture = new Fixture();
}