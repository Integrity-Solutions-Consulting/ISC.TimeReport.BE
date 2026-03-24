namespace isc_tmr_backend.Features.Notifications.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Notifications.Domain;
using MediatR;


public class GetNotificationsQueryHandler(INotificationRepository repository) : IRequestHandler<GetNotificationsQuery, Result<IEnumerable<GetNotificationResponse>>>
{
    public async Task<Result<IEnumerable<GetNotificationResponse>>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
    {

        Result<IEnumerable<Notification>> result = await repository.GetAllAsync(cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        // mapeamos la entidad del Domain al DTO de respuesta
        // esto evita exponer detalles internos del Domain hacia afuera
        // equivalente a usar un ModelMapper o MapStruct en Spring
        IEnumerable<GetNotificationResponse> response = result.Value.Select(n => new GetNotificationResponse(
            n.Id,
            n.To,
            n.Subject,
            n.Channel.ToString(), // convertimos el enum a string para la respuesta
            n.CreatedAt
        ));

        return Result.Ok(response);
    }
}