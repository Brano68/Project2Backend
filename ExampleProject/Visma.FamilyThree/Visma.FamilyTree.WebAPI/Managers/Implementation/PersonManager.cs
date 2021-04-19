using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visma.FamilyTree.Composition.Constants;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.RandomNumberGenerator.Interfaces;
using Visma.FamilyTree.Repository.Interfaces;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;

namespace Visma.FamilyTree.WebAPI.Managers.Implementation
{
    public class PersonManager : IPersonManager
    {
        public PersonManager(
            IPersonRepo personRepo,
            IChildRepo childRepo,
            ICacheManager cacheManager,
            INumberGeneratorClient numberGenerator,
            ILogger<PersonManager> logger)
        {
            PersonRepo = personRepo;
            CacheManager = cacheManager;
            NumberGenerator = numberGenerator;
            ChildRepo = childRepo;
            Logger = logger;
        }

        private ILogger<PersonManager> Logger { get; set; }

        private IPersonRepo PersonRepo { get; }

        private IChildRepo ChildRepo { get; }

        private ICacheManager CacheManager { get; }

        private INumberGeneratorClient NumberGenerator { get; }

        public async Task<PersonDTO> AddPerson(PersonDTO person)
        {
            person.ID = Guid.NewGuid();

            await PersonRepo.AddPerson(person).ConfigureAwait(false);

            person.Age = person.Birthday.HasValue
                ? DateTime.Today.Year - person.Birthday.Value.Year
                : await NumberGenerator.GetRandomNumbers().ConfigureAwait(false);

            Logger.LogInformation($"Person with id {person.ID} successfully created.");

            CacheManager.CleanCachedItem(CacheKeys.PersonListKey);

            return person;
        }

        public async Task<PersonDTO> UpdatePerson(Guid personId, PersonDTO person)
        {
            var personEntity = await PersonRepo.GetPerson(personId).ConfigureAwait(false);

            if (personEntity == null)
                return null;

            await PersonRepo.UpdatePerson(personId, person).ConfigureAwait(false);

            person.Age = person.Birthday.HasValue
                ? DateTime.Today.Year - person.Birthday.Value.Year
                : await NumberGenerator.GetRandomNumbers().ConfigureAwait(false);

            person.ID = personId;

            Logger.LogInformation($"Person with id {person.ID} successfully updated.");

            CacheManager.CleanCachedItem($"{CacheKeys.PersonKey}-{person.ID}");

            return person;
        }

        public async Task<PersonDTO> GetPerson(Guid id)
        {
            var person = await ChildRepo.GetPersonWithChildren(id).ConfigureAwait(false);

            if (person == null)
                return null;

            var randomNumber = await NumberGenerator.GetRandomNumbers().ConfigureAwait(false);

            var personDto = new PersonDTO
            {
                ID = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Birthday = person.Birthday,
                Age = person.Birthday.HasValue
                    ? DateTime.Today.Year - person.Birthday.Value.Year
                    : randomNumber,
                Children = person.Child.Select(c => new ChildDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    Birthday = c.Birthday,
                    Age = c.Birthday.HasValue
                        ? DateTime.Today.Year - person.Birthday.Value.Year
                        : randomNumber,
                    PersonId = person.Id
                })
            };

            Logger.LogInformation($"Person with id {id} successfully received.");

            return personDto;
        }

        public async Task<IEnumerable<PersonDTO>> GetAllPersons()
        {
            if (!CacheManager.GetCacheMemoryObject(CacheKeys.PersonListKey, out IEnumerable<PersonDTO> personList))
            {
                var persons = await PersonRepo.GetAllPersons().ConfigureAwait(false);

                var randomNumber = await NumberGenerator.GetRandomNumbers().ConfigureAwait(false);

                personList = persons.Select(p => new PersonDTO
                {
                    ID = p.Id,
                    Name = p.Name,
                    Surname = p.Surname,
                    Age = p.Birthday.HasValue
                        ? DateTime.Today.Year - p.Birthday.Value.Year
                        : randomNumber,
                    Birthday = p.Birthday,
                    Children = p.Child.Select(c => new ChildDTO
                    {
                        Id = c.Id,
                        PersonId = p.Id,
                        Name = c.Name,
                        Surname = c.Surname,
                        Age = c.Birthday.HasValue
                            ? DateTime.Today.Year - c.Birthday.Value.Year
                            : randomNumber,
                        Birthday = c.Birthday
                    })
                });

                CacheManager.SetMemory(CacheKeys.PersonListKey, personList);
            }

            Logger.LogInformation($"Get all successfully received.");

            return personList;
        }
    }
}
