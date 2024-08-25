using Core.Validators;
using FluentAssertions;

namespace UnitTests.Tests.Core.Validators;

public class EmailAddressValidatorTests
{
    private readonly EmailAddressValidator _validator = new();

    [Theory]
    [InlineData("valid@mail.com", true)]
    [InlineData("valid.valid@mail.com", true)]
    [InlineData("invalid@ma!l.com", false)]
    [InlineData("invalid..invalid@mail.com", false)]
    [InlineData("invalid@", false)]
    [InlineData("@invalid.com", false)]
    [InlineData("", false)]
    public void Validate_GivenActual_ReturnsExpectedResult(string actual, bool expected)
    {
        _validator.Validate(actual).IsValid.Should().Be(expected);
    }
}