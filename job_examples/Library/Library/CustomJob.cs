using Library.BusinessLayer.Services;

using Scheduler.Jobs;
using Scheduler.Jobs.Attributes;
using Scheduler.Logging;

namespace Library
{
    [JobMetadata(Description = "The custom job which serialize complex object (Expected result: Success, Execution Time: 10seconds)", Logger = "CustomJobLogger")]
    public class CustomJob : BaseJob
    {
        private readonly ILogger _logger;
        private readonly SomeService _someService;

        public CustomJob(SomeService someService, ILoggerProvider loggerProvider) 
        {
            _someService = someService;
            _logger = loggerProvider.GetLogger("CustomJobLogger");
        }

        public override string Schedule => "0 0/1 * 1/1 * ? *";

        public override void ExecuteJob()
        {
            _logger.Info("Starting execution of Custom Job");

            var result = _someService.DoSomething();
            _logger.Debug($"The result of execution: {result}");

            _logger.Info("The execution has been completed successfully");
        }
    }
}
