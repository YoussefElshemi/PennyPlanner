using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class AuthenticationRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenAuthenticationResponse_ReturnsAuthenticationResponseDto()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();

        // Act
        var authenticationRequest = AuthenticationRequestMapper.Map(loginRequestDto);

        // Assert
        authenticationRequest.Username.Should().Be(new Username(authenticationRequest.Username.ToString()));
        authenticationRequest.Password.Should().Be(new Password(authenticationRequest.Password.ToString()));
    }
}