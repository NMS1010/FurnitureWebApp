using FluentValidation;

namespace FurnitureWeb.ViewModels.Catalog.Discounts
{
    public class DiscountCreateRequestValidator : AbstractValidator<DiscountCreateRequest>
    {
        public DiscountCreateRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("StartDate must less than EndDate");
        }
    }
}