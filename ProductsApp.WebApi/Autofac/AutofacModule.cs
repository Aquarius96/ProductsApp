using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductsApp.DataAccess;
using ProductsApp.Logic.Interfaces;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services;
using ProductsApp.Logic.Services.Interfaces;
using System.Data.SqlClient;
using System.Linq;

namespace ProductsApp.WebApi.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<DataContext>();
                var builder = new SqlConnectionStringBuilder(config.GetConnectionString("DefaultConnection"))
                {
                    Password = config["databasePassword"]
                };
                opt.UseSqlServer(builder.ConnectionString);

                var dateService = new DateService();

                return new DataContext(opt.Options, dateService);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .Where(t => typeof(ILogic).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(IService).Assembly)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .AsImplementedInterfaces();
        }
    }
}
