using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.WebAPI.Managers.Interfaces
{
    public interface IPersonManager
    {
        Task<IEnumerable<PersonDTO>> GetAllPersons();

        Task<PersonDTO> GetPerson(Guid id);

        Task<PersonDTO> AddPerson(PersonDTO person);

        Task<PersonDTO> UpdatePerson(Guid personId, PersonDTO person);
    }
}
