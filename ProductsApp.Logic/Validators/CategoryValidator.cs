using FluentValidation;
using ProductsApp.Models;

namespace ProductsApp.Logic.Validators
{
    public class CategoryValidator :AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .Length(3, 30);
        }
    }
}
