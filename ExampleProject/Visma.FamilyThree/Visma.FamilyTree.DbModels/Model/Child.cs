using System;
using System.Collections.Generic;

namespace Visma.FamilyTree.DbModels.Model
{
    public partial class Child
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthday { get; set; }
        public Guid? PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
