using FluentValidation;

namespace LaborStats.Application.Professions;

public sealed class UpdateProfessionRequestValidator : AbstractValidator<UpdateProfessionRequest>
{
    public UpdateProfessionRequestValidator()
    {
        RuleFor(x => x.KzisCode)
            .GreaterThan(0)
            .WithMessage("KzisCode must be a positive integer.")
            .When(x => x.KzisCode is not null);

        RuleFor(x => x.KzisName)
            .NotEmpty()
            .WithMessage("KzisName cannot be empty.")
            .MaximumLength(255)
            .WithMessage("KzisName cannot exceed 255 characters.")
            .When(x => x.KzisName is not null);

        RuleFor(x => x.ProfessionGroupId)
            .NotEmpty()
            .WithMessage("ProfessionGroupId cannot be empty.")
            .When(x => x.ProfessionGroupId is not null);
    }
}
