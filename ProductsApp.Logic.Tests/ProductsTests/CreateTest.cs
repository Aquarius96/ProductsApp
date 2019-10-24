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
    public class CreateTest : BaseTest
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
            Product = Builder<Product>
                .CreateNew()
                .Build();
            await ProductValidator.SetValidatorSuccess();
        }

        [Fact]
        public async Task Return_Success_When_Product_Is_Added()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.Create(Product);
            //Assert
            result.Should()
                .BeSuccess(Product);
            ProductValidator.Verify(v => v.ValidateAsync(Product, It.IsAny<CancellationToken>()), Times.Once);
            Repository.Verify(r => r.Add(Product), Times.Once);
            Repository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Throw_ArgumentNullException_When_Product_Is_Null()
        {
            //Arrange
            var logic = Create();
            Product = null;
            //Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => logic.Create(Product));
        }

        [Fact]
        public async Task Return_Errors_When_Validation_Fails()
        {
            //Arrange
            const string warning = "You cannot create a new product";
            var logic = Create();
            await ProductValidator.SetValidatorFailure(warning);
            //Act
            var result = await logic.Create(Product);
            //Assert
            result.Should()
                .BeFailure(warning);
            ProductValidator.Verify(v => v.ValidateAsync(Product, It.IsAny<CancellationToken>()), Times.Once);
            Repository.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            Repository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}