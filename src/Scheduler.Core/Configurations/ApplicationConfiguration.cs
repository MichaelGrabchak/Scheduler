using Scheduler.Core.Attributes;

namespace Scheduler.Core.Configurations
{
    public class ApplicationConfiguration : IEngineConfiguration, IApplicationConfiguration
    {
        [Key("Application:Name")]
        public string ApplicationName { get; set; }

        [Key("Engine:JobsDirectory")]
        public string JobsDirectory { get; set; }

        [Key("Engine:IsImmediateStartEnabled")]
        public bool IsImmediateEngineStartEnabled { get; set; }

        [Key("Engine:IsJobsDirectoryTrackingEnabled")]
        public bool IsJobsDirectoryTrackingEnabled { get; set; }
    }
}
