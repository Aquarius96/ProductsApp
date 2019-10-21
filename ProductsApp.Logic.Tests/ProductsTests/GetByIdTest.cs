using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using ProductsApp.Logic.Products;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.ProductsTests
{
    public class GetByIdTest : BaseTest
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
            Repository.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(Product);
        }

        [Fact]
        public async Task Return_Errors_When_Product_Does_Not_Exist()
        {
            //Arrange
            var logic = Create();
            Repository.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Product) null);
            //Act
            var result = await logic.GetById(10);
            //Assert
            result.Should().BeFailure("There is no Product with id: 10");
            Repository.Verify(r => r.GetById(10), Times.Once);
        }

        [Fact]
        public async Task Return_Result_When_Product_Exists()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.GetById(10);
            //Assert
            result.Should().BeSuccess(Product);
            Repository.Verify(r => r.GetById(10), Times.Once);
        }
    }
}
