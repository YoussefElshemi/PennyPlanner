using System.Net;
using Core.Enums;
using Core.Interfaces.Services;
using FastEndpoints;
using Presentation.Constants;
using Presentation.WebApi.Models.Common;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class GetUsersSortableFields(
    IUserService userService) : EndpointWithoutRequest<QueryFieldsDto>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiRoutes.UserManagement.GetUsersSortableFields);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Produces<QueryFieldsDto>()
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.GetUsersSortableFields;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var queryFieldsDto = new QueryFieldsDto { Fields = userService.GetSortableFields().ToArray() };

        await SendAsync(queryFieldsDto, cancellation: cancellationToken);
    }
}