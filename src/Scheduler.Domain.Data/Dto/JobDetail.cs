using System;

namespace Scheduler.Domain.Data.Dto
{
    public class JobDetail
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public string JobDescription { get; set; }
        public string JobSchedule { get; set; }
        public DateTime? JobLastRunTime { get; set; }
        public DateTime? JobNextRunTime { get; set; }
        public byte StatusId { get; set; }
        public Guid InstanceId { get; set; }
    }
}
