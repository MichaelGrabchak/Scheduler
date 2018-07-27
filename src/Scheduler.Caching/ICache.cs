namespace Scheduler.Caching
{
    public interface ICache
    {
        bool IsCached(string key);

        T Get<T>(string key);

        void Set(string key, object value);

        void Remove(string key);
    }
}
