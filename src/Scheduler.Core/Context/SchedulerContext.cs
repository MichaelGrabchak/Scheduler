using System.Configuration;

namespace Scheduler.Core.Context
{
    public class SchedulerContext : ISchedulerContext
    {
        public string InstanceId => ConfigurationManager.AppSettings.Get("SchedulerInstanceId");

        public string ConnectionString => ConfigurationManager.ConnectionStrings["SchedulerDWH"].ConnectionString;
    }
}
