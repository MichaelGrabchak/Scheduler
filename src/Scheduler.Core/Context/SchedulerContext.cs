using System;
using System.Configuration;

namespace Scheduler.Core.Context
{
    public class SchedulerContext : IContext
    {
        public Guid InstanceId { get; }

        public string ConnectionString { get; }

        public SchedulerContext()
        {
            InstanceId = new Guid(ConfigurationManager.AppSettings.Get(Constants.System.InstanceIdKey));

            ConnectionString = ConfigurationManager.ConnectionStrings[Constants.System.DataWarehouse.ConnectionStringKey].ConnectionString;
        }
    }
}
