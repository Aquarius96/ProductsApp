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

namespace ProductsApp.WebApi.Tests.ProductsControllerTests
{
    public class GetAllActiveTest : BaseTest
    {
        protected IEnumerable<Product> Products { get; set; }
        protected IEnumerable<ProductDto> ProductsDto { get; set; }
        protected Result<IEnumerable<Product>> ProductsResult { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();
            CorrectFlow();
            return controller;
        }

        private void CorrectFlow()
        {
            Products = Builder<Product>
                .CreateListOfSize(10)
                .Build();
            ProductsResult = Result.Ok(Products);

            Mapper.Setup(m => m.Map<IEnumerable<ProductDto>>(Products))
                .Returns(ProductsDto);
            Logic.Setup(l => l.GetAllActive())
                .ReturnsAsync(ProductsResult);
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
                .BeOk(ProductsDto);
            Mapper.Verify(m => m.Map<IEnumerable<ProductDto>>(Products), Times.Once);
            Logic.Verify(l => l.GetAllActive(), Times.Once);
        }
    }
}