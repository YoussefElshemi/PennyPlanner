using AutoFixture;
using AutoFixture.Kernel;

namespace BehaviouralTests.TestHelpers;

public static class AutoFixtureHelper
{
    public static Fixture Create()
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(new RandomUtcDateTimeGenerator());
        return fixture;
    }

    private class RandomUtcDateTimeGenerator : ISpecimenBuilder
    {
        private static readonly Random Random = new();

        private readonly DateTime _startDate = new DateTime(2000, 1, 1).ToUniversalTime();
        private readonly DateTime _endDate  = new DateTime(2100, 1, 1).ToUniversalTime();

        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DateTime))
            {
                var range = _endDate - _startDate;
                var randTimeSpan = new TimeSpan((long)(Random.NextDouble() * range.Ticks));

                return _startDate.Add(randTimeSpan);
            }

            return new NoSpecimen();
        }
    }
}