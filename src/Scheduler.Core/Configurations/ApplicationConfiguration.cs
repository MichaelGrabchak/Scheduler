using Scheduler.Core.Attributes;

namespace Scheduler.Core.Configurations
{
    public class ApplicationConfiguration : ICacheConfiguration, IEngineConfiguration
    {
        [Key("Caching:Expiration")]
        public int? CacheExpiration { get; set; }

        [Key("Engine:JobsDirectory")]
        public string JobsDirectory { get; set; }

        [Key("Engine:IsImmediateStartEnabled")]
        public bool IsImmediateEngineStartEnabled { get; set; }

        [Key("Engine:IsJobsDirectoryTrackingEnabled")]
        public bool IsJobsDirectoryTrackingEnabled { get; set; }
    }
}
