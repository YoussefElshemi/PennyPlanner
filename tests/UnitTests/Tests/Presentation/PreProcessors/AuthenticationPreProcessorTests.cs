using System.Security.Claims;
using Core.Interfaces.Repositories;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Moq;
using Presentation.PreProcessors;
using UnitTests.TestHelpers;

namespace UnitTests.Tests.Presentation.PreProcessors;

public class AuthenticationPreProcessorTests : BaseTestClass
{
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly AuthenticationPreProcessor _authenticationPreProcessor;

    public AuthenticationPreProcessorTests()
    {
        var scopeFactory = SetUpServiceScopeFactory(
            (typeof(IUserRepository), _mockUserRepository.Object)
        );

        _authenticationPreProcessor = new AuthenticationPreProcessor(scopeFactory);
    }

    [Fact]
    public async Task PreProcessAsync_UserNotLoggedIn_DoesNothing()
    {
        // Arrange
        var mockHttpClient = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var mockContext = new Mock<IPreProcessorContext>();
        mockContext
            .Setup(c => c.HttpContext)
            .Returns(mockHttpClient);

        // Act
        await _authenticationPreProcessor.PreProcessAsync(mockContext.Object, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(r => r.ExistsByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task PreProcessAsync_EndpointDoesNotRequireAuth_DoesNothing()
    {
        // Arrange
        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())], "TestAuthType");
        var mockHttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity)
        };

        var endpointMetadata = new EndpointMetadataCollection(new AllowAnonymousAttribute());
        var routeEndpoint = new RouteEndpoint(_ => Task.CompletedTask, RoutePatternFactory.Parse("/"), 0, endpointMetadata, "test");
        mockHttpContext.SetEndpoint(routeEndpoint);

        var contextMock = new Mock<IPreProcessorContext>();
        contextMock
            .Setup(c => c.HttpContext)
            .Returns(mockHttpContext);

        // Act
        await _authenticationPreProcessor.PreProcessAsync(contextMock.Object, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(r => r.ExistsByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task PreProcessAsync_UserExists_DoesNothing()
    {
        // Arrange
        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())], "TestAuthType");
        var mockHttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity)
        };

        var endpointMetadata = new EndpointMetadataCollection();
        var routeEndpoint = new RouteEndpoint(_ => Task.CompletedTask, RoutePatternFactory.Parse("/"), 0, endpointMetadata, "test");
        mockHttpContext.SetEndpoint(routeEndpoint);

        var contextMock = new Mock<IPreProcessorContext>();
        contextMock
            .Setup(c => c.HttpContext)
            .Returns(mockHttpContext);

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        await _authenticationPreProcessor.PreProcessAsync(contextMock.Object, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(r => r.ExistsByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task PreProcessAsync_UserDoesNotExist_Throws()
    {
        // Arrange
        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())], "TestAuthType");
        var mockHttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity)
        };

        var endpointMetadata = new EndpointMetadataCollection();
        var routeEndpoint = new RouteEndpoint(_ => Task.CompletedTask, RoutePatternFactory.Parse("/"), 0, endpointMetadata, "test");
        mockHttpContext.SetEndpoint(routeEndpoint);

        var contextMock = new Mock<IPreProcessorContext>();
        contextMock
            .Setup(c => c.HttpContext)
            .Returns(mockHttpContext);

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _authenticationPreProcessor.PreProcessAsync(contextMock.Object, CancellationToken.None));
        _mockUserRepository.Verify(r => r.ExistsByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}