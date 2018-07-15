using System.Threading.Tasks;

using MyAmazing.TaskLib;

using Scheduler.Core.Dependencies;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;
using Scheduler.Logging.NLog.Loggers;

namespace MyAmazing.Console
{
    public class ProgramStarter
    {
        private readonly ILogger _logger;

        private readonly HelloWorldJob _helloWorldJob;
        private readonly GoodbyeWorldJob _goodbyeWorldJob;

        public ProgramStarter(HelloWorldJob helloWorldJob, GoodbyeWorldJob goodbyeWorldJob, ILoggerProvider loggerProvider)
        {
            _helloWorldJob = helloWorldJob;
            _goodbyeWorldJob = goodbyeWorldJob;

            _logger = loggerProvider.GetLogger();
        }

        public void Run()
        {
            _logger.Info("Starting the Console...");

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
            Container.RegisterType<ILogger, NLogLogger>(Constants.LoggerNames.DefaultLogger);
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
