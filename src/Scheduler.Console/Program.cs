using System;
using System.Timers;
using System.Threading;

using Scheduler.Console.Dependencies;
using Scheduler.Engine;
using Scheduler.Core.Dependencies;

namespace Scheduler.Console
{
    public class ProgramStarter
    {
        private readonly ISchedulerEngine _engine;

        public ProgramStarter(ISchedulerEngine engine)
        {
            _engine = engine;
        }

        public void Run()
        {
            System.Console.WriteLine("Starting the Console...");

            ShowScheduledJobs(null, null);

            System.Timers.Timer aTimer = new System.Timers.Timer() { Interval = TimeSpan.FromMinutes(1).TotalMilliseconds, AutoReset = true };
            aTimer.Elapsed += ShowScheduledJobs;
            aTimer.Start();
            
            Thread.Sleep(Timeout.Infinite);
        }

        private void ShowScheduledJobs(object source, ElapsedEventArgs e)
        {
            System.Console.WriteLine("-Rediscovering the jobs...");
            _engine.Discover();

            System.Console.WriteLine("Scheduled jobs are:");

            foreach (var type in _engine.GetAllJobs())
            {
                System.Console.WriteLine($"- Job '{type.Name}' ('{type.Group}' group)");
            }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var dependenciesManager = new ConsoleDependencyManager();
            dependenciesManager.RegisterDependencies();

            Container.Resolve<ProgramStarter>().Run();
        }
    }
}
