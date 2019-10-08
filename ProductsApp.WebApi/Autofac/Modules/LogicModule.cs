using Autofac;
using ProductsApp.Logic.Interfaces;

namespace ProductsApp.WebApi.Autofac.Modules
{
    public class LogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .Where(t => typeof(ILogic).IsAssignableFrom(t))
                .AsImplementedInterfaces();
        }
    }
}
