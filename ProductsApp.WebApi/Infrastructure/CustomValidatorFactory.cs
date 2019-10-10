using Autofac;
using FluentValidation;
using ProductsApp.Logic.Services.Interfaces;

namespace ProductsApp.WebApi.Infrastructure
{
    public class CustomValidatorFactory : ICustomValidatorFactory
    {
        private readonly IComponentContext _context;

        public CustomValidatorFactory(IComponentContext context)
        {
            _context = context;
        }
        public IValidator<T> Create<T>() where T : class
        {
            IValidator<T> validator = _context.Resolve<IValidator<T>>();
            return validator;
        }

        public IValidator<T> Create<T, TValidator>() where TValidator : class, IValidator<T>
        {
            TValidator instance = _context.Resolve<TValidator>();
            return instance;
        }
    }
}
