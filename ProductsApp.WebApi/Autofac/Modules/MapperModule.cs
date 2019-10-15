using Autofac;
using AutoMapper;
using System.Collections.Generic;

namespace ProductsApp.WebApi.Autofac.Modules
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(MapperModule).Assembly).As<Profile>();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(MapperModule).Assembly)
                .AsClosedTypesOf(typeof(ITypeConverter<,>))
                .AsSelf();

            builder.RegisterAssemblyTypes(typeof(MapperModule).Assembly)
                .AsClosedTypesOf(typeof(IValueResolver<,,>))
                .AsSelf();
        }
    }
}
