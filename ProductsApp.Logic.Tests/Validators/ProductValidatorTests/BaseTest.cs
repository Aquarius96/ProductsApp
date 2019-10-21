using ProductsApp.Logic.Validators;

namespace ProductsApp.Logic.Tests.Validators.ProductValidatorTests
{
    public class BaseTest
    {
        protected virtual ProductValidator Create()
        {
            return new ProductValidator();
        }
    }
}