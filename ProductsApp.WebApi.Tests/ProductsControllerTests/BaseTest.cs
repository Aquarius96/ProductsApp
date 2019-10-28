using AutoMapper;
using Moq;
using ProductsApp.Logic.Interfaces;
using ProductsApp.WebApi.Controllers;

namespace ProductsApp.WebApi.Tests.ProductsControllerTests
{
    public class BaseTest
    {
        protected Mock<IProductLogic> Logic { get; set; }
        protected Mock<IMapper> Mapper { get; set; }

        protected virtual ProductsController Create()
        {
            Logic = new Mock<IProductLogic>();
            Mapper = new Mock<IMapper>();

            return new ProductsController(Logic.Object,
                Mapper.Object);
        }
    }
}