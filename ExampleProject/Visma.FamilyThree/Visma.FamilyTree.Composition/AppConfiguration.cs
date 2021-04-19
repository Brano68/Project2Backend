using System.Collections.Generic;

namespace Visma.FamilyTree.Composition
{
    public class AppConfiguration
    {
        public string ConnectionString { get; set; }

        public string RandomNumberGenerator { get; set; }

        public int CacheTimeOutSeconds { get; set; }

        public IDictionary<string, object> RandomGeneratorQueryParams { get; set; }
    }
}
