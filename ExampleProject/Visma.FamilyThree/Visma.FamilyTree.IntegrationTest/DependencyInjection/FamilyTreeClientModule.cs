using Autofac;
using Visma.FamilyTree.IntegrationTest.HttpClients.Implementation;
using Visma.FamilyTree.IntegrationTest.HttpClients.Interfaces;

namespace Visma.FamilyTree.IntegrationTest.DependencyInjection
{
    public class FamilyTreeClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FamilyTreeClient>().As<IFamilyTreeClient>().SingleInstance();
        }
    }
}
