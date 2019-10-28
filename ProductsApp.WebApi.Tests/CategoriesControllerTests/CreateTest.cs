using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsApp.Logic;
using ProductsApp.Models;
using ProductsApp.WebApi.Controllers;
using ProductsApp.WebApi.Dto;
using Xunit;

namespace ProductsApp.WebApi.Tests.CategoriesControllerTests
{
    public class CreateTest : BaseTest
    {
        protected Category Category { get; set; }
        protected CategoryDto CategoryDto { get; set; }
        protected Result<Category> OkCategoryResult { get; set; }
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
            CategoryDto = Builder<CategoryDto>
                .CreateNew()
                .Build();
            OkCategoryResult = Result.Ok(Category);
            ErrorCategoryResult = Result.Error<Category>("Error");

            Mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryDto>()))
                .Returns(Category);
            Logic.Setup(l => l.Create(It.IsAny<Category>()))
                .ReturnsAsync(OkCategoryResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_Create_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.Create(It.IsAny<Category>()))
                .ReturnsAsync(ErrorCategoryResult);
            //Act
            var result = await controller.Create(CategoryDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            controller.Should()
                .HasError(string.Empty, "Error");
            Mapper.Verify(m => m.Map<Category>(CategoryDto), Times.Once);
            Logic.Verify(l => l.Create(Category), Times.Once);
        }

        [Fact]
        public async Task Return_Status201_When_Category_Is_Added()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Create(CategoryDto);
            //Assert
            result.Should()
                .BeSuccess(CategoryDto);
            Mapper.Verify(m => m.Map<Category>(CategoryDto), Times.Once);
            Logic.Verify(l => l.Create(Category), Times.Once);
        }
    }
}