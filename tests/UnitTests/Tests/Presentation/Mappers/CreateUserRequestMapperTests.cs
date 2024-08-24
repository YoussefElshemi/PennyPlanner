using AutoFixture;
using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class CreateUserRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenRegisterRequestDto_ReturnsCreateUserRequest()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid();
        var ipAddress = Fixture.Create<string>();

        // Act
        var createUserRequest = CreateUserRequestMapper.Map(registerRequestDto, ipAddress);

        // Assert
        createUserRequest.Username.Should().Be(new Username(registerRequestDto.Username));
        createUserRequest.EmailAddress.Should().Be(new EmailAddress(registerRequestDto.EmailAddress));
        createUserRequest.Password.Should().Be(new Password(registerRequestDto.Password));
        createUserRequest.IpAddress.Should().Be(new IpAddress(ipAddress));
    }
}