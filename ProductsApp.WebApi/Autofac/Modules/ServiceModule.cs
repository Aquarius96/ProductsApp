using Autofac;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.WebApi.Infrastructure;

namespace ProductsApp.WebApi.Autofac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IService).Assembly)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(CustomValidatorFactory).Assembly)
                .Where(t => typeof(ICustomValidatorFactory).IsAssignableFrom(t))
                .AsImplementedInterfaces();            
        }
    }
}
