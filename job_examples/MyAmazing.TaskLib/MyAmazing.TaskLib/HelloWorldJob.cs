using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Scheduler.Jobs;
using Scheduler.Jobs.Attributes;
using Scheduler.Logging;

namespace MyAmazing.TaskLib
{
    [JobMetadata(Name = "Hello World Job", Group = "Another group")]
    public class HelloWorldJob : BaseJob
    {
        private readonly ILogger _logger;

        public HelloWorldJob(ILoggerProvider loggerProvider)
        {
            _logger = loggerProvider.GetLogger();
        }

        public override string Schedule => "0 0/1 * 1/1 * ? *";

        public override void ExecuteJob()
        {
            _logger.Info("What a nice guy...");

            var test = new JTokenWriter();
            var test2 = test.GetType().GetProperty("CurrentToken");

            var complexObject = new
            {
                Name = "ComplexObject",
                ExpiryDate = new DateTime(2017, 7, 7),
                AttributeNames = new[] { "attr1", "attr2", "attr3" },
                IsCurrentPropertyExist = (test2 != null)
            };

            _logger.Info($"Serialized object: {JsonConvert.SerializeObject(complexObject)}");
        }
    }
}
