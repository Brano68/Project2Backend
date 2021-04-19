using System.Threading.Tasks;

namespace Visma.FamilyTree.RandomNumberGenerator.Interfaces
{
    public interface INumberGeneratorClient
    {
        Task<int> GetRandomNumbers();
    }
}
