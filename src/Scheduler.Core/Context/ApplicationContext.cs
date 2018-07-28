using System;
using System.Configuration;

namespace Scheduler.Core.Context
{
    public class ApplicationContext : IApplicationContext
    {
        public Guid InstanceId { get; }

        public ApplicationContext()
        {
            InstanceId = new Guid(ConfigurationManager.AppSettings.Get(Constants.System.InstanceIdKey));
        }
    }
}
