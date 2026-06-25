using FluentValidation;
using ResultPattern_FluentValidation.Services.Models.Request;

namespace ResultPattern_FluentValidation.Api.Validators;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Qty)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
    }
}
