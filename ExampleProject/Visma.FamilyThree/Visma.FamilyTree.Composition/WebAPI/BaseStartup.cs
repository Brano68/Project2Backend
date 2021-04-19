using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using Visma.FamilyTree.Composition.ConfigModels;

namespace Visma.FamilyTree.Composition.WebAPI
{
    public abstract class BaseStartup
    {
        protected BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            services.AddLogging(logBuilder =>
            {
                logBuilder.AddApplicationInsights(config.GetValue<string>($"{nameof(Logging)}:{nameof(Logging.ApplicationInsights)}:{nameof(Logging.ApplicationInsights.InstrumentationKey)}"));
            });

            services.AddApplicationInsightsTelemetry();

            services.Configure<AppInfo>(Configuration.GetSection(nameof(AppInfo)));

            services.AddMvcCore(option => AddServiceOptions(option, services));
            services.AddCors();
            services.AddSwaggerGen(c => c.SwaggerDoc(config.GetValue<string>($"{nameof(AppInfo)}:{nameof(AppInfo.ApplicationVerison)}"),
                new Info
                {
                    Title = config.GetValue<string>($"{nameof(AppInfo)}:{nameof(AppInfo.ApplicationName)}"),
                    Version = config.GetValue<string>($"{nameof(AppInfo)}:{nameof(AppInfo.ApplicationVerison)}")
                }));

            RegisterServices(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            RegisterTypes(builder);

            this.ApplicationContainer = builder.Build();

            this.RegisterScopedServices(this.ApplicationContainer);

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appInfo = app.ApplicationServices.GetRequiredService<IOptions<AppInfo>>().Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{appInfo.ApplicationVerison}/swagger.json", $"{appInfo.ApplicationName}"));

            app.UseHttpsRedirection();
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseMvc();
        }

        protected abstract void RegisterTypes(ContainerBuilder builder);

        protected abstract void RegisterServices(IServiceCollection services);

        protected abstract void RegisterScopedServices(IContainer container);

        protected abstract void AddServiceOptions(MvcOptions options, ILogger services);

        private void AddServiceOptions(MvcOptions options, IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<BaseStartup>>();

            AddServiceOptions(options, logger);
        }
    }
}
