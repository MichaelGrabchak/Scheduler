using Scheduler.Console.Configurations;
using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Engine;

namespace Scheduler.Console.Dependencies.Configurations
{
    public class ConsoleAppDependencyConfiguration : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<SchedulerSettings, SchedulerConsoleSettings>();
            Container.RegisterType<ProgramStarter, ProgramStarter>();
        }
    }
}
