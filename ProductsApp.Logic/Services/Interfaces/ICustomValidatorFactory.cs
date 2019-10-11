using FluentValidation;

namespace ProductsApp.Logic.Services.Interfaces
{
    public interface ICustomValidatorFactory : IService
    {        
        IValidator<T> Create<T>() where T : class;
    }
}
