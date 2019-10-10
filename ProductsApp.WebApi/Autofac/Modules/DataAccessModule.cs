﻿using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductsApp.DataAccess;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services;
using ProductsApp.WebApi.Configuration;
using System.Data.SqlClient;

namespace ProductsApp.WebApi.Autofac.Modules
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<DataContext>();
                var builder = new SqlConnectionStringBuilder(config.GetConnectionString(SettingsNames.ConnectionString))
                {
                    Password = config[SettingsNames.DataBasePassword]
                };
                opt.UseSqlServer(builder.ConnectionString);

                var dateService = new DateService();

                return new DataContext(opt.Options, dateService);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .AsImplementedInterfaces();
        }
    }
}
