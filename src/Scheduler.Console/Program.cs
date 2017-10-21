using System;
using System.Timers;
using System.Threading;

using Scheduler.Core;
using Scheduler.Core.Engine;
using Scheduler.Core.Logging;
using Scheduler.Core.Configurations;
using Scheduler.Engine.Quartz;
using Scheduler.Infrastructure.Configuration;

using Scheduler.Logging.NLog;

using Microsoft.Practices.Unity;

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
            container.RegisterType<BaseLogger, NLogLogger>(new InjectionConstructor(Constants.System.DefaultSchedulerLoggerName));
            container.RegisterType<SchedulerSettings, SchedulerConsoleSettings>();
            container.RegisterType<ISchedulerEngine, QuartzScheduler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(SchedulerSettings)));

            GlobalUnity.Container = container;

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
