using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.IntegrationTest.HttpClients.Interfaces;

namespace Visma.FamilyTree.IntegrationTest.HttpClients.Implementation
{
    public class FamilyTreeClient : HttpClient, IFamilyTreeClient
    {
        private const string ContentType = "application/json";

        public FamilyTreeClient()
        {
            base.BaseAddress = new Uri(FamilyTreePaths.BaseUrl);
        }

        private JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public async Task<PersonDTO> PostPerson(PersonDTO personDTO)
        {
            var request = JsonConvert.SerializeObject(personDTO, JsonSerializerSettings);
            var requestContent = new StringContent(request, Encoding.UTF8, ContentType);

            var response = await this.PostAsync(FamilyTreePaths.Person, requestContent)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<PersonDTO>(responseContent, JsonSerializerSettings)
                : throw new Exception("Family Three SVC unavailable");
        }

        public async Task<ChildDTO> PostChild(Guid personId, ChildDTO childDTO)
        {
            var request = JsonConvert.SerializeObject(childDTO, JsonSerializerSettings);
            var requestContent = new StringContent(request, Encoding.UTF8, ContentType);

            var response = await this.PostAsync(FamilyTreePaths.PostChildLink(personId), requestContent)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<ChildDTO>(responseContent, JsonSerializerSettings)
                : throw new Exception("Family Three SVC unavailable");
        }

        public async Task<PersonDTO> PutPerson(PersonDTO personDTO)
        {
            var request = JsonConvert.SerializeObject(personDTO, JsonSerializerSettings);
            var requestContent = new StringContent(request, Encoding.UTF8, ContentType);

            var response = await this.PutAsync(FamilyTreePaths.PersonById(personDTO.ID), requestContent)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<PersonDTO>(responseContent, JsonSerializerSettings)
                : throw new Exception("Family Three SVC unavailable");
        }

        public async Task<ChildDTO> PutChild(Guid personId, ChildDTO childDTO)
        {
            var request = JsonConvert.SerializeObject(childDTO, JsonSerializerSettings);
            var requestContent = new StringContent(request, Encoding.UTF8, ContentType);

            var response = await this.PutAsync(FamilyTreePaths.PutChild(personId, childDTO.Id), requestContent)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<ChildDTO>(responseContent, JsonSerializerSettings)
                : throw new Exception("Family Three SVC unavailable");
        }

        public async Task<IEnumerable<PersonDTO>> GetPersons()
        {
            var response = await this.GetAsync(FamilyTreePaths.Person)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(responseContent, JsonSerializerSettings)
                : throw new Exception("Family Three SVC unavailable");
        }

        public async Task<PersonDTO> GetPerson(Guid personId)
        {
            var response = await this.GetAsync(FamilyTreePaths.PersonById(personId))
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<PersonDTO>(responseContent, JsonSerializerSettings)
                : throw new Exception("Family Three SVC unavailable");
        }
    }
}
