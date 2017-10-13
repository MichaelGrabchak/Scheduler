using System;
using System.Timers;
using System.Threading;

using Scheduler.Core.Engine;
using Scheduler.Engine.Quartz;

using Microsoft.Practices.Unity;
using Scheduler.Infrastructure.Configuration;

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
            aTimer.Elapsed += new ElapsedEventHandler(ShowScheduledJobs);
            aTimer.Start();
            
            Thread.Sleep(Timeout.Infinite);
        }

        private void ShowScheduledJobs(object source, ElapsedEventArgs e)
        {
            System.Console.WriteLine("Scheduled jobs are:");

            foreach (var type in _engine.GetAllJobs())
            {
                System.Console.WriteLine($"- Job '{type.Name}' ('{type.Group}' group)");
            }
        }
    }

    class Program
    {
        private static IUnityContainer InitContainer()
        {
            var container = new UnityContainer();

            // Register a class that continues the program
            container.RegisterType<ProgramStarter, ProgramStarter>();

            // Custom stuff
            container.RegisterType<SchedulerSettings, SchedulerConsoleSettings>();
            container.RegisterType<ISchedulerEngine, QuartzScheduler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(SchedulerSettings)));

            return container;
        }

        static void Main(string[] args)
        {
            var container = InitContainer();
            var program = container.Resolve<ProgramStarter>();
            program.Run();
        }
    }
}
