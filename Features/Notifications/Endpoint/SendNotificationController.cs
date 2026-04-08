namespace isc_tmr_backend.Features.Notifications.Endpoint;

using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using isc_tmr_backend.Features.Notifications.Application.Commands;
using isc_tmr_backend.Features.Notifications.Application.Queries;
using MediatR;

public static class SendNotificationEndpoint
{
    public static RouteGroupBuilder MapNotificationEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/notifications", SendNotification)
             .WithName("SendNotification")
             .WithTags("Notifications")
             .Produces<SendNotificationResponse>(201);

        group.MapGet("/notifications", GetNotifications)
             .WithName("GetNotifications")
             .WithTags("Notifications")
             .Produces<IEnumerable<GetNotificationResponse>>(200);

        return group;
    }

    private static async Task<IResult> SendNotification(SendNotificationCommand command, IValidator<SendNotificationCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        // validamos el comando antes de enviarlo a MediatR
        // si falla retornamos un 400 con los errores de validación
        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);

        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        // enviamos el comando a MediatR
        // MediatR busca el handler registrado para SendNotificationCommand
        Result<SendNotificationResponse> result = await mediator.Send(command, cancellationToken);

        // traducimos el Result de negocio a respuesta HTTP
        // Result.IsSuccess → 201 Created
        // Result.IsFailed → 400 Bad Request con los errores
        return result.IsSuccess ? Results.Created($"/api/notifications/{result.Value.NotificationId}", result.Value) : Results.BadRequest(result.Errors.Select(e => e.Message));
    }

    private static async Task<IResult> GetNotifications(IMediator mediator, CancellationToken cancellationToken)
    {
        // los queries no necesitan validación porque no reciben input
        Result<IEnumerable<GetNotificationResponse>> result = await mediator.Send(new GetNotificationsQuery(), cancellationToken);

        // Result.IsSuccess → 200 Ok con la lista
        // Result.IsFailed → 500 con los errores
        return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(string.Join(", ", result.Errors.Select(e => e.Message)));
    }
}