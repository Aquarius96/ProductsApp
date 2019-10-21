using ProductsApp.Logic.Validators;

namespace ProductsApp.Logic.Tests.Validators.CategoryValidatorTests
{
    public class BaseTest
    {
        protected virtual CategoryValidator Create()
        {
            return new CategoryValidator();
        }
    }
}