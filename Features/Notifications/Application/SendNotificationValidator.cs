namespace isc_tmr_backend.Features.Notifications.Application;

using FluentValidation;
using isc_tmr_backend.Features.Notifications.Application.Commands;

public class SendNotificationValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationValidator()
    {
        RuleFor(x => x.Request.To)
            .NotEmpty().WithMessage("El destinatario es requerido.")
            .EmailAddress().WithMessage("El destinatario debe ser un email válido.");

        RuleFor(x => x.Request.Subject)
            .NotEmpty().WithMessage("El asunto es requerido.")
            .MaximumLength(200).WithMessage("El asunto no puede superar 200 caracteres.");

        RuleFor(x => x.Request.Body)
            .NotEmpty().WithMessage("El cuerpo es requerido.");
    }
}