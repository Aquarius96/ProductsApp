using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsApp.Logic;
using ProductsApp.Models;
using ProductsApp.WebApi.Controllers;
using Xunit;

namespace ProductsApp.WebApi.Tests.ProductsControllerTests
{
    public class DeleteTest : BaseTest
    {
        protected Product Product { get; set; }
        protected Result<Product> OkProductResult { get; set; }
        protected  Result<Product> ErrorProductResult { get; set; }
        protected Result OkResult { get; set; }
        protected Result ErrorResult { get; set; }

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
            OkProductResult = Result.Ok(Product);
            ErrorProductResult = Result.Error<Product>("Error");
            OkResult = Result.Ok();
            ErrorResult = Result.Error("Error");
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(OkProductResult);
            Logic.Setup(l => l.Remove(Product))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_GetById_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(ErrorProductResult);
            //Act
            var result = await controller.Delete(10);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Logic.Verify(l => l.Remove(Product), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequest_When_Update_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.Remove(It.IsAny<Product>()))
                .ReturnsAsync(ErrorProductResult);
            //Act
            var result = await controller.Delete(10);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Logic.Verify(l => l.Remove(Product), Times.Once);
        }

        [Fact]
        public async Task Return_NoContent_When_Product_Is_Deleted()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Delete(10);
            //Assert
            result.Should()
                .BeOfType<NoContentResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Logic.Verify(l => l.Remove(Product), Times.Once);
        }
    }
}