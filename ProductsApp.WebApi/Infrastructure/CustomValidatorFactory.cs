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
    }
}
