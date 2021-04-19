using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Visma.FamilyTree.DbModels.Model;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.Repository.Interfaces;

namespace Visma.FamilyTree.Repository.Implementation
{
    public class PersonRepo : IPersonRepo
    {
        public PersonRepo(
            ILifetimeScope lifetimeScope)
        {
            ScopeProvider = lifetimeScope;
        }

        private ILifetimeScope ScopeProvider { get; }

        public async Task AddPerson(PersonDTO personDTO)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                familyTreeContext.Person.Add(MapToEntity(personDTO));

                await familyTreeContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdatePerson(Guid id, PersonDTO personDTO)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                personDTO.ID = id;

                familyTreeContext.Person.Update(MapToEntity(personDTO));

                await familyTreeContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                return await familyTreeContext.Person.Include(c => c.Child).OrderBy(sn => sn.Surname).ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<Person> GetPerson(Guid id)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                return await familyTreeContext.Person.FindAsync(id)
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Person>> GetPerson(string name, string surname)
        {
            using (var scope = ScopeProvider.BeginLifetimeScope())
            {
                var familyTreeContext = scope.Resolve<FamilyTreedbContext>();

                return await Task.FromResult(familyTreeContext.Person.Where(c => name == c.Name && surname == c.Surname)
                    .Include(c => c.Child))
                    .ConfigureAwait(false);
            }
        }

        private Person MapToEntity(PersonDTO personDTO) =>
            new Person
            {
                Id = personDTO.ID,
                Name = personDTO.Name,
                Surname = personDTO.Surname,
                Birthday = personDTO.Birthday
            };
    }
}
