using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Categories;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.CategoriesTests
{
    public class GetByIdTest : BaseTest
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
            Category = Builder<Category>
                .CreateNew()
                .Build();
            Repository.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(Category);
        }

        [Fact]
        public async Task Return_Errors_When_Category_Does_Not_Exist()
        {
            //Arrange
            var logic = Create();
            Category = null;
            Repository.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(Category);
            //Act
            var result = await logic.GetById(10);
            //Assert
            result.Should()
                .BeFailure("There is no Category with id: 10");
            Repository.Verify(r => r.GetById(10), Times.Once);
        }

        [Fact]
        public async Task Return_Result_When_Category_Exists()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.GetById(2);
            //Assert
            result.Should()
                .BeSuccess(Category);
            Repository.Verify(r => r.GetById(2), Times.Once);
        }
    }
}
