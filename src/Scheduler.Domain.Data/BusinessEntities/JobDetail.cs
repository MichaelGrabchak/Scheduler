using System;

using Scheduler.Domain.Data.Attributes;
using Scheduler.Domain.Data.Enums;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class JobDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public string JobDescription { get; set; }
        public string JobSchedule { get; set; }
        public DateTime JobLastRunTime { get; set; }
        public DateTime JobNextRunTime { get; set; }
        [Column("STATUS_ID")]
        public byte StatusId { get; set; }
    }
}
