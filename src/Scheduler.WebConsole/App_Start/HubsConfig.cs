using Scheduler.WebConsole.Dependencies;

namespace Scheduler.WebConsole
{
    public class HubsConfig
    {
        public static void RegisterHubs()
        {
            HubSchedulerDependencies.Configure();
        }   
    }
}
