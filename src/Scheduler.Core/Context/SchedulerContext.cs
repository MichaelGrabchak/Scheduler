using System;
using System.Configuration;

namespace Scheduler.Core.Context
{
    public class SchedulerContext : ISchedulerContext
    {
        private string RawInstanceId = ConfigurationManager.AppSettings.Get("SchedulerInstanceId");
        public Guid InstanceId { get; }

        public string ConnectionString => ConfigurationManager.ConnectionStrings["SchedulerDWH"].ConnectionString;

        public SchedulerContext()
        {
            InstanceId = new Guid(RawInstanceId);
        }
    }
}
