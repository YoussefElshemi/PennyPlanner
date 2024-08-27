using AutoMapper;
using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Authentication.Models.Responses;
using Presentation.WebApi.Common.Models.Requests;
using UserManagementUpdateUserRequestDto = Presentation.WebApi.UserManagement.Models.Requests.UpdateUserRequestDto;

namespace Presentation.Mappers;

public class PresentationProfile : Profile
{
    private const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;

    public PresentationProfile()
    {
        CreateMap<LoginRequestDto, AuthenticationRequest>()
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom((_, _, _, context) => new IpAddress(context.Items["IpAddress"].ToString()!)));

        CreateMap<AuthenticationResponse, AuthenticationResponseDto>();

        CreateMap<ChangePasswordRequestDto, ChangePasswordRequest>();

        CreateMap<RegisterRequestDto, CreateUserRequest>()
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom((_, _, _, context) => new IpAddress(context.Items["IpAddress"].ToString()!)));

        CreateMap<PagedRequestDto, PagedRequest>()
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber.HasValue ? new PageNumber(src.PageNumber.Value) : new PageNumber(DefaultPageNumber)))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize.HasValue ? new PageSize(src.PageSize.Value) : new PageSize(DefaultPageSize)));

        CreateMap<RefreshTokenRequestDto, RefreshTokenRequest>()
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom((_, _, _, context) => new IpAddress(context.Items["IpAddress"].ToString()!)));

        CreateMap<RequestPasswordResetRequestDto, RequestPasswordResetRequest>();

        CreateMap<ResetPasswordRequestDto, ResetPasswordRequest>();

        CreateMap<UpdateUserRequestDto, UserManagementUpdateUserRequestDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom((_, _, _, context) => new UserId(Guid.Parse(context.Items["UserId"].ToString()!))));

        CreateMap<User, UserProfileResponseDto>();
    }
}