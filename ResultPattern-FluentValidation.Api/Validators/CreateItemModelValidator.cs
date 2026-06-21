using FluentValidation;
using ResultPattern_FluentValidation.Api.Models;

namespace ResultPattern_FluentValidation.Api.Validators;

public class CreateItemModelValidator : AbstractValidator<CreateItemModel>
{
    public CreateItemModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Qty)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
    }
}
