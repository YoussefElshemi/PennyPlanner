using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.Constants;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.UserManagement.Models.Requests;
using Presentation.WebApi.UserManagement.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.UserManagement.Endpoints;

public class GetUser(
    IUserService userService,
    IUserRepository userRepository,
    IMapper mapper) : Endpoint<UserRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiRoutes.UserManagement.GetUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Produces<UserProfileResponseDto>()
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.GetUser;
            s.ExampleRequest = ExampleRequests.UserManagement.GetUser;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(UserRequestDto userRequestDto, CancellationToken cancellationToken)
    {
        var validator = new GetUserRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(userRequestDto, cancellationToken);

        var user = await userService.GetAsync(new UserId(userRequestDto.UserId));

        var response = mapper.Map<UserProfileResponseDto>(user);

        await SendAsync(response, cancellation: cancellationToken);
    }
}