using Scheduler.Core.Dependencies;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;

namespace Library.Console
{
    public class ProgramStarter
    {
        private readonly CustomJob _job;

        public ProgramStarter(CustomJob job)
        {
            _job = job;
        }

        public void Run()
        {
            System.Console.WriteLine("Starting the Console...");

            _job.ExecuteJob();

            System.Console.WriteLine("The execution has finished.");

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
