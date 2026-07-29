using FluentValidation;

namespace LaborStats.Application.ProfessionGroups;

public sealed class UpdateProfessionGroupRequestValidator : AbstractValidator<UpdateProfessionGroupRequest>
{
    public UpdateProfessionGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Profession group name cannot be empty.")
            .MaximumLength(255)
            .WithMessage("Profession group name cannot exceed 255 characters.")
            .When(x => x.Name is not null);

        RuleFor(x => x.NameEn)
            .MaximumLength(255)
            .WithMessage("Profession group English name cannot exceed 255 characters.")
            .When(x => x.NameEn is not null);
    }
}
