using FluentValidation;
using Moq;
using ProductsApp.Logic.Products;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;

namespace ProductsApp.Logic.Tests.ProductsTests
{
    public class BaseTest
    {
        protected Mock<IProductRepository> Repository { get; set; }
        protected Mock<ICustomValidatorFactory> ValidatorFactory { get; set; }
        protected Mock<IValidator<Product>> ProductValidator { get; set; }

        protected virtual ProductLogic Create()
        {
            Repository = new Mock<IProductRepository>();
            ValidatorFactory = new Mock<ICustomValidatorFactory>();
            ProductValidator = new Mock<IValidator<Product>>();

            return new ProductLogic(Repository.Object,
                ValidatorFactory.Object);
        }
    }
}