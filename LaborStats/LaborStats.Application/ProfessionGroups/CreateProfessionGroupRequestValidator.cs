using FluentValidation;

namespace LaborStats.Application.ProfessionGroups;

public sealed class CreateProfessionGroupRequestValidator : AbstractValidator<CreateProfessionGroupRequest>
{
    public CreateProfessionGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Profession group name is required.")
            .MaximumLength(255)
            .WithMessage("Profession group name cannot exceed 255 characters.");

        RuleFor(x => x.NameEn)
            .MaximumLength(255)
            .WithMessage("Profession group English name cannot exceed 255 characters.")
            .When(x => x.NameEn is not null);
    }
}
