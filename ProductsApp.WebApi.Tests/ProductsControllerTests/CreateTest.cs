using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsApp.Logic;
using ProductsApp.Models;
using ProductsApp.WebApi.Controllers;
using ProductsApp.WebApi.Dto;
using Xunit;

namespace ProductsApp.WebApi.Tests.ProductsControllerTests
{
    public class CreateTest : BaseTest
    {
        protected Product Product { get; set; }
        protected ProductDto ProductDto { get; set; }
        protected Result<Product> OkProductResult { get; set; }
        protected Result<Product> ErrorProductResult { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();
            CorrectFlow();
            return controller;
        }

        private void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();
            ProductDto = Builder<ProductDto>
                .CreateNew()
                .Build();
            OkProductResult = Result.Ok(Product);
            ErrorProductResult = Result.Error<Product>("Error");
            Mapper.Setup(m => m.Map<Product>(ProductDto))
                .Returns(Product);
            Logic.Setup(l => l.Create(Product))
                .ReturnsAsync(OkProductResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_Create_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.Create(Product))
                .ReturnsAsync(ErrorProductResult);
            //Act
            var result = await controller.Create(ProductDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            controller.Should()
                .HasError(string.Empty, "Error");
            Mapper.Verify(m => m.Map<Product>(ProductDto), Times.Once);
            Logic.Verify(l => l.Create(Product), Times.Once);
        }

        [Fact]
        public async Task Return_Status201_When_Product_Is_Added()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Create(ProductDto);
            //Assert
            result.Should()
                .BeSuccess(ProductDto);
            Mapper.Verify(m => m.Map<Product>(ProductDto), Times.Once);
            Logic.Verify(l => l.Create(Product), Times.Once);
        }
    }
}