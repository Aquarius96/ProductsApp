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
    public class GetByIdTest : BaseTest
    {
        protected Category Category { get; set; }
        protected Result<Category> OkCategoryResult { get; set; }
        protected Result<Category> ErrorCategoryResult { get; set; }
        protected CategoryDto CategoryDto { get; set; }

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
            CategoryDto = Builder<CategoryDto>
                .CreateNew()
                .Build();
            Mapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
                .Returns(CategoryDto);
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(OkCategoryResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_Result_Is_Failure()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(ErrorCategoryResult);
            //Act
            var result = await controller.GetById(10);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(10), Times.Once);
            Mapper.Verify(m => m.Map<CategoryDto>(Category), Times.Never);
        }

        [Fact]
        public async Task Return_Category_When_Result_Is_Success()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetById(10);
            //Assert
            result.Should()
                .BeOk(CategoryDto);
            Logic.Verify(l => l.GetById(10), Times.Once);
            Mapper.Verify(m => m.Map<CategoryDto>(Category), Times.Once);
        }
    }
}