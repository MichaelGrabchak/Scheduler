using Library.BusinessLayer.Services;

using Scheduler.Jobs;
using Scheduler.Jobs.Attributes;

namespace Library
{
    [JobMetadata(Description = "The custom job which serialize complex object (Expected result: Success, Execution Time: 10seconds)", Logger = "CustomJobLogger")]
    public class CustomJob : BaseJob
    {
        private readonly SomeService _someService;

        public CustomJob(SomeService someService) 
        {
            _someService = someService;
        }

        public override string Schedule => "0 0/1 * 1/1 * ? *";

        public override void ExecuteJob()
        {
            Logger.Info("Starting execution of Custom Job");

            var result = _someService.DoSomething();
            Logger.Debug($"The result of execution: {result}");

            Logger.Info("The execution has been completed successfully");
        }
    }
}
