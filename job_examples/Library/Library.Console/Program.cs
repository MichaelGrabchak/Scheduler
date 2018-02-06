using Scheduler.Core.Logging;
using Scheduler.Core.Configurations;
using Scheduler.Logging.NLog;
using Scheduler.Jobs;

using Microsoft.Practices.Unity;

namespace Library.Console
{
    public class ProgramStarter
    {
        private readonly BaseJob _job;

        public ProgramStarter(BaseJob job)
        {
            _job = job;
        }

        public void Run()
        {
            System.Console.WriteLine("Starting the Console...");

            _job.ExecuteJob();

            System.Console.ReadLine();
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
            container.RegisterType<BaseJob, CustomJob>();
            container.RegisterType<ISchedulerLogger, NLogLogger>();

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
