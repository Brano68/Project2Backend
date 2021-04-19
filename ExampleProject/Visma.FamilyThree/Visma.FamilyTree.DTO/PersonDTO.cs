using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Visma.FamilyTree.DTO.Converters;

namespace Visma.FamilyTree.DTO
{
    public class PersonDTO
    {
        public Guid ID { get; set; }

        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Surname { get; set; }

        [JsonConverter(typeof(DateConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Birthday { get; set; }

        public int Age { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ChildDTO> Children { get; set; }

        public GeriatryEnum Geriatry { get; set; }
    }
}
