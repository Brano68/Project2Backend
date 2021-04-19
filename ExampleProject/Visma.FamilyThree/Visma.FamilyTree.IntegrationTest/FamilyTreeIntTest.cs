using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Visma.FamilyTree.IntegrationTest.Data;
using Visma.FamilyTree.IntegrationTest.DependencyInjection;
using Visma.FamilyTree.IntegrationTest.HttpClients.Interfaces;
using Xunit;

namespace Visma.FamilyTree.IntegrationTest
{
    public class FamilyTreeIntTest
    {
        [Fact(DisplayName = "Create Person Add Child Get Data")]
        public async Task CreateAndGetSuccess()
        {
            var familyTreeClient = ServiceLocator.Container.Resolve<IFamilyTreeClient>();

            var testPerson = await familyTreeClient.PostPerson(PersonData.MockPostPerson).ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var testChild = await familyTreeClient.PostChild(testPerson.ID, PersonData.MockPostChild).ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var persons = await familyTreeClient.GetPersons().ConfigureAwait(false);

            Assert.Contains(persons, p =>
            p.ID == testPerson.ID
            && p.Name == testPerson.Name
            && p.Surname == testPerson.Surname
            && p.Birthday == testPerson.Birthday);

            Assert.Contains(persons.FirstOrDefault(p => p.ID == testPerson.ID)?.Children, c =>
            c.Id == testChild.Id
            && c.Name == testChild.Name
            && c.Surname == testChild.Surname
            && c.Birthday == testChild.Birthday);
        }

        [Fact(DisplayName = "Create Person Add Child and Update Data")]
        public async Task CreateAndPutSuccess()
        {
            var familyTreeClient = ServiceLocator.Container.Resolve<IFamilyTreeClient>();

            var testPerson = await familyTreeClient.PostPerson(PersonData.MockPostPerson).ConfigureAwait(false);

            // 15 sec due to when SVC is restarted 1 connection to Azure DB takes more time
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var testChild = await familyTreeClient.PostChild(testPerson.ID, PersonData.MockPostChild).ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var putPerson = PersonData.MockPutPerson;
            putPerson.ID = testPerson.ID;

            testPerson = await familyTreeClient.PutPerson(putPerson).ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var putChild = PersonData.MockPutChild;
            putChild.Id = testChild.Id;
            testChild = await familyTreeClient.PutChild(testPerson.ID, PersonData.MockPutChild).ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var person = await familyTreeClient.GetPerson(testPerson.ID).ConfigureAwait(false);

            Assert.True(person != null);
            Assert.Equal(person.ID, testPerson.ID);
            Assert.Equal(person.Name, PersonData.MockPutPerson.Name);
            Assert.Equal(person.Surname, PersonData.MockPutPerson.Surname);
            Assert.Equal(person.Birthday, PersonData.MockPutPerson.Birthday);
            Assert.Contains(person.Children, c => c.Id == testChild.Id);

            var child = person.Children.FirstOrDefault(c => c.Id == testChild.Id);

            Assert.Equal(child.Name, PersonData.MockPutChild.Name);
            Assert.Equal(child.Surname, PersonData.MockPutChild.Surname);
            Assert.Equal(child.Birthday, PersonData.MockPutChild.Birthday);
            Assert.Equal(child.PersonId, person.ID);
        }
    }
}
