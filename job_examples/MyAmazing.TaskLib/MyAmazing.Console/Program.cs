using System.Threading.Tasks;

using MyAmazing.TaskLib;

using Scheduler.Dependencies;
using Scheduler.Logging;

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

            Task.Run(() =>
            {
                _helloWorldJob.ExecuteJob();
            });

            Task.Run(() =>
            {
                _goodbyeWorldJob.ExecuteJob();
            });

            System.Console.ReadLine();
        }
    }

    internal class Program
    {
        private static void InitContainer()
        {
            // Register a class that continues the program
            Container.RegisterType<ProgramStarter, ProgramStarter>();

            // Custom stuff
            Container.RegisterType<ILogger, Logger>();
            Container.RegisterType<ILoggerProvider, LoggerProvider>();
        }

        private static void Main(string[] args)
        {
            InitContainer();

            var program = Container.Resolve<ProgramStarter>();

            program.Run();
        }
    }
}
