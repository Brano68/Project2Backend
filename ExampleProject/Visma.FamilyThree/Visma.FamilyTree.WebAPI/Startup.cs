using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Visma.FamilyTree.Composition;
using Visma.FamilyTree.Composition.WebAPI;
using Visma.FamilyTree.DbModels.Model;
using Visma.FamilyTree.RandomNumberGenerator.Implementation;
using Visma.FamilyTree.RandomNumberGenerator.Interfaces;
using Visma.FamilyTree.Repository.Implementation;
using Visma.FamilyTree.Repository.Interfaces;
using Visma.FamilyTree.WebAPI.Filters;
using Visma.FamilyTree.WebAPI.Managers.Implementation;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;

namespace Visma.FamilyTree.WebAPI
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void AddServiceOptions(MvcOptions options, ILogger logger)
        {
            options.Filters.Add(new HandleExceptionFilterAttribute(logger));
        }

        protected override void RegisterScopedServices(IContainer container)
        {
            using (var lifetimescope = container.BeginLifetimeScope())
            {
                var dbContextService = lifetimescope.Resolve<FamilyTreedbContext>();
            }
        }

        protected override void RegisterServices(IServiceCollection services)
        {
            services.Configure<AppConfiguration>(Configuration.GetSection(nameof(AppConfiguration)));
            services.AddDbContext<FamilyTreedbContext>(opt => opt.UseInMemoryDatabase(Configuration.GetConnectionString("DefaultConnection")));
        }

        protected override void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ChildRepo>().As<IChildRepo>();
            builder.RegisterType<PersonRepo>().As<IPersonRepo>();
            builder.RegisterType<CacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<PersonManager>().As<IPersonManager>().SingleInstance();
            builder.RegisterType<ChildManager>().As<IChildManager>().SingleInstance();
            builder.RegisterType<NumberGeneratorClient>().As<INumberGeneratorClient>().SingleInstance();
        }
    }
}
