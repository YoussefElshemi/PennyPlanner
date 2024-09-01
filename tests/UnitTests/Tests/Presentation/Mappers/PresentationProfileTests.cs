using AutoFixture;
using AutoMapper;
using Core.Models;
using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.Authentication.Models.Responses;
using Presentation.WebApi.Emails.Models;
using Presentation.WebApi.UserManagement.Models.Requests;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Common.Models.Requests;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;
using UpdateUserRequestDto = Presentation.WebApi.AuthenticatedUser.Models.Requests.UpdateUserRequestDto;

namespace UnitTests.Tests.Presentation.Mappers;

public class PresentationProfileTests : BaseTestClass
{
    private readonly IMapper _mapper;

    public PresentationProfileTests()
    {
        var mapperConfig = new MapperConfiguration(x => x.AddProfile<PresentationProfile>());
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public void Map_LoginRequestDto_ReturnsAuthenticationRequest()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();
        var ipAddress = Fixture.Create<string>();

        // Act
        var authenticationRequest = _mapper.Map<AuthenticationRequest>(loginRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        // Assert
        authenticationRequest.Username.Should().Be(new Username(authenticationRequest.Username.ToString()));
        authenticationRequest.Password.Should().Be(new Password(authenticationRequest.Password.ToString()));
        authenticationRequest.IpAddress.Should().Be(new IpAddress(ipAddress));
    }

    [Fact]
    public void Map_GivenAuthenticationResponse_ReturnsAuthenticationResponseDto()
    {
        // Arrange
        var authenticationResponse = FakeAuthenticationResponse.CreateValid(Fixture);

        // Act
        var authenticationResponseDto = _mapper.Map<AuthenticationResponseDto>(authenticationResponse);

        // Assert
        authenticationResponseDto.UserId.Should().Be(authenticationResponse.UserId.ToString());
        authenticationResponseDto.TokenType.Should().Be(authenticationResponse.TokenType.ToString());
        authenticationResponseDto.AccessToken.Should().Be(authenticationResponse.AccessToken.ToString());
        authenticationResponseDto.AccessTokenExpiresIn.Should().Be(authenticationResponse.AccessTokenExpiresIn);
        authenticationResponseDto.RefreshToken.Should().Be(authenticationResponse.RefreshToken.ToString());
        authenticationResponseDto.RefreshTokenExpiresIn.Should().Be(authenticationResponse.RefreshTokenExpiresIn);
    }

    [Fact]
    public void Map_GivenChangePasswordRequestDto_ReturnsChangePasswordRequest()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid();

        // Act
        var changePasswordRequest = _mapper.Map<ChangePasswordRequest>(changePasswordRequestDto);

        // Assert
        changePasswordRequest.Password.Should().Be(new Password(changePasswordRequestDto.Password));
    }

    [Fact]
    public void Map_GivenRegisterRequestDto_ReturnsCreateUserRequest()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid();
        var ipAddress = Fixture.Create<string>();

        // Act
        var createUserRequest = _mapper.Map<CreateUserRequest>(registerRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        // Assert
        createUserRequest.Username.Should().Be(new Username(registerRequestDto.Username));
        createUserRequest.EmailAddress.Should().Be(new EmailAddress(registerRequestDto.EmailAddress));
        createUserRequest.Password.Should().Be(new Password(registerRequestDto.Password));
        createUserRequest.IpAddress.Should().Be(new IpAddress(ipAddress));
    }

