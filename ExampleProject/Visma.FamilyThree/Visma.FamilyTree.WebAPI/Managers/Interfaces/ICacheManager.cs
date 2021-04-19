namespace Visma.FamilyTree.WebAPI.Managers.Interfaces
{
    public interface ICacheManager
    {
        bool GetCacheMemoryObject<T>(string key, out T cacheObject);

        void SetMemory<T>(string key, T cacheObject);

        void CleanCachedItem(string key);


    }
}
