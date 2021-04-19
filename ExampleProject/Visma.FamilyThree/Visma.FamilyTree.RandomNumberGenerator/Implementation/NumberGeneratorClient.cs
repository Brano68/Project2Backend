using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Visma.FamilyTree.Composition;
using Visma.FamilyTree.RandomNumberGenerator.Interfaces;

namespace Visma.FamilyTree.RandomNumberGenerator.Implementation
{
    public class NumberGeneratorClient : HttpClient, INumberGeneratorClient
    {
        public NumberGeneratorClient(
            IOptions<AppConfiguration> options)
        {
            AppConfiguration = options.Value;
            this.BaseAddress = new Uri(AppConfiguration.RandomNumberGenerator);
        }

        private AppConfiguration AppConfiguration { get; }

        public async Task<int> GetRandomNumbers()
        {
            var httpQuery = HttpUtility.ParseQueryString(this.BaseAddress.Query);

            foreach (var query in AppConfiguration.RandomGeneratorQueryParams)
            {
                httpQuery[query.Key] = query.Value.ToString();
            }

            var response = await this.GetAsync($"?{httpQuery}").ConfigureAwait(false);

            return await GetResponseNumber(response.Content).ConfigureAwait(false);
        }

        private async Task<int> GetResponseNumber(HttpContent httpContent)
        {
            string responseString = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
            return int.TryParse(responseString, out int number)
                ? number
                : 0;
        }
    }
}
