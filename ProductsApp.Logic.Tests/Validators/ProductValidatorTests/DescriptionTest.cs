using FluentValidation.TestHelper;
using ProductsApp.Logic.Validators;
using Xunit;

namespace ProductsApp.Logic.Tests.Validators.ProductValidatorTests
{
    public class DescriptionTest : BaseTest
    {
        protected string Description { get; set; }

        protected override ProductValidator Create()
        {
            var validator = base.Create();
            CorrectFlow();
            return validator;
        }

        private void CorrectFlow()
        {
            Description = "Domyślny opis produktu";
        }

        [Fact]
        public void Return_Success()
        {
            //Arrange
            var validator = Create();
            //Assert
            validator.ShouldNotHaveValidationErrorFor(p => p.Description, Description);
        }

        [Fact]
        public void Return_Error_When_Description_Is_Null()
        {
            //Arrange
            var validator = Create();
            Description = null;
            //Assert
            validator.ShouldHaveValidationErrorFor(p => p.Description, Description);
        }

        [Fact]
        public void Return_Error_When_Description_Length_Is_Invalid()
        {
            //Arrange
            var validator = Create();
            Description = "aa";
            //Assert
            validator.ShouldHaveValidationErrorFor(p => p.Description, Description);
        }
    }
}