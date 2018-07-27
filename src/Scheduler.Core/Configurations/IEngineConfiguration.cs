namespace Scheduler.Core.Configurations
{
    public interface IEngineConfiguration
    {
        string JobsDirectory { get; set; }

        bool IsImmediateEngineStartEnabled { get; set; }

        bool IsJobsDirectoryTrackingEnabled { get; set; }
    }
}
