using System.Collections.Generic;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Products;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.ProductsTests
{
    public class GetAllActiveTest : BaseTest
    {
        protected IEnumerable<Product> Products { get; set; }
        public Result<IEnumerable<Product>> ProductsResult { get; set; }

        protected override ProductLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private void CorrectFlow()
        {
            Products = Builder<Product>
                .CreateListOfSize(10)
                .Build();
            ProductsResult = Result.Ok(Products);
            Repository.Setup(r => r.GetAllActive())
                .ReturnsAsync(Products);
        }

        [Fact]
        public async Task Return_All_Active()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.GetAllActive();
            //
            result.Should()
                .BeSuccess(ProductsResult.Value);
            Repository.Verify(r => r.GetAllActive(), Times.Once);
        }
    }
}