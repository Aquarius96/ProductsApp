using FluentValidation;
using Moq;
using ProductsApp.Logic.Categories;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;

namespace ProductsApp.Logic.Tests.CategoriesTests
{
    public class BaseTest
    {
        protected Mock<ICategoryRepository> Repository { get; set; }
        protected Mock<ICustomValidatorFactory> ValidatorFactory { get; set; }
        protected Mock<IValidator<Category>> CategoryValidator { get; set; }

        protected virtual CategoryLogic Create()
        {
            Repository = new Mock<ICategoryRepository>();
            ValidatorFactory = new Mock<ICustomValidatorFactory>();
            CategoryValidator = new Mock<IValidator<Category>>();

            ValidatorFactory.Setup(v => v.Create<Category>())
                .Returns(CategoryValidator.Object);

            return new CategoryLogic(Repository.Object, 
                ValidatorFactory.Object);
        }
    }
}