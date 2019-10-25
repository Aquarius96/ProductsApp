using System;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Categories;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.CategoriesTests
{
    public class RemoveTest : BaseTest
    {
        protected Category Category { get; set; }

        protected override CategoryLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private void CorrectFlow()
        {
            Category = Builder<Category>.CreateNew().Build();
        }

        [Fact]
        public async Task Throw_ArgumentNullException_When_Category_Is_Null()
        {
            //Arrange
            var logic = Create();
            Category = null;
            //Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => logic.Remove(Category));
        }

        [Fact]
        public async Task Return_Success_When_Category_Is_Deleted()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.Remove(Category);
            //Arrange
            result.Should()
                .BeSuccess();
            Repository.Verify(r => r.Delete(Category), Times.Once);
            Repository.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}