using BookShop.Core.Models.ShopModel;
using FluentValidation;

namespace BookShop.FluentValidation
{
    public class UpdateShopBalanceModelValidator : AbstractValidator<UpdateShopBalanceModel>
    {
        public UpdateShopBalanceModelValidator()
        {
            RuleFor(t => t.ShopId)
                .NotEmpty()
                .NotNull()
                .WithMessage("ShopId must be set")
                .GreaterThan(0)
                .WithMessage("ShopId must be greater then 0");
            RuleFor(t=>t.Amount)
                .NotEmpty()
                .NotNull()
                .WithMessage("Amount must be set")
                .GreaterThan(0)
                .WithMessage("Amount must be greater then 0");
        }
    }
}
