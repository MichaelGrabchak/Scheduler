namespace Scheduler.Caching
{
    public class NoCache : ICache
    {
        public bool IsCached(string key)
        {
            return false;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public void Set(string key, object value)
        {
            
        }

        public void Remove(string key)
        {
            
        }
    }
}
