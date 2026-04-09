namespace isc_tmr_backend.Features.Projects.Application.Validators;

using FluentValidation;
using isc_tmr_backend.Features.Projects.Application.Commands;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.Request.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required.");
    }
}
