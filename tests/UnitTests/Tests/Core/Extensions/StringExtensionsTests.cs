using Core.Extensions;
using FluentAssertions;

namespace UnitTests.Tests.Core.Extensions;

public class StringExtensionTests
{
    [Theory]
    [InlineData(" ", "7215EE9C7D9DC229D2921A40E899EC5F")]
    [InlineData("hello world", "5EB63BBBE01EEED093CB22BB8F5ACDC3")]
    public void Md5Hash_InputString_ReturnsMd5(string input, string expected)
    {
        input.Md5Hash().Should().Be(expected);
    }
}