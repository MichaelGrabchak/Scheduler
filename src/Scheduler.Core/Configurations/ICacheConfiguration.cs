namespace Scheduler.Core.Configurations
{
    public interface ICacheConfiguration
    {
        int? CacheExpiration { get; set; }
    }
}
