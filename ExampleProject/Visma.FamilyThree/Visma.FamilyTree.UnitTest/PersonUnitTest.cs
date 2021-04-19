using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.RandomNumberGenerator.Interfaces;
using Visma.FamilyTree.Repository.Interfaces;
using Visma.FamilyTree.UnitTest.Data;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;
using Xunit;

namespace Visma.FamilyTree.UnitTest
{
    public class PersonUnitTest
    {
        public PersonUnitTest()
        {
            PersonRepoMock = new Mock<IPersonRepo>();

            CacheManagerMock = new Mock<ICacheManager>();

            NumberGeneratorMock = new Mock<INumberGeneratorClient>();

            ChildRepoMock = new Mock<IChildRepo>();

            //PersonManagerTest = new PersonManager(
            //    PersonRepoMock.Object,
            //    ChildRepoMock.Object,
            //    CacheManagerMock.Object,
            //    NumberGeneratorMock.Object);
        }

        private IPersonManager PersonManagerTest { get; }

        private Mock<IPersonRepo> PersonRepoMock { get; }

        private Mock<ICacheManager> CacheManagerMock { get; }

        private Mock<IChildRepo> ChildRepoMock { get; }

        private Mock<INumberGeneratorClient> NumberGeneratorMock { get; }

        [Fact(DisplayName = "Post Person Success Test")]
        public async Task PostPersonTest()
        {
            PersonRepoMock.Setup(r => r.AddPerson(It.IsAny<PersonDTO>()));

            NumberGeneratorMock.Setup(n => n.GetRandomNumbers()).Returns(Task.FromResult(37));

            var testedPersonDto = await PersonManagerTest.AddPerson(PersonData.MockPerson).ConfigureAwait(false);

            NumberGeneratorMock.Verify(pr => pr.GetRandomNumbers(), Times.Never);

            Assert.NotEqual(new Guid(), testedPersonDto.ID);
            Assert.Equal(PersonData.MockPerson.Name, testedPersonDto.Name);
            Assert.Equal(PersonData.MockPerson.Surname, testedPersonDto.Surname);
            Assert.Equal(PersonData.MockPerson.Birthday, testedPersonDto.Birthday);
        }

        [Fact(DisplayName = "Get Persons Success Test When data are not cached")]
        public async Task GetPersonsTest()
        {
            NumberGeneratorMock.Setup(n => n.GetRandomNumbers()).Returns(Task.FromResult(37));

            PersonRepoMock.Setup(r => r.GetAllPersons()).Returns(Task.FromResult(PersonData.MockPersonEntityList));

            IEnumerable<PersonDTO> mockPersonList = new List<PersonDTO>();

            CacheManagerMock.Setup(c => c.GetCacheMemoryObject(
                It.IsAny<string>(),
                out mockPersonList)).Returns(false);

            CacheManagerMock.Setup(c => c.SetMemory(
                It.IsAny<string>(),
                mockPersonList));

            NumberGeneratorMock.Setup(n => n.GetRandomNumbers()).Returns(Task.FromResult(37));

            var testedPersonList = await PersonManagerTest.GetAllPersons().ConfigureAwait(false);

            var testedPerson = testedPersonList.FirstOrDefault();
            var testedChild = testedPerson.Children.First();

            PersonRepoMock.Verify(pr => pr.GetAllPersons(), Times.Once);
            NumberGeneratorMock.Verify(ng => ng.GetRandomNumbers(), Times.Once);
            CacheManagerMock.Verify(cm => cm.GetCacheMemoryObject(It.IsAny<string>(), out mockPersonList), Times.Once);
            CacheManagerMock.Verify(cm => cm.SetMemory(It.IsAny<string>(), It.IsAny<IEnumerable<PersonDTO>>()), Times.Once);

            Assert.True(testedPersonList.Any());
            Assert.True(testedPerson.Children.Any());
            Assert.Equal(testedPerson.ID, testedChild.PersonId);
        }
    }
}
