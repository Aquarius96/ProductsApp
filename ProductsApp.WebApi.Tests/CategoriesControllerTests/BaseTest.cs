using AutoMapper;
using Moq;
using ProductsApp.Logic.Interfaces;
using ProductsApp.WebApi.Controllers;

namespace ProductsApp.WebApi.Tests.CategoriesControllerTests
{
    public class BaseTest
    {
        protected Mock<ICategoryLogic> Logic { get; set; }
        protected Mock<IMapper> Mapper { get; set; }

        protected virtual CategoriesController Create()
        {
            Logic = new Mock<ICategoryLogic>();
            Mapper = new Mock<IMapper>();

            return new CategoriesController(Logic.Object, 
                Mapper.Object);
        }
    }
}