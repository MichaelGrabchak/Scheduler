namespace Scheduler.Domain.Data.Dto
{
    public class InstanceSettings
    {
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }

        public bool IsImmediateEngineStartEnabled { get; set; }
        public bool IsJobsDirectoryTrackingEnabled { get; set; }

        public string JobsDirectory { get; set; }
    }
}
