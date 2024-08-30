using AutoFixture;
using Presentation.WebApi.Email.Models;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Email.Models.Request;

public static class FakeRedriveEmailRequestDto
{
    public static RedriveEmailRequestDto CreateValid(IFixture fixture)
    {
        return new RedriveEmailRequestDto
        {
            EmailId = fixture.Create<Guid>()
        };
    }
}