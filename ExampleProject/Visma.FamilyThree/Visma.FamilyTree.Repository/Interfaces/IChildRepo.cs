using System;
using System.Threading.Tasks;
using Visma.FamilyTree.DbModels.Model;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.Repository.Interfaces
{
    public interface IChildRepo
    {
        Task<Child> GetChild(Guid Id);

        Task<Person> GetPersonWithChildren(Guid personId);

        Task AddChild(Guid personID, ChildDTO childDTO);

        Task UpdateChild(Guid personID, Guid childId, ChildDTO childDTO);
    }
}
