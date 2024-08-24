using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTests.TestHelpers;

public class BaseTestClass
{
    protected readonly IFixture Fixture = new Fixture();
    protected readonly Mock<TimeProvider> MockTimeProvider = new();
    public readonly DateTime Today = new (2020, 1, 1);

    protected BaseTestClass()
    {
        MockTimeProvider.Setup(x => x.GetUtcNow()).Returns(Today);
    }

    protected static IServiceScopeFactory SetUpServiceScopeFactory(params (Type ServiceType, object Implementation)[] services)
    {
        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        var mockScope = new Mock<IServiceScope>();
        var mockServiceProvider = new Mock<IServiceProvider>();

        mockScopeFactory.Setup(f => f.CreateScope()).Returns(mockScope.Object);
        mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

        mockServiceProvider.Setup(p => p.GetService(It.IsAny<Type>())).Returns<Type>(type =>
        {
            var service = services.FirstOrDefault(s => s.ServiceType == type).Implementation;
            return service;
        });

        return mockScopeFactory.Object;
    }
}