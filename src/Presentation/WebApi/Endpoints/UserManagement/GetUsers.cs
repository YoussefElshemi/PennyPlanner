using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Common;
using Presentation.WebApi.Models.Common.Validators;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class GetUsers(IUserService userService,
    IUserRepository userRepository) : Endpoint<PagedRequestDto, PagedResponseDto<UserProfileResponseDto>>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiUrls.UserManagement.GetUsers);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();
    }

    public override async Task HandleAsync(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        var validator = new PagedRequestDtoValidator<Core.Models.User>(userRepository);
        await validator.ValidateAndThrowAsync(pagedRequestDto, cancellationToken);

        var pagedRequest = PagedRequestMapper.Map(pagedRequestDto);

        var pagedResponse = await userService.GetAllAsync(pagedRequest);

        var pagedResponseDto = PagedResponseMapper.Map(pagedResponse, UserProfileResponseMapper.Map);

        await SendAsync(pagedResponseDto, cancellation: cancellationToken);
    }
}