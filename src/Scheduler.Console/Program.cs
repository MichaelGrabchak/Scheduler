using System;
using System.Timers;
using System.Threading;

using Scheduler.Engine;
using Scheduler.Engine.Quartz;
using Scheduler.Logging.NLog;
using Scheduler.Console.Configurations;

using Microsoft.Practices.Unity;

using Scheduler.Dependencies;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;
using Scheduler.Logging.NLog.Loggers;

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

    internal class Program
    {
        private static void InitContainer()
        {
            // Register a class that continues the program
            Container.RegisterType<ProgramStarter, ProgramStarter>();

            // Custom stuff
            Container.RegisterType<ILogger, NLogLogger>();
            Container.RegisterType<ILoggerProvider, LoggerProvider>();
            Container.RegisterType<SchedulerSettings, SchedulerConsoleSettings>();
            Container.RegisterType<ISchedulerEngine, QuartzScheduler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(SchedulerSettings)));
        }

        private static void Main(string[] args)
        {
            InitContainer();

            var program = Container.Resolve<ProgramStarter>();

            program.Run();
        }
    }
}
