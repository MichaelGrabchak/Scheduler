using System;

using Scheduler.Core;
using Scheduler.Core.Logging;
using Scheduler.Jobs.Attributes;
using Scheduler.Jobs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MyAmazing.TaskLib
{
    [JobMetadata(Name = "Hello World Job", Group = "Another group")]
    public class HelloWorldJob : BaseJob
    {
        public readonly ISchedulerLogger Logger = SchedulerLogManager.GetSchedulerLogger();

        public override string Schedule => Constants.Scheduler.Frequency.Cron.EveryMinute;

        public override void ExecuteJob()
        {
            Logger.Info("What a nice guy...");

            var test = new JTokenWriter();
            var test2 = test.GetType().GetProperty("CurrentToken");

            var complexObject = new
            {
                Name = "ComplexObject",
                ExpiryDate = new DateTime(2017, 7, 7),
                AttributeNames = new[] { "attr1", "attr2", "attr3" },
                IsCurrentPropertyExist = (test2 != null)
            };

            Logger.Info($"Serialized object: {JsonConvert.SerializeObject(complexObject)}");
        }
    }
}
