using FluentValidation;
using ProductsApp.Models;

namespace ProductsApp.Logic.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .Length(3, 50);

            RuleFor(p => p.Description)
                .NotEmpty()
                .Length(3, 250);

            RuleFor(p => p.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
        }
    }
}
