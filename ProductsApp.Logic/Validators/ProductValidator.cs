using FluentValidation;
using ProductsApp.Models;

namespace ProductsApp.Logic.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().Length(3, 50);
        }
    }
}
