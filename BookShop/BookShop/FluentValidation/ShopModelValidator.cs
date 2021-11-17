using BookShop.Core.Models.ShopModel;
using FluentValidation;

namespace BookShop.FluentValidation
{
    public class ShopModelValidator : AbstractValidator<ShopModel>
    {
        public ShopModelValidator()
        {
            RuleFor(t => t.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name must be set");
            RuleFor(t => t.MaxBookQuantity)
                .NotEmpty()
                .NotNull()
                .WithMessage("MaxBookQuantity must be set")
                .GreaterThan(0)
                .WithMessage("MaxBookQuantity must be greater then 0");
            RuleFor(t => t.Balance)
                .NotNull()
                .NotEmpty()
                .WithMessage("Balance must be set")
                .GreaterThan(0)
                .WithMessage("Balance must be greater then 0");
        }
    }
}
