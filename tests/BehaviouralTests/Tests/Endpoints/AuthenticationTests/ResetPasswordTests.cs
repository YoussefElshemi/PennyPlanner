using System.Net;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Helpers;
using Core.Services;
using Core.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Authentication.Endpoints;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;
using Xunit;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace BehaviouralTests.Tests.Endpoints.AuthenticationTests;

[Collection("Sequential")]
public class ResetPasswordTests(TestFixture testFixture) : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IServiceProvider _serviceProvider = testFixture.Services;

    [Fact]
    public async Task ResetPassword_PasswordResetTokenDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var resetPasswordRequest = FakeResetPasswordRequestDto.CreateValid(_fixture);
        var existingPasswordResetEntity = FakePasswordResetEntity.CreateValid(_fixture);
        await InsertPasswordReset(existingPasswordResetEntity);

        // Act
        var (httpResponseMessage, problemDetail) =
            await testFixture.Client.POSTAsync<ResetPassword, ResetPasswordRequestDto, ProblemDetails>(resetPasswordRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        problemDetail.Detail.Should().Be(ResetPasswordRequestDtoValidator.PasswordResetTokenNotFoundErrorMessage);
    }

    [Fact]
    public async Task ResetPassword_PasswordResetTokenAlreadyUsed_ReturnsConflict()
    {
        // Arrange
        var resetPasswordRequest = FakeResetPasswordRequestDto.CreateValid(_fixture);
        var existingPasswordResetEntity = FakePasswordResetEntity.CreateValid(_fixture) with
        {
            ResetToken = resetPasswordRequest.PasswordResetToken,
            IsUsed = true
        };

        await InsertPasswordReset(existingPasswordResetEntity);

        // Act
        var (httpResponseMessage, problemDetail) =
            await testFixture.Client.POSTAsync<ResetPassword, ResetPasswordRequestDto, ProblemDetails>(resetPasswordRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetail.Detail.Should().Be(ResetPasswordRequestDtoValidator.PasswordResetTokenAlreadyUsedErrorMessage);
    }

    [Fact]
    public async Task ResetPassword_PasswordDidNotChange_ReturnsConflict()
    {
        // Arrange
        var resetPasswordRequest = FakeResetPasswordRequestDto.CreateValid(_fixture);
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(new Password(resetPasswordRequest.Password), passwordSalt);

        var userEntity = FakeUserEntity.CreateValid(_fixture) with
        {
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash,
            IsDeleted = false
        };

        var existingPasswordResetEntity = FakePasswordResetEntity.CreateValid(_fixture) with
        {
            UserId = userEntity.UserId,
            UserEntity = userEntity,
            ResetToken = resetPasswordRequest.PasswordResetToken,
            IsUsed = false
        };

        await InsertPasswordReset(existingPasswordResetEntity);

        // Act
        var (httpResponseMessage, problemDetail) =
            await testFixture.Client.POSTAsync<ResetPassword, ResetPasswordRequestDto, ProblemDetails>(resetPasswordRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
        problemDetail.Detail.Should().Be(ResetPasswordRequestDtoValidator.PasswordDidNotChangeErrorMessage);
        await AssertPasswordResetIsUsed(resetPasswordRequest.PasswordResetToken, false);
    }

    [Fact]
    public async Task ResetPassword_InvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var resetPasswordRequest = FakeResetPasswordRequestDto.CreateValid(_fixture) with
        {
            ConfirmPassword = string.Empty
        };

        var existingPasswordResetEntity = FakePasswordResetEntity.CreateValid(_fixture) with
        {
            ResetToken = resetPasswordRequest.PasswordResetToken,
            IsUsed = false
        };

        await InsertPasswordReset(existingPasswordResetEntity);

        // Act
        var (httpResponseMessage, validationProblemDetails) =
            await testFixture.Client.POSTAsync<ResetPassword, ResetPasswordRequestDto, ValidationProblemDetails>(resetPasswordRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationProblemDetails.Errors.Values.First().Should().Contain(ResetPasswordRequestDtoValidator.ConfirmPasswordErrorMessage);
        await AssertPasswordResetIsUsed(resetPasswordRequest.PasswordResetToken, false);
    }

    [Fact]
    public async Task ResetPassword_GivenValidRequest_ReturnsNoContent()
    {
        // Arrange
        var resetPasswordRequest = FakeResetPasswordRequestDto.CreateValid(_fixture);
        var existingPasswordResetEntity = FakePasswordResetEntity.CreateValid(_fixture) with
        {
            ResetToken = resetPasswordRequest.PasswordResetToken,
            IsUsed = false
        };

        await InsertPasswordReset(existingPasswordResetEntity);

        // Act
        var httpResponseMessage =
            await testFixture.Client.POSTAsync<ResetPassword, ResetPasswordRequestDto>(resetPasswordRequest);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await AssertPasswordResetIsUsed(resetPasswordRequest.PasswordResetToken, true);
    }

    private async Task InsertPasswordReset(PasswordResetEntity existingPasswordResetEntity)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.PasswordResets.AddAsync(existingPasswordResetEntity);
        await context.SaveChangesAsync();
    }

    private async Task AssertPasswordResetIsUsed(string resetToken, bool expected)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        var entity = await context.PasswordResets.SingleAsync(x => x.ResetToken == resetToken);
        entity.IsUsed.Should().Be(expected);
    }

    protected override async Task TearDownAsync()
    {
        await testFixture.ResetDatabaseAsync();
    }
}