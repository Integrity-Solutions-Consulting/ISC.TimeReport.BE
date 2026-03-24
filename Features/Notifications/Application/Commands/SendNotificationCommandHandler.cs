namespace isc_tmr_backend.Features.Notifications.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Notifications.Domain;
using isc_tmr_backend.Features.Notifications.Infrastructure;
using MediatR;

public class SendNotificationCommandHandler(INotificationRepository repository, IEmailSender emailSender, ISqsPublisher sqsPublisher, IConfiguration config ) : IRequestHandler<SendNotificationCommand, Result<SendNotificationResponse>>
{

    public async Task<Result<SendNotificationResponse>> Handle(SendNotificationCommand command, CancellationToken cancellationToken)
    {
        
        Notification notification = Notification.Create(
            command.Request.To,
            command.Request.Subject,
            command.Request.Body,
            command.Request.Channel
        );

        Result<Notification> saveResult = await repository.AddAsync(notification, cancellationToken);


        if (saveResult.IsFailed) return Result.Fail(saveResult.Errors);

        // enviamos el email si el canal lo requiere
        if (command.Request.Channel is NotificationChannel.EMAIL or NotificationChannel.BOTH) await emailSender.SendAsync(notification.To, notification.Subject, notification.Body);

        // publicamos a SQS si el canal lo requiere
        if (command.Request.Channel is NotificationChannel.SQS or NotificationChannel.BOTH) await sqsPublisher.PublishAsync(notification, config["Aws:SqsQueueUrl"] ?? "local-queue");

        // retornamos un Result.Ok con la respuesta del comando
        // el Endpoint traducirá esto a un 201 Created
        return Result.Ok(new SendNotificationResponse(notification.Id, true,"Notification sent successfully."));
    }
}