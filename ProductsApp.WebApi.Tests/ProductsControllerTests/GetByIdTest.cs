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
    public class GetByIdTest : BaseTest
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

            Mapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(ProductDto);
            Logic.Setup(m => m.GetById(It.IsAny<int>()))
                .ReturnsAsync(OkProductResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_GetById_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(ErrorProductResult);
            //Act
            var result = await controller.GetById(10);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Mapper.Verify(m => m.Map<ProductDto>(Product), Times.Never);
        }

        [Fact]
        public async Task Return_Product_When_Result_Is_Success()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetById(10);
            //Assert
            result.Should()
                .BeOk(ProductDto);
            Logic.Verify(l => l.GetById(10), Times.Once);
            Mapper.Verify(m => m.Map<ProductDto>(Product), Times.Once);
        }
    }
}