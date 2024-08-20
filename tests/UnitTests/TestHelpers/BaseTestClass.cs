using AutoFixture;
using Moq;

namespace UnitTests.TestHelpers;

public class BaseTestClass
{
    protected readonly IFixture Fixture = new Fixture();
    protected readonly Mock<TimeProvider> MockTimeProvider = new();
    private readonly DateTime Today = new (2020, 1, 1);

    protected BaseTestClass()
    {
        MockTimeProvider.Setup(x => x.GetUtcNow()).Returns(Today);
    }
}