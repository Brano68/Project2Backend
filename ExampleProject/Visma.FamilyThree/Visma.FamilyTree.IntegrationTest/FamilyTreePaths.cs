using System;

namespace Visma.FamilyTree.IntegrationTest
{
    public static class FamilyTreePaths
    {
        public static string BaseUrl { get; } = "http://localhost:49158/api/";

        public static string Person { get; } = "person";

        private static string PostChild { get; } = "person/{0}/child";

        public static string PostChildLink(Guid id) =>
            string.Format(PostChild, id.ToString());

        public static string PersonById(Guid id) =>
            $"{Person}/{id}";

        public static string PutChild(Guid personId, Guid childId) =>
            string.Format($"{PostChild}/{childId}", personId);
    }
}
