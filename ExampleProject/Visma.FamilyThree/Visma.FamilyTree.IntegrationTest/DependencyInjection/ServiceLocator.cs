using Autofac;

namespace Visma.FamilyTree.IntegrationTest.DependencyInjection
{
    public static class ServiceLocator
    {
        private static IContainer container;

        static ServiceLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<FamilyTreeClientModule>();

            Container = builder.Build();
        }

        public static IContainer Container
        {
            get
            {
                return container;
            }

            private set
            {
                container = value;
            }
        }
    }
}
