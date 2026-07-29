using FluentValidation;

namespace LaborStats.Application.Professions;

public sealed class CreateProfessionRequestValidator : AbstractValidator<CreateProfessionRequest>
{
    public CreateProfessionRequestValidator()
    {
        RuleFor(x => x.KzisCode)
            .GreaterThan(0)
            .WithMessage("KzisCode must be a positive integer.");

        RuleFor(x => x.KzisName)
            .NotEmpty()
            .WithMessage("KzisName is required.")
            .MaximumLength(255)
            .WithMessage("KzisName cannot exceed 255 characters.");

        RuleFor(x => x.ProfessionGroupId)
            .NotEmpty()
            .WithMessage("ProfessionGroupId is required.");
    }
}
