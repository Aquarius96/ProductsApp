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
    public class UpdateTest : BaseTest
    {
        protected Product Product { get; set; }
        protected ProductDto ProductDto { get; set; }
        protected int ProductId { get; set; }
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
            ProductId = 10;
            Product = Builder<Product>
                .CreateNew()
                .With(p => p.Id = ProductId)
                .Build();
            ProductDto = Builder<ProductDto>
                .CreateNew()
                .With(p => p.Id = ProductId)
                .Build();
            OkProductResult = Result.Ok(Product);
            ErrorProductResult = Result.Error<Product>("Error");

            Mapper.Setup(m => m.Map(ProductDto, Product))
                .Returns(Product);
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(OkProductResult);
            Logic.Setup(l => l.Update(It.IsAny<Product>()))
                .ReturnsAsync(OkProductResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_Ids_Are_Different()
        {
            //Arrange
            var controller = Create();
            ProductId++;
            //Act
            var result = await controller.Update(ProductId, ProductDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestResult>();
            Logic.Verify(l => l.GetById(It.IsAny<int>()), Times.Never);
            Mapper.Verify(m => m.Map(ProductDto, Product), Times.Never);
            Logic.Verify(l => l.Update(Product), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequest_When_GetById_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(ErrorProductResult);
            //Act
            var result = await controller.Update(ProductId, ProductDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(It.IsAny<int>()), Times.Once);
            Mapper.Verify(m => m.Map(ProductDto, Product), Times.Never);
            Logic.Verify(l => l.Update(Product), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequest_When_Update_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.Update(It.IsAny<Product>()))
                .ReturnsAsync(ErrorProductResult);
            //Act
            var result = await controller.Update(ProductId, ProductDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(ProductId), Times.Once);
            Mapper.Verify(m => m.Map(ProductDto, Product), Times.Once);
            Logic.Verify(l => l.Update(Product), Times.Once);
        }

        [Fact]
        public async Task Return_Success_When_Product_Is_Updated()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Update(ProductId, ProductDto);
            //Assert
            result.Should()
                .BeOk(ProductDto);
            Logic.Verify(l => l.GetById(ProductId), Times.Once);
            Mapper.Verify(m => m.Map(ProductDto, Product), Times.Once);
            Logic.Verify(l => l.Update(Product), Times.Once);
        }
    }
}