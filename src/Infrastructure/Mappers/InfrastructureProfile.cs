using AutoMapper;
using Core.Enums;
using Core.Models;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<LoginEntity, Login>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserEntity));
        CreateMap<Login, LoginEntity>();

        CreateMap<PasswordResetEntity, PasswordReset>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserEntity));
        CreateMap<PasswordReset, PasswordResetEntity>();

        CreateMap<UserEntity, User>()
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => (UserRole)src.UserRoleId));
        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => (int)src.UserRole));

        CreateMap<EmailMessageOutboxEntity, EmailMessage>();
        CreateMap<EmailMessage, EmailMessageOutboxEntity>();

        CreateMap<OneTimePasscode, OneTimePasscodeEntity>();
        CreateMap<OneTimePasscodeEntity, OneTimePasscode>();

    }
}