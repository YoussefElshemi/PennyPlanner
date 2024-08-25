using AutoMapper;
using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Models.Common;
using Presentation.WebApi.Models.User;

namespace Presentation.Mappers;

public class PresentationProfile : Profile
{
    public PresentationProfile()
    {
        CreateMap<LoginRequestDto, AuthenticationRequest>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => new Username(src.Username)))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new Password(src.Password)))
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom((_, _, _, context) => new IpAddress(context.Items["IpAddress"].ToString()!)));

        CreateMap<AuthenticationResponse, AuthenticationResponseDto>()
            .ForMember(dest => dest.TokenType, opt => opt.MapFrom(src => src.TokenType.ToString()));

        CreateMap<ChangePasswordRequestDto, ChangePasswordRequest>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new Password(src.Password)));

        CreateMap<RegisterRequestDto, CreateUserRequest>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => new Username(src.Username)))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new Password(src.Password)))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => new EmailAddress(src.EmailAddress)))
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom((_, _, _, context) => new IpAddress(context.Items["IpAddress"].ToString()!)));

        CreateMap<PagedRequestDto, PagedRequest>()
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber.HasValue ? new PageNumber(src.PageNumber.Value) : new PageNumber(DefaultPageNumber)))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize.HasValue ? new PageSize(src.PageSize.Value) : new PageSize(DefaultPageSize)));

        CreateMap<RefreshTokenRequestDto, RefreshTokenRequest>()
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => new RefreshToken(src.RefreshToken)))
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom((_, _, _, context) => new IpAddress(context.Items["IpAddress"].ToString()!)));

        CreateMap<RequestResetPasswordRequestDto, RequestResetPasswordRequest>()
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => new EmailAddress(src.EmailAddress)));

        CreateMap<ResetPasswordRequestDto, ResetPasswordRequest>()
            .ForMember(dest => dest.PasswordResetToken, opt => opt.MapFrom(src => new PasswordResetToken(src.PasswordResetToken)))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new Password(src.Password)));

        CreateMap<UpdateUserRequestDto, WebApi.Models.UserManagement.UpdateUserRequestDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom((_, _, _, context) => new UserId(Guid.Parse(context.Items["UserId"].ToString()!))));

        CreateMap<User, UserProfileResponseDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.ToString()))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress.ToString()))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole.ToString()));
    }

    private const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
}