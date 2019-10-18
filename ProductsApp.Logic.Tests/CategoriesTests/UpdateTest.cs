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
    public class UpdateTest : BaseTest
    {
        protected Category Category { get; set; }

        protected override CategoryLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private async Task CorrectFlow()
        {
            Category = Builder<Category>.CreateNew().Build();
            await CategoryValidator.SetValidatorSuccess();
            ValidatorFactory.Setup(v => v.Create<Category>())
                .Returns(CategoryValidator.Object);
        }

        [Fact]
        public async Task Throw_Exception_When_Category_Is_Null()
        {
            //Arrange
            var logic = Create();
            Category = null;
            //Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => logic.Update(Category));
            ValidatorFactory.Verify(v => v.Create<Category>(), Times.Never);
            Repository.Verify(r => r.SaveChanges(), Times.Never);
            CategoryValidator.Verify(v => v.ValidateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Return_Errors_When_Validation_Fails()
        {
            //Arrange
            var warning = "You cannot update Category";
            var logic = Create();
            await CategoryValidator.SetValidatorFailure(warning);
            //Act
            var result = await logic.Update(Category);
            //Assert
            result.Should().BeFailure(warning);
            Repository.Verify(r => r.SaveChanges(), Times.Never);
            CategoryValidator.Verify(v => v.ValidateAsync(Category, It.IsAny<CancellationToken>()), Times.Once());
            ValidatorFactory.Verify(v => v.Create<Category>(), Times.Once);
        }

        [Fact]
        public async Task Return_Success_When_Category_Is_Updated()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.Update(Category);
            //Assert
            result.Should().BeSuccess(Category);
            Repository.Verify(r => r.SaveChanges(), Times.Once);
            CategoryValidator.Verify(v => v.ValidateAsync(Category, It.IsAny<CancellationToken>()), Times.Once);
            ValidatorFactory.Verify(v => v.Create<Category>(), Times.Once);
        }
    }
}
