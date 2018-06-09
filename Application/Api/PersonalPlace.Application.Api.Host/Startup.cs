using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pactor.Infra.Crosscutting.IoC.Core;
using Pactor.Infra.Crosscutting.LogCore;
using Pactor.Infra.Crosscutting.ObjectMapper;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.NHibernate.Facility;
using PersonalPlace.Application.Api.Services.Controllers;

namespace PersonalPlace.Application.Api.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; set; }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddApplicationPart(typeof(ValuesController).Assembly)
                .AddControllersAsServices()
                .AddAuthorization()
                .AddJsonFormatters();

            var logger = LogBootstrap.GetLogger(typeof(Startup));
            var machineName = Environment.MachineName;

            logger.Debug(() => $"Starting IoC container at {machineName}");

            var containerBootstrap = new ContainerBootstrap();
            var builder = containerBootstrap.Config();

            builder.RegisterInstance(Configuration)
                   .As<IConfiguration>()
                   .SingleInstance();

            logger.Debug(() => $"Starting object mapping at {machineName}");

            ObjectMapperBootstrap.Config(builder, new[] { "PersonalPlace", "Pactor" });

            logger.Debug(() => $"Startig ORM at {machineName}");

            var connectionProvider = new ConnectionProvider(Configuration);
            var ormBootstrap = new NHibernateORMBootstrap(LogBootstrap.GetLogger(typeof(NHibernateORMBootstrap).FullName), connectionProvider);
            NHibernateProfiler.Initialize();
            ormBootstrap.Config(builder);

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
