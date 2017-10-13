using Scheduler.Core;
using Scheduler.Core.Jobs;

namespace MyAmazing.TaskLib
{
    public class HelloWorldJob : BaseJob
    {
        public override string Schedule => Constants.Scheduler.Frequency.Cron.EveryMinute;

        public override void ExecuteJob()
        {
            LogInfo("What a nice guy...");
        }
    }
}
