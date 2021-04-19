using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.RandomNumberGenerator.Interfaces;
using Visma.FamilyTree.Repository.Interfaces;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;

namespace Visma.FamilyTree.WebAPI.Managers.Implementation
{
    public class ChildManager : IChildManager
    {
        public ChildManager(
            IChildRepo childRepo,
            IPersonRepo personRepo,
            INumberGeneratorClient numberGenerator,
            ILogger<ChildManager> logger)
        {
            ChildRepo = childRepo;
            PersonRepo = personRepo;
            NumberGenerator = numberGenerator;
            Logger = logger;
        }

        private ILogger<ChildManager> Logger { get; }

        private IChildRepo ChildRepo { get; }

        private IPersonRepo PersonRepo { get; }

        private INumberGeneratorClient NumberGenerator { get; }

        public async Task<ChildDTO> AddChild(Guid personId, ChildDTO child)
        {
            var personEntity = await PersonRepo.GetPerson(personId).ConfigureAwait(false);

            if (personEntity == null)
                return null;

            child.Id = Guid.NewGuid();
            child.PersonId = personId;

            await ChildRepo.AddChild(personId, child).ConfigureAwait(false);

            child.Age = child.Birthday.HasValue
                ? DateTime.Today.Year - child.Birthday.Value.Year
                : await NumberGenerator.GetRandomNumbers().ConfigureAwait(false);

            Logger.LogInformation($"Child with id {child.Id} successfully created.");

            return child;
        }

        public async Task<ChildDTO> UpdateChild(Guid personId, Guid childId, ChildDTO child)
        {
            var personEntity = await PersonRepo.GetPerson(personId).ConfigureAwait(false);
            var childEntity = await ChildRepo.GetChild(childId).ConfigureAwait(false);

            if (personEntity == null || childEntity == null)
                return null;

            await ChildRepo.UpdateChild(personId, childId, child).ConfigureAwait(false);

            child.PersonId = personId;
            child.Age = child.Birthday.HasValue
                ? DateTime.Today.Year - child.Birthday.Value.Year
                : await NumberGenerator.GetRandomNumbers().ConfigureAwait(false);

            Logger.LogInformation($"Child with id {child.Id} successfully updated.");

            return child;
        }
    }
}
