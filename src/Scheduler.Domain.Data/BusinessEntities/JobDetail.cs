using System;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class JobDetail
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public string JobDescription { get; set; }
        public string JobSchedule { get; set; }
        public DateTimeOffset? JobLastRunTime { get; set; }
        public DateTimeOffset? JobNextRunTime { get; set; }
        public byte StatusId { get; set; }
        public Guid InstanceId { get; set; }

        public virtual JobStatus Status { get; set; }
        public virtual SchedulerInstance Instance { get; set; }
    }
}
