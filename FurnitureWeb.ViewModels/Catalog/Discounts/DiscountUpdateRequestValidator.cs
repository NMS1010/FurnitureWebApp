using FluentValidation;

namespace FurnitureWeb.ViewModels.Catalog.Discounts
{
    public class DiscountUpdateRequestValidator : AbstractValidator<DiscountUpdateRequest>
    {
        public DiscountUpdateRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("StartDate must less than EndDate");
        }
    }
}