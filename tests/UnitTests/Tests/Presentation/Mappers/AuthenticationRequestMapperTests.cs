using AutoFixture;
using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class AuthenticationRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_LoginRequestDto_ReturnsAuthenticationRequest()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();
        var ipAddress = Fixture.Create<string>();

        // Act
        var authenticationRequest = AuthenticationRequestMapper.Map(loginRequestDto, ipAddress);

        // Assert
        authenticationRequest.Username.Should().Be(new Username(authenticationRequest.Username.ToString()));
        authenticationRequest.Password.Should().Be(new Password(authenticationRequest.Password.ToString()));
        authenticationRequest.IpAddress.Should().Be(new IpAddress(ipAddress));
    }
}