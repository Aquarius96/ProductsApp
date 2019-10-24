using System;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Categories;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.CategoriesTests
{
    public class CreateTest : BaseTest
    {
        protected Category Category;

        protected override CategoryLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private async Task CorrectFlow()
        {
            Category = Builder<Category>
                .CreateNew()
                .Build();
            await CategoryValidator.SetValidatorSuccess();
        }

        [Fact]
        public async Task Return_Success_When_Category_Is_Added()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.Create(Category);
            //Assert
            result.Should()
                .BeSuccess(Category);
            CategoryValidator.Verify(v => v.ValidateAsync(Category, It.IsAny<CancellationToken>()), Times.Once);
            Repository.Verify(r => r.Add(Category), Times.Once);
            Repository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Throw_ArgumentNullException_When_Category_Is_Null()
        {
            //Arrange
            var logic = Create();
            Category = null;
            //Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => logic.Create(Category));
        }

        [Fact]
        public async Task Return_Errors_When_Validation_Fails()
        {
            //Arrange
            const string warning = "You cannot create a new category";
            var logic = Create();
            await CategoryValidator.SetValidatorFailure(warning);
            //Act
            var result = await logic.Create(Category);
            //Assert
            result.Should()
                .BeFailure(warning);
            CategoryValidator.Verify(v => v.ValidateAsync(Category, It.IsAny<CancellationToken>()), Times.Once);
            Repository.Verify(r => r.Add(It.IsAny<Category>()), Times.Never);
            Repository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}