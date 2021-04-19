using System;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;

namespace Visma.FamilyTree.WebAPI.Managers.Interfaces
{
    public interface IChildManager
    {
        Task<ChildDTO> AddChild(Guid personId, ChildDTO child);

        Task<ChildDTO> UpdateChild(Guid personId, Guid childId, ChildDTO child);
    }
}
