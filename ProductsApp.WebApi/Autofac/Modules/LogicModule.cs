using Autofac;
using FluentValidation;
using ProductsApp.Logic.Interfaces;
using ProductsApp.WebApi.Infrastructure;

namespace ProductsApp.WebApi.Autofac.Modules
{
    public class LogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .Where(t => typeof(ILogic).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();
        }
    }
}
