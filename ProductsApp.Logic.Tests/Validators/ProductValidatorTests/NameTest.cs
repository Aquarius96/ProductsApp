using FluentValidation.TestHelper;
using ProductsApp.Logic.Validators;
using Xunit;

namespace ProductsApp.Logic.Tests.Validators.ProductValidatorTests
{
    public class NameTest : BaseTest
    {
        protected string Name { get; set; }

        protected override ProductValidator Create()
        {
            var validator = base.Create();
            CorrectFlow();
            return validator;
        }

        private void CorrectFlow()
        {
            Name = "Domyślna nazwa produktu";
        }

        [Fact]
        public void Return_Success()
        {
            //Arrange
            var validator = Create();
            //Assert
            validator.ShouldNotHaveValidationErrorFor(p => p.Name, Name);
        }
        
        [Fact]
        public void Return_Error_When_Name_Is_Null()
        {
            //Arrange
            var validator = Create();
            Name = null;
            //Assert
            validator.ShouldHaveValidationErrorFor(p => p.Name, Name);
        }

        [Fact]
        public void Return_Error_When_Name_Length_Is_Invalid()
        {
            //Arrange
            var validator = Create();
            Name = "aa";
            //Assert
            validator.ShouldHaveValidationErrorFor(p => p.Name, Name);
        }
    }
}