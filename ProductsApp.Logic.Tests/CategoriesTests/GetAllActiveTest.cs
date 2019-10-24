using System.Collections.Generic;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Moq;
using ProductsApp.Logic.Categories;
using ProductsApp.Models;
using Xunit;

namespace ProductsApp.Logic.Tests.CategoriesTests
{
    public class GetAllActiveTest : BaseTest
    {
        protected IEnumerable<Category> Categories { get; set; }
        public Result<IEnumerable<Category>> CategoriesResult { get; set; }

        protected override CategoryLogic Create()
        {
            var logic = base.Create();
            CorrectFlow();
            return logic;
        }

        private void CorrectFlow()
        {
            Categories = Builder<Category>
                .CreateListOfSize(10)
                .Build();
            CategoriesResult = Result.Ok(Categories);
            Repository.Setup(r => r.GetAllActive())
                .ReturnsAsync(Categories);
        }

        [Fact]
        public async Task Return_All_Active()
        {
            //Arrange
            var logic = Create();
            //Act
            var result = await logic.GetAllActive();
            //Assert
            result.Should()
                .BeSuccess(CategoriesResult.Value);
            Repository.Verify(r => r.GetAllActive(), Times.Once);
        } 
    }
}