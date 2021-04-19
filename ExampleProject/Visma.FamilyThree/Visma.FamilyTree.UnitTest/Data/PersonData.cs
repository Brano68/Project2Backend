using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Visma.FamilyTree.DbModels.Model;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.UnitTest.Data
{
    public static class PersonData
    {
        private static Guid StaticGuid { get; } = new Guid("9bfa5344-9468-442b-85aa-d4843ed8d27e");

        public static PersonDTO MockPerson { get; } = new PersonDTO
        {
            Name = "Test",
            Surname = "Test",
            Birthday = DateTime.Today,
            Age = 15
        };

        public static IEnumerable<PersonDTO> MockPersonDtoList { get; } = new List<PersonDTO>
        {
            new PersonDTO
            {
                ID = StaticGuid,
                Name = "Test",
                Surname = "Test",
                Birthday = DateTime.Today,
                Age = 15,
                Children = new List<ChildDTO>
                {
                    new ChildDTO
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test",
                        Surname = "Test",
                        Birthday = DateTime.Today,
                        Age = 15,
                        PersonId = StaticGuid
                    }
                }
            }
        };

        public static IEnumerable<Person> MockPersonEntityList { get; } = new List<Person>
        {
            new Person
            {
                Id = StaticGuid,
                Name = "Test",
                Surname = "Test",
                Birthday = DateTime.Today,
                Child = new Collection<Child>
                {
                    new Child
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test",
                        Surname = "Test",
                        Birthday = DateTime.Today,
                        PersonId = StaticGuid
                    }
                }
            }
        };
    }
}
