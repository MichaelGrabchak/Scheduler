namespace Scheduler.Caching.Configurations
{
    public interface ICacheConfiguration
    {
        int? CacheExpiration { get; }
    }
}
