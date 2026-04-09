namespace isc_tmr_backend.Features.Tasks.Application.Validators;

using FluentValidation;
using isc_tmr_backend.Features.Tasks.Application.Commands;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Request.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.Request.ProjectId)
            .NotEmpty().WithMessage("Project ID is required.");

        RuleFor(x => x.Request.CreatedBy)
            .NotEmpty().WithMessage("Created by user ID is required.");
    }
}