    [Fact]
    public void Map_GivenPagedRequestDto_ReturnsPageRequest()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture);

        // Act
        var pagedRequest = _mapper.Map<PagedRequest>(pagedRequestDto);

        // Assert
        pagedRequest.PageSize.Should().Be(new PageSize(pagedRequestDto.PageSize!.Value));
        pagedRequest.PageNumber.Should().Be(new PageNumber(pagedRequestDto.PageNumber!.Value));
    }

    [Fact]
    public void Map_GivenRefreshTokenRequestDto_ReturnsRefreshTokenRequest()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture);
        var ipAddress = Fixture.Create<string>();

        // Act
        var refreshTokenRequest = _mapper.Map<RefreshTokenRequest>(refreshTokenRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        // Assert
        refreshTokenRequest.RefreshToken.Should().Be(new RefreshToken(refreshTokenRequestDto.RefreshToken));
        refreshTokenRequest.IpAddress.Should().Be(new IpAddress(ipAddress));
    }

    [Fact]
    public void Map_GivenRequestPasswordResetRequestDto_ReturnsRequestPasswordResetRequest()
    {
        // Arrange
        var requestPasswordResetRequestDto = FakeRequestPasswordResetRequestDto.CreateValid();

        // Act
        var requestPasswordResetRequest = _mapper.Map<RequestPasswordResetRequest>(requestPasswordResetRequestDto);

        // Assert
        requestPasswordResetRequest.EmailAddress.Should().Be(new EmailAddress(requestPasswordResetRequestDto.EmailAddress));
    }

    [Fact]
    public void Map_GivenResetPasswordRequestDto_ReturnsResetPasswordRequest()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);

        // Act
        var resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(resetPasswordRequestDto);

        // Assert
        resetPasswordRequest.PasswordResetToken.Should().Be(new PasswordResetToken(resetPasswordRequestDto.PasswordResetToken));
        resetPasswordRequest.Password.Should().Be(new Password(resetPasswordRequestDto.Password));
    }

    [Fact]
    public void Map_GivenUpdateUserRequestDto_ReturnsResetPasswordRequest()
    {
        // Arrange
        var userId = Fixture.Create<Guid>();
        var userUpdateRequestDto = FakeUpdateUserRequestDto.CreateValid();

        // Act
        var userManagementUserUpdateRequestDto = _mapper.Map<UpdateUserRequestDto, UserManagementUpdateUserRequestDto>(userUpdateRequestDto, opt => { opt.Items["UserId"] = userId; });

        // Assert
        userManagementUserUpdateRequestDto.UserId.Should().Be(userId);
        userManagementUserUpdateRequestDto.Username.Should().Be(userUpdateRequestDto.Username);
        userManagementUserUpdateRequestDto.EmailAddress.Should().Be(userUpdateRequestDto.EmailAddress);
    }

    [Fact]
    public void Map_User_ReturnsUserProfileResponseDto()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        // Act
        var userProfileResponseDto = _mapper.Map<UserProfileResponseDto>(user);

        // Assert
        userProfileResponseDto.UserId.Should().Be(user.UserId);
        userProfileResponseDto.Username.Should().Be(user.Username);
        userProfileResponseDto.EmailAddress.Should().Be(user.EmailAddress);
        userProfileResponseDto.UserRole.Should().Be(user.UserRole.ToString());
        userProfileResponseDto.CreatedBy.Should().Be(user.CreatedBy.ToString());
        userProfileResponseDto.CreatedAt.Should().Be(user.CreatedAt.ToString());
        userProfileResponseDto.UpdatedBy.Should().Be(user.UpdatedBy.ToString());
        userProfileResponseDto.UpdatedAt.Should().Be(user.UpdatedAt.ToString());
    }

    [Fact]
    public void Map_EmailMessage_ReturnsEmailResponseDto()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        // Act
        var emailMessageResponseDto = _mapper.Map<EmailResponseDto>(emailMessage);

        // Assert
        emailMessageResponseDto.EmailId.Should().Be(emailMessage.EmailId.ToString());
        emailMessageResponseDto.EmailAddress.Should().Be(emailMessage.EmailAddress.ToString());
        emailMessageResponseDto.EmailSubject.Should().Be(emailMessage.EmailSubject.ToString());
        emailMessageResponseDto.EmailBody.Should().Be(emailMessage.EmailBody.ToString());
        emailMessageResponseDto.IsProcessed.Should().Be(emailMessage.IsProcessed);
        emailMessageResponseDto.CreatedBy.Should().Be(emailMessage.CreatedBy.ToString());
        emailMessageResponseDto.CreatedAt.Should().Be(emailMessage.CreatedAt.ToString());
        emailMessageResponseDto.UpdatedBy.Should().Be(emailMessage.UpdatedBy.ToString());
        emailMessageResponseDto.UpdatedAt.Should().Be(emailMessage.UpdatedAt.ToString());
    }
}