using AutoFixture;
using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Authentication;

namespace UnitTests.Tests.Presentation.Mappers;

public class RefreshTokenRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenRefreshTokenRequestDto_ReturnsRefreshTokenRequest()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture);
        var ipAddress = Fixture.Create<string>();

        // Act
        var refreshTokenRequest = RefreshTokenRequestMapper.Map(refreshTokenRequestDto, ipAddress);

        // Assert
        refreshTokenRequest.RefreshToken.Should().Be(new RefreshToken(refreshTokenRequestDto.RefreshToken));
        refreshTokenRequest.IpAddress.Should().Be(new IpAddress(ipAddress));
    }
}