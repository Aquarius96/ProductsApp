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
    public class UpdateTest : BaseTest
    {
        protected Category Category { get; set; }
        protected CategoryDto CategoryDto { get; set; }
        protected int CategoryId { get; set; }
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
            CategoryId = 10;
            Category = Builder<Category>
                .CreateNew()
                .With(c => c.Id)
                .Build();
            CategoryDto = Builder<CategoryDto>
                .CreateNew()
                .With(c => c.Id = CategoryId)
                .Build();
            Mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryDto>()))
                .Returns(Category);
            OkCategoryResult = Result.Ok(Category);
            ErrorCategoryResult = Result.Error<Category>("Error");
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(OkCategoryResult);
            Logic.Setup(l => l.Update(It.IsAny<Category>()))
                .ReturnsAsync(OkCategoryResult);
        }

        [Fact]
        public async Task Return_BadRequest_When_Ids_Are_Different()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Update(9, CategoryDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestResult>();
            Logic.Verify(l => l.GetById(It.IsAny<int>()), Times.Never);
            Mapper.Verify(m => m.Map(CategoryDto, Category), Times.Never);
            Logic.Verify(l => l.Update(Category), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequest_When_GetById_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(ErrorCategoryResult);
            //Act
            var result = await controller.Update(CategoryId, CategoryDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            Logic.Verify(l => l.GetById(CategoryId), Times.Once);
            Mapper.Verify(m => m.Map(CategoryDto, Category), Times.Never);
            Logic.Verify(l => l.Update(Category), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequest_When_Update_Fails()
        {
            //Arrange
            var controller = Create();
            Logic.Setup(l => l.GetById(It.IsAny<int>()))
                .ReturnsAsync(OkCategoryResult);
            Logic.Setup(l => l.Update(It.IsAny<Category>()))
                .ReturnsAsync(ErrorCategoryResult);
            //Act
            var result = await controller.Update(CategoryId, CategoryDto);
            //Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
            controller.Should()
                .HasError(string.Empty, "Error");
            Logic.Verify(l => l.GetById(CategoryId), Times.Once);
            Mapper.Verify(m => m.Map(CategoryDto, Category), Times.Once);
            Logic.Verify(l => l.Update(Category), Times.Once);
        }

        [Fact]
        public async Task Return_Success_When_Category_Is_Updated()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Update(CategoryId, CategoryDto);
            //Assert
            result.Should()
                .BeOk(CategoryDto);
            Logic.Verify(l => l.GetById(CategoryId), Times.Once);
            Mapper.Verify(m => m.Map(CategoryDto, Category), Times.Once);
            Logic.Verify(l => l.Update(Category), Times.Once);
        }
    }
}