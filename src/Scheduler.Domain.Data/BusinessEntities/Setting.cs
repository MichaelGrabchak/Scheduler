using System;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Guid InstanceId { get; set; }
        public virtual Instance Instance { get; set; }
    }
}
