using System.ComponentModel;

using Library.BusinessLayer.Services;

using Scheduler.Core.Jobs;

namespace Library
{
    [Description("The custom job which serialize complex object (Expected result: Success, Execution Time: 10seconds)")]
    public class CustomJob : BaseJob
    {
        private readonly SomeService _someService;

        public CustomJob() 
            : base(loggerName: "CustomJobLogger")
        {
            _someService = new SomeService();
        }
        public override string Schedule => "0 0/1 * 1/1 * ? *";

        public override void ExecuteJob()
        {
            LogInfo("Starting execution of Custom Job");

            var result = _someService.DoSomething();
            LogDebug($"The result of execution: {result}");

            LogInfo("The execution has been completed successfully");
        }
    }
}
