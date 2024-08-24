using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class AuthenticationResponseMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenAuthenticationResponse_ReturnsAuthenticationResponseDto()
    {
        // Arrange
        var authenticationResponse = FakeAuthenticationResponse.CreateValid(Fixture);

        // Act
        var authenticationResponseDto = AuthenticationResponseMapper.Map(authenticationResponse);

        // Assert
        authenticationResponseDto.UserId.Should().Be(authenticationResponse.UserId.ToString());
        authenticationResponseDto.TokenType.Should().Be(authenticationResponse.TokenType.ToString());
        authenticationResponseDto.AccessToken.Should().Be(authenticationResponse.AccessToken.ToString());
        authenticationResponseDto.AccessTokenExpiresIn.Should().Be(authenticationResponse.AccessTokenExpiresIn);
        authenticationResponseDto.RefreshToken.Should().Be(authenticationResponse.RefreshToken.ToString());
        authenticationResponseDto.RefreshTokenExpiresIn.Should().Be(authenticationResponse.RefreshTokenExpiresIn);
    }
}