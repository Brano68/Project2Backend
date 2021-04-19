using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Visma.FamilyTree.DbModels.Model;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.Repository.Interfaces;

namespace Visma.FamilyTree.Repository.Implementation
{
    public class ChildRepo : IChildRepo
    {
        public ChildRepo(
            ILifetimeScope lifetimeScope)
        {
            ScopeProvider = lifetimeScope;
        }

        private ILifetimeScope ScopeProvider { get; }

        public async Task AddChild(Guid personID, ChildDTO childDTO)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                await familyTreeContext.Child.AddAsync(MapToEntity(personID, childDTO)).ConfigureAwait(false);

                await familyTreeContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateChild(Guid personID, Guid childId, ChildDTO childDTO)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                childDTO.Id = childId;

                familyTreeContext.Child.Update(MapToEntity(personID, childDTO));

                await familyTreeContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<Person> GetPersonWithChildren(Guid personId)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                return await familyTreeContext.Person.Where(p => p.Id == personId)
                    .Include(c => c.Child)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
            }
        }

        public async Task<Child> GetChild(Guid id)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                return await familyTreeContext.Child.FindAsync(id).ConfigureAwait(false);
            }
        }

        private Child MapToEntity(Guid personId, ChildDTO childDTO) =>
            new Child
            {
                PersonId = personId,
                Id = childDTO.Id,
                Name = childDTO.Name,
                Surname = childDTO.Surname,
                Birthday = childDTO.Birthday
            };
    }
}
