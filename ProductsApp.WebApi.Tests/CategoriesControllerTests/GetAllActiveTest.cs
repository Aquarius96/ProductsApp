using System.Collections.Generic;
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
    public class GetAllActiveTest : BaseTest
    {
        protected IEnumerable<Category> Categories { get; set; }
        protected Result<IEnumerable<Category>> CategoriesResult { get; set; }
        protected IEnumerable<CategoryDto> CategoriesDto { get; set; }

        protected override CategoriesController Create()
        {
            var controller = base.Create();
            CorrectFlow();
            return controller;
        }

        private void CorrectFlow()
        {
            Categories = Builder<Category>
                .CreateListOfSize(10)
                .Build();
            CategoriesDto = Builder<CategoryDto>
                .CreateListOfSize(10)
                .Build();
            CategoriesResult = Result.Ok(Categories);

            Mapper.Setup(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<List<Category>>()))
                .Returns(CategoriesDto);
            Logic.Setup(l => l.GetAllActive())
                .ReturnsAsync(CategoriesResult);
        }

        [Fact]
        public async Task Return_All_Active()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetAllActive();
            //Assert
            result.Should()
                .BeOk(CategoriesDto);
            Logic.Verify(l => l.GetAllActive(), Times.Once);
            Mapper.Verify(m => m.Map<IEnumerable<CategoryDto>>(Categories), Times.Once);
        }
    }
}