using Scheduler.Core.Jobs;
using System;
using System.Threading;

namespace Library
{
    public class CustomJob : BaseJob
    {
        public CustomJob() 
            : base(loggerName: "CustomJobLogger")
        {

        }
        public override string Schedule => "0 0/1 * 1/1 * ? *";

        public override void ExecuteJob()
        {
            LogDebug("Going to sleep");

            Thread.Sleep(10000);

            LogDebug("Woke up");
        }
    }
}
