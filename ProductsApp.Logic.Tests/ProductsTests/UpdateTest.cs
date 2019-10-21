using System;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Products;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.ProductsTests
{
    public class UpdateTest : BaseTest
    {
        protected Product Product { get; set; }

        protected override ProductLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private async Task CorrectFlow()
        {
            Product = Builder<Product>.CreateNew().Build();
            await ProductValidator.SetValidatorSuccess();
            ValidatorFactory.Setup(v => v.Create<Product>())
                .Returns(ProductValidator.Object);
        }

        [Fact]
        public async Task Throw_ArgumentNullException_When_Product_Is_Null()
        {
            //Arrange
            var logic = Create();
            Product = null;
            //Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => logic.Update(Product));
            ValidatorFactory.Verify(v => v.Create<Product>(), Times.Never);
            Repository.Verify(r => r.SaveChanges(), Times.Never);
            ProductValidator.Verify(v => v.ValidateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Return_Errors_When_Validation_Fails()
        {
            //Arrange
            var warning = "You cannot update Product";
            var logic = Create();
            await ProductValidator.SetValidatorFailure(warning);
            //Act
            var result = await logic.Update(Product);
            //Assert
            ValidatorFactory.Verify(v => v.Create<Product>(), Times.Once);
            ProductValidator.Verify(v => v.ValidateAsync(Product, It.IsAny<CancellationToken>()), Times.Once);
            Repository.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Return_Success_When_Product_Is_Updated()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.Update(Product);
            //Assert
            ValidatorFactory.Verify(v => v.Create<Product>(), Times.Once);
            ProductValidator.Verify(v => v.ValidateAsync(Product, It.IsAny<CancellationToken>()), Times.Once);
            Repository.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}
