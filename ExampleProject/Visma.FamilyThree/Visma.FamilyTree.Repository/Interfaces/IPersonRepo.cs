using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visma.FamilyTree.DbModels.Model;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.Repository.Interfaces
{
    public interface IPersonRepo
    {
        Task<Person> GetPerson(Guid id);

        Task<IEnumerable<Person>> GetAllPersons();

        Task AddPerson(PersonDTO personDTO);

        Task UpdatePerson(Guid id, PersonDTO personDTO);
    }
}
