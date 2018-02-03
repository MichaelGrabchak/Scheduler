using MyAmazing.TaskLib;

using Scheduler.Core.Configurations;
using Scheduler.Core.Logging;
using Scheduler.Logging.NLog;

using Microsoft.Practices.Unity;

namespace MyAmazing.Console
{
    public class ProgramStarter
    {
        private readonly HelloWorldJob _helloWorldJob;
        private readonly GoodbyeWorldJob _goodbyeWorldJob;

        public ProgramStarter(HelloWorldJob helloWorldJob, GoodbyeWorldJob goodbyeWorldJob)
        {
            _helloWorldJob = helloWorldJob;
            _goodbyeWorldJob = goodbyeWorldJob;
        }

        public void Run()
        {
            System.Console.WriteLine("Starting the Console...");

            _helloWorldJob.ExecuteJob();
            _goodbyeWorldJob.ExecuteJob();

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
            container.RegisterType<HelloWorldJob, HelloWorldJob>();
            container.RegisterType<GoodbyeWorldJob, GoodbyeWorldJob>();
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
