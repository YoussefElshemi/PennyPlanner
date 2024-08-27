using AutoMapper;
using Infrastructure.Mappers;
using Presentation.Mappers;

namespace BehaviouralTests.TestHelpers;

public static class AutoMapperHelper
{
    public static IMapper Create()
    {
        var mapperConfig = new MapperConfiguration(x =>
        {
            x.AddProfile<InfrastructureProfile>();
            x.AddProfile<PresentationProfile>();
        });

        return mapperConfig.CreateMapper();
    }
}