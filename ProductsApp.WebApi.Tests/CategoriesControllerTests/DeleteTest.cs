using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsApp.Logic;
using ProductsApp.Models;
using ProductsApp.WebApi.Controllers;
using Xunit;

namespace ProductsApp.WebApi.Tests.CategoriesControllerTests
{
    public class DeleteTest : BaseTest
    {
        protected Category Category { get; set; }
        protected Result OkResult { get; set; }
        protected Result<Category> OkCategoryResult { get; set; }
        protected Result ErrorResult { get; set; }
        protected Result<Category> ErrorCategoryResult { get; set; }

        protected override CategoriesController Create()
        {
            var controller = base.Create();
            CorrectFlow();
            return controller;
        }

        private void CorrectFlow()
        {
            Category = Builder<Category>
                .CreateNew()
                .Build();
            OkCategoryResult = Result.Ok(Category);
            ErrorCategoryResult = Result.Error<Category>("Error");
            OkResult = Result.Ok();
            ErrorResult = Result.Error("Error");
            
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => OkCategoryResult);
            Logic.Setup(l => l.Remove(It.IsAny<Category>()))
                .ReturnsAsync(() => OkResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_GetById_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(ErrorCategoryResult);
            //Act
            var result = await controller.Delete(10);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Logic.Verify(l => l.Remove(Category), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequest_When_Remove_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.Remove(It.IsAny<Category>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.Delete(10);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Logic.Verify(l => l.Remove(Category), Times.Once);
        }

        [Fact]
        public async Task Return_Ok()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Delete(10);
            //Assert
            result.Should()
                .BeOfType<NoContentResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Logic.Verify(l => l.Remove(Category), Times.Once);
        }
    }
}