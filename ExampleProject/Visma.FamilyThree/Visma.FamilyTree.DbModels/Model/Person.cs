using System;
using System.Collections.Generic;

namespace Visma.FamilyTree.DbModels.Model
{
    public partial class Person
    {
        public Person()
        {
            Child = new HashSet<Child>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Child> Child { get; set; }
    }
}
