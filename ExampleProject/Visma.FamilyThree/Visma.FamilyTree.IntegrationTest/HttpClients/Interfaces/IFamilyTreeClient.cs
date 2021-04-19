using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.IntegrationTest.HttpClients.Interfaces
{
    public interface IFamilyTreeClient
    {
        Task<PersonDTO> PostPerson(PersonDTO personDTO);

        Task<ChildDTO> PostChild(Guid personId, ChildDTO childDTO);

        Task<ChildDTO> PutChild(Guid personId, ChildDTO childDTO);

        Task<PersonDTO> PutPerson(PersonDTO personDTO);

        Task<IEnumerable<PersonDTO>> GetPersons();

        Task<PersonDTO> GetPerson(Guid personId);
    }
}
