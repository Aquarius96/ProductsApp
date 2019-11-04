using Autofac;
using ProductsApp.DataAccess;
using ProductsApp.Logic.Services.Interfaces;

namespace ProductsApp.WebApi.Autofac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IService).Assembly)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ServiceModule).Assembly)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.RegisterType<DatabaseRestoreService>()
                .AsImplementedInterfaces();
        }
    }
}
