using System;
using System.Collections.Generic;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.IntegrationTest.Data
{
    public static class PersonData
    {
        private static Guid StaticGuid { get; } = new Guid("9bfa5344-9468-442b-85aa-d4843ed8d27e");

        public static PersonDTO MockPostPerson { get; } = new PersonDTO
        {
            Name = "TestPost",
            Surname = "TestPost",
            Birthday = DateTime.Today,
        };

        public static ChildDTO MockPostChild { get; } = new ChildDTO
        {
            Name = "TestPost",
            Surname = "TestPost",
            Birthday = DateTime.Today,
        };

        public static PersonDTO MockPutPerson { get; } = new PersonDTO
        {
            Name = "TestPut",
            Surname = "TestPut",
            Birthday = DateTime.Today.AddYears(-1),
        };

        public static ChildDTO MockPutChild { get; } = new ChildDTO
        {
            Name = "TestPut",
            Surname = "TestPut",
            Birthday = DateTime.Today.AddYears(-1),
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
    }
}
