using FluentValidation;
using ResultPattern_FluentValidation.Api.Models;

namespace ResultPattern_FluentValidation.Api.Validators;

public class UpdateItemModelValidator : AbstractValidator<UpdateItemModel>
{
    public UpdateItemModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name is not null);

        RuleFor(x => x.Qty)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Qty.HasValue);

        RuleFor(x=>x.Price)
            .GreaterThanOrEqualTo(0)
            .When(x=>x.Price.HasValue);
    }
}
