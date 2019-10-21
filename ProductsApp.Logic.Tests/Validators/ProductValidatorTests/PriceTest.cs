using FluentValidation.TestHelper;
using ProductsApp.Logic.Validators;
using Xunit;

namespace ProductsApp.Logic.Tests.Validators.ProductValidatorTests
{
    public class PriceTest : BaseTest
    {
        protected decimal Price { get; set; }

        protected override ProductValidator Create()
        {
            var validator = base.Create();
            CorrectFlow();
            return validator;
        }

        private void CorrectFlow()
        {
            Price = 100;
        }

        [Fact]
        public void Return_Success()
        {
            //Arrange
            var validator = Create();
            //Assert
            validator.ShouldNotHaveValidationErrorFor(p => p.Price, Price);
        }

        [Fact]
        public void Return_Error_When_Price_Has_Negative_Value()
        {
            //Arrange
            var validator = Create();
            Price = -10;
            //Assert
            validator.ShouldHaveValidationErrorFor(p => p.Price, Price);
        }
    }
}