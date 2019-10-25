using System;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Products;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.ProductsTests
{
    public class RemoveTest : BaseTest
    {
        protected Product Product { get; set; }
        protected override ProductLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private void CorrectFlow()
        {
            Product = Builder<Product>.CreateNew().Build();
        }

        [Fact]
        public async Task Throw_ArgumentNullException_When_Product_Is_Null()
        {
            //Arrange
            var logic = Create();
            Product = null;
            //Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => logic.Remove(Product));
        }

        [Fact]
        public async Task Return_Success_When_Product_Is_Deleted()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.Remove(Product);
            //Assert
            result.Should()
                .BeSuccess();
            Repository.Verify(r => r.Delete(Product), Times.Once);
            Repository.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}